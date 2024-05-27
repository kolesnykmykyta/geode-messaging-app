param location string
param appServicePlanId string

param keyVaultUrl string

resource webApplication 'Microsoft.Web/sites@2023-12-01' = {
  name: 'geode-api'
  location: location
  tags: {
    'hidden-related:${resourceGroup().id}/providers/Microsoft.Web/serverfarms/appServicePlan': 'Resource'
  }
  properties: {
    serverFarmId: appServicePlanId
    siteConfig:{
      appSettings: [
        {
          name: 'KeyVaultConfig__KeyVaultUrl'
          value: keyVaultUrl
        }
      ]
    }
  }
  
}

