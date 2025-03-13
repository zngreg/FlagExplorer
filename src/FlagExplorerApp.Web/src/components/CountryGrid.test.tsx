import { render, screen, waitFor } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import CountryGrid from "./CountryGrid";

describe("CountryGrid Component", () => {
  test("should display country cards when API call succeeds", async () => {
    const mockCountries = [
      { name: "Germany", flag: "https://restcountries.com/data/deu.svg" },
      { name: "France", flag: "https://restcountries.com/data/fra.svg" }
    ];

    global.fetch = jest.fn(() =>
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve(mockCountries),
      } as Response)
    );

    render(
      <MemoryRouter>
        <CountryGrid />
      </MemoryRouter>
    );

    expect(screen.getByText("Loading countries...")).toBeInTheDocument();

      await waitFor(() => {  
        const germanyElement = screen.getByLabelText("Germany");
        expect(germanyElement).toHaveAttribute("href", "/country/Germany");

        const franceElement = screen.getByLabelText("France");
        expect(franceElement).toHaveAttribute("href", "/country/France");
    });
  });

  test("should display error message when API call fails", async () => {
    global.fetch = jest.fn(() =>
      Promise.resolve({
        ok: false,
        statusText: "Internal Server Error",
      } as Response)
    );

    render(
      <MemoryRouter>
        <CountryGrid />
      </MemoryRouter>
    );

    expect(screen.getByText("Loading countries...")).toBeInTheDocument();

    await waitFor(() => {
      expect(screen.getByText("Error fetching countries. Please try again later.")).toBeInTheDocument();
    });
  });
});