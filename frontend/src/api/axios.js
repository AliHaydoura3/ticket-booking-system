import axios from "axios";

const apiClient = axios.create({
  baseURL: "https://ticket-booking-system-api-d9atfsehd8ajddey.germanywestcentral-01.azurewebsites.net/api",
});

apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem("authToken");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default apiClient;
