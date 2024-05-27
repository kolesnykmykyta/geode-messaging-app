param location string

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: 'geode-app-plan'
  location: location
  sku: {
    name: 'F1'
    size: 'F1'
    family: 'F'
    capacity: 0
  }
}

output appServicePlanId string = appServicePlan.id
