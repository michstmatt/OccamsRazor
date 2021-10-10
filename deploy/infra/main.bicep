param webAppName string = '' // Generate unique String for web app name
param sqlPassword string
param sku string = 'F1' // The SKU of App Service Plan
param linuxFxVersion string = '' // The runtime stack of web app
param location string = resourceGroup().location // Location for all resources
param acrName string = 'occamsrazor'
var appServicePlanName = toLower('ASP-${webAppName}')
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
          value: true
        }
        {
          name: 'WEBSITES_PORT'
          value: '8080'
        }
        {
          name: 'SQL_PASSWORD'
          value: sqlPassword
        }
      ]
    }
  }
}
