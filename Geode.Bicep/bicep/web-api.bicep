param location string
param appServicePlanId string
param angularAppUrl string

@secure()
param keyVaultUrl string
@secure()
param tenantId string
@secure()
param clientId string
@secure()
param clientSecret string

resource webApplication 'Microsoft.Web/sites@2023-12-01' = {
  name: 'geode-web-app'
  location: location
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/appServicePlan': 'Resource'
  }
  properties: {
    serverFarmId: appServicePlanId
    siteConfig:{
      ftpsState: 'AllAllowed'
      cors:{
        allowedOrigins: [
          'https://0.0.0.0'
          'http://localhost:4200:'
          angularAppUrl
        ]
        supportCredentials: true
      }
      appSettings: [
        {
          name: 'KeyVaultConfig__KeyVaultUrl'
          value: keyVaultUrl
        }
        {
          name: 'KeyVaultConfig__TenantId'
          value: tenantId
        }
        {
          name: 'KeyVaultConfig__ClientId'
          value: clientId
        }
        {
          name: 'KeyVaultConfig__ClientSecret'
          value: clientSecret
        }
      ]
    }
  } 
}

