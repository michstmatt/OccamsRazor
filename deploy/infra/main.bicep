param webAppName string = '' // Generate unique String for web app name
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
          name:  'dockerRegistryUrl'
          value: 'https://${acrResource.properties.loginServer}'
        }
        {
          name:  'dockerRegistryUserName'
          value: acrResource.listCredentials().username
        }
        {
          name:  'dockerRegistryPassword'
          value: acrResource.listCredentials().passwords[0].value
        }
      ]
    }
  }
}
