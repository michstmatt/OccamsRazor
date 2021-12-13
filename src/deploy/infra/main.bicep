param webAppName string = '' // Generate unique String for web app name
param sqlPassword string
param sku string = 'P1V2' // The SKU of App Service Plan
param linuxFxVersion string = '' // The runtime stack of web app
param location string = 'eastus2' // Location for all resources
param acrName string = 'occamsrazor'
param sqlName string = 'occamsrazormaria'
param dbName string = 'trivia'
param sqlUser string = 'mariadbadmin'
var appServicePlanName = toLower('ASP-${webAppName}')
var dbSize = 6144

resource acrResource 'Microsoft.ContainerRegistry/registries@2021-06-01-preview' = {
  name: acrName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
  }
}

resource maria 'Microsoft.DBforMariaDB/servers@2018-06-01' = {
  name: sqlName
  location: location
  sku: {
    capacity: 2
    family: 'Gen5'
    name: 'GP_Gen5_2'
    size: '${dbSize}'
    tier: 'GeneralPurpose'
  }
  properties: {
    minimalTlsVersion: 'TLS1_2'
    publicNetworkAccess: 'Enabled'
    sslEnforcement: 'Enabled'
    storageProfile: {
      backupRetentionDays: 0
      geoRedundantBackup: 'Disabled'
      storageAutogrow: 'Enabled'
      storageMB: dbSize
    }
    version: '10.2'
    createMode: 'Default'
    administratorLogin: sqlUser
    administratorLoginPassword: sqlPassword
  }
}

resource database 'Microsoft.DBforMariaDB/servers/databases@2018-06-01' = {
  name: '${sqlName}/${dbName}'
  dependsOn: [
    maria
  ]
}

resource appServicePlan 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanName
  location: location
  properties: {
    reserved: true
  }
  sku: {
    name: sku
  }
  kind: 'linux'
}

resource appService 'Microsoft.Web/sites@2020-06-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: linuxFxVersion
      appSettings:[
        {
          name:  'DOCKER_REGISTRY_SERVER_URL'
          value: 'https://${acrResource.properties.loginServer}'
        }
        {
          name:  'DOCKER_REGISTRY_SERVER_USERNAME'
          value: acrResource.listCredentials().username
        }
        {
          name:  'DOCKER_REGISTRY_SERVER_PASSWORD'
          value: acrResource.listCredentials().passwords[0].value
        }
        {
          name: 'WEBSITES_ENABLE_APP_SERVICE_STORAGE'
          value: 'true'
        }
        {
          name: 'WEBSITES_PORT'
          value: '8080'
        }
        {
          name: 'CONNECTION_STRING'
          value: 'server=${sqlName}.mariadb.database.azure.com;Port=3306;Database=${dbName};user=${sqlUser}@${sqlName};Password=${sqlPassword};'
        }
        {
          name: 'MARIA_DB_VERSION'
          value: '10.2.0'
        }
      ]
    }
  }
  dependsOn:[
    appServicePlan
  ]
}
