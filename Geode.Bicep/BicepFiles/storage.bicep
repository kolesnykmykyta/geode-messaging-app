param location string

resource storageaccount 'Microsoft.Storage/storageAccounts@2023-04-01' = {
  name: 'geode-storage-account'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    accessTier: 'Hot'
    allowBlobPublicAccess: true
    publicNetworkAccess: 'Enabled'
  }
}

resource blobservice 'Microsoft.Storage/storageAccounts/blobServices@2023-04-01' = {
  name: 'default'
  parent: storageaccount
}

resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-04-01' = {
  name: 'geode'
  parent: blobservice
  properties:{
    publicAccess: 'Blob'
  }
}
