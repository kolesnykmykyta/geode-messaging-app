param location string = deployment().location
targetScope = 'subscription'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'geode-rg'
  location: location
}

module storageAccount './storage.bicep' = {
  scope: resourceGroup
  name: 'geode-storage-account'
  params: {
    location: location
  }
}

module keyVault './key-vault.bicep' = {
  scope: resourceGroup
  name: 'geode-key-vault'
  params: {
    location: location
  }
}

module appServicePlan './app-service-plan.bicep' = {
  scope: resourceGroup
  name: 'geode-app-plan'
  params: {
    location: location
  }
}

module appService './web-app.bicep' = {
  scope: resourceGroup
  name: 'geode-api'
  params: {
    location: location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
    keyVaultUrl: keyVault.outputs.keyVaultUrl
  }
}

module database './database.bicep' = {
  scope: resourceGroup
  name: 'geode-database'
  params: {
    location: location
  }
}
