param location string = deployment().location

@secure()
param dbAdminSettings object
@secure()
param jwtSettings object
@secure()
param objectId string
@secure()
param clientId string
@secure()
param clientSecret string

targetScope = 'subscription'

resource resourceGroup 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'geode-rg'
  location: location
}

module storageAccount 'storage.bicep' = {
  scope: resourceGroup
  name: 'geodestorageaccount'
  params: {
    location: location
  }
}

module database 'database.bicep' = {
  scope: resourceGroup
  name: 'geode-database'
  params: {
    location: location
    adminName: dbAdminSettings.adminName
    adminPassword: dbAdminSettings.adminPassword
  }
}

module keyVault 'key-vault.bicep' = {
  scope: resourceGroup
  name: 'geode-key-vault'
  params: {
    location: location
    dbConnectionString: database.outputs.connectionString
    storageConnectionString: storageAccount.outputs.connectionString
    jwtSettings: jwtSettings
    objectId: objectId
  }
}

module appServicePlan 'app-service-plan.bicep' = {
  scope: resourceGroup
  name: 'geode-app-plan'
  params: {
    location: location
  }
}

module appService 'web-api.bicep' = {
  scope: resourceGroup
  name: 'geode-web-app'
  params: {
    location: location
    appServicePlanId: appServicePlan.outputs.appServicePlanId
    keyVaultUrl: keyVault.outputs.keyVaultUrl
    tenantId: subscription().tenantId
    clientId: clientId
    clientSecret: clientSecret
  }
}
