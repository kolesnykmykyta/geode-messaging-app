param location string
param objectId string

@secure()
param connectionString string
@secure()
param jwtSettings object

resource keyVault 'Microsoft.KeyVault/vaults@2019-09-01' = {
  name: 'geode-key-vault'
  location: location
  properties: {
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: true
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: objectId
        permissions: {
          secrets: [
            'list'
            'get'
          ]
        }
      }
    ]
    sku: {
      name: 'standard'
      family: 'A'
    }
  }
}

resource dbConnectionSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'ConnectionStrings--Default'
  properties: {
    value: connectionString
  }
}

resource jwtIssuerSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'Jwt--Issuer'
  properties: {
    value: jwtSettings.issuer
  }
}

resource jwtAudienceSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'Jwt--Audience'
  properties: {
    value: jwtSettings.audience
  }
}

resource jwtKeySecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  parent: keyVault
  name: 'Jwt--Key'
  properties: {
    value: jwtSettings.key
  }
}


output keyVaultUrl string = keyVault.properties.vaultUri
