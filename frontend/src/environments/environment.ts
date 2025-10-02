declare const window: any;

export const environment = {
  production: false,
  apiUrl: window.ENV?.API_URL
    ? window.ENV.API_URL + '/api/v1.0'
    : 'https://localhost:7051/api/v1.0',
};
