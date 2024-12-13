export const environment = {
  production: true,
  apiUrlToken: (window["env"] && window["env"]["apiUrlToken"]) || '',
  apiUrlFoodtrucks: (window["env"] && window["env"]["apiUrlFoodtrucks"]) || '',
  azureMapKey: (window["env"] && window["env"]["azureMapKey"]) || ''
};