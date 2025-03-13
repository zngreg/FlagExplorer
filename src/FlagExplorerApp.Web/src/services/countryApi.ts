import API_BASE_URL from "../config";

export const getAllCountries = async () => {
  const response = await fetch(`${API_BASE_URL}/countries`);
  if (!response.ok) {
    throw new Error(`Error fetching countries: ${response.statusText}`);
  }
  return response.json();
};

export const getCountryDetails = async (countryName: string) => {
  const response = await fetch(`${API_BASE_URL}/countries/${countryName}`);
  if (!response.ok) {
    throw new Error(`Error fetching country details: ${response.statusText}`);
  }
  return response.json();
};