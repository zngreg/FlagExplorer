import { getAllCountries, getCountryDetails } from "./CountryApi";

const mockCountries = [
  { name: "Germany", flag: "https://restcountries.com/data/deu.svg" },
  { name: "France", flag: "https://restcountries.com/data/fra.svg" }
];

const mockCountryDetails = {
  name: "Germany",
  flag: "https://restcountries.com/data/deu.svg",
  population: 83000000,
  capital: "Berlin"
};

describe("Country API Service", () => {
  beforeEach(() => {
    global.fetch = jest.fn();
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  test("getAllCountries should return a list of countries", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: true,
      json: async () => mockCountries
    });

    const countries = await getAllCountries();

    expect(fetch).toHaveBeenCalledWith("http://localhost:5100/countries");
    expect(countries).toEqual(mockCountries);
  });

  test("getAllCountries should throw an error if response is not OK", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: false,
      statusText: "Internal Server Error"
    });

    await expect(getAllCountries()).rejects.toThrow("Error fetching countries: Internal Server Error");
  });

  test("getCountryDetails should return details of a specific country", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: true,
      json: async () => mockCountries[0]
    });

    const details = await getCountryDetails("Germany");

    expect(fetch).toHaveBeenCalledWith("http://localhost:5100/countries/Germany");
    expect(details).toEqual(mockCountries[0]);
  });

  test("getCountryDetails should throw an error if response is not OK", async () => {
    (fetch as jest.Mock).mockResolvedValueOnce({
      ok: false,
      statusText: "Not Found"
    });

    await expect(getCountryDetails("UnknownCountry"))
      .rejects.toThrow("Error fetching country details: Not Found");
  });
});