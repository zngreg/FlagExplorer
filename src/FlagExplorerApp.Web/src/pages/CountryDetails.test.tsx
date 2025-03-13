import { render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter, Route, Routes } from "react-router-dom";
import CountryDetails from "./CountryDetails";
import * as countryApi from "../services/CountryApi";

describe("CountryDetails", () => {
  beforeEach(() => {
    jest.restoreAllMocks();
  });

  test("should show loading message initially", () => {
    jest.spyOn(global, "fetch").mockResolvedValue(
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve(null),
      } as Response)
    );

    render(
      <MemoryRouter initialEntries={["/country/Germany"]}>
        <Routes>
          <Route path="/country/:name" element={<CountryDetails />} />
        </Routes>
      </MemoryRouter>
    );

    expect(screen.getByText("Loading...")).toBeInTheDocument();
  });

  test("should display country details when API call succeeds", async () => {
    const mockCountry = {
      name: "Germany",
      flag: "https://restcountries.com/data/deu.svg",
      population: 83166711,
      capital: "Berlin",
    };

    jest.spyOn(global, "fetch").mockResolvedValue(
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve(mockCountry),
      } as Response)
    );

    render(
      <MemoryRouter initialEntries={["/country/Germany"]}>
        <Routes>
          <Route path="/country/:name" element={<CountryDetails />} />
        </Routes>
      </MemoryRouter>
    );

    await waitFor(() => {
      expect(screen.getByText("Germany")).toBeInTheDocument();
      expect(screen.getByText("Population: 83,166,711")).toBeInTheDocument();
      expect(screen.getByText("Capital: Berlin")).toBeInTheDocument();
      expect(screen.getByRole("img", { name: "Germany" })).toHaveAttribute(
        "src",
        "https://restcountries.com/data/deu.svg"
      );
    });
  });

  test("should show error message if API call fails", async () => {
    jest.spyOn(countryApi, "getCountryDetails").mockRejectedValue(new Error("API error"));

    render(
      <MemoryRouter initialEntries={["/country/Germany"]}>
        <Routes>
          <Route path="/country/:name" element={<CountryDetails />} />
        </Routes>
      </MemoryRouter>
    );

    await waitFor(() =>
      expect(
        screen.getByText("Error fetching country details. Please try again later.")
      ).toBeInTheDocument()
    );
  });

  test("should show 'Country not found' if API returns null", async () => {
    jest.spyOn(global, "fetch").mockResolvedValue(
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve(null),
      } as Response)
    );

    render(
      <MemoryRouter initialEntries={["/country/Unknown"]}>
        <Routes>
          <Route path="/country/:name" element={<CountryDetails />} />
        </Routes>
      </MemoryRouter>
    );

    await waitFor(() =>
      expect(screen.getByText("Country not found")).toBeInTheDocument()
    );
  });
});