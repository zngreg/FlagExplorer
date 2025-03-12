import axios from "axios";
import API_BASE_URL from "../config";

export const getAllCountries = async () => {
  const response = await axios.get(`${API_BASE_URL}/countries`);
  return response.data;
};

export const getCountryDetails = async (countryName: string) => {
  const response = await axios.get(`${API_BASE_URL}/countries/${countryName}`);
  return response.data;
};