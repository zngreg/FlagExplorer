import { MemoryRouter } from "react-router-dom";
import { render, screen } from "@testing-library/react";
import CountryCard from "./CountryCard";

describe("CountryCard", () => {
  test("should render a link with the country name encoded in the URL", () => {
    render(
      <MemoryRouter>
        <CountryCard name="Germany" flag="https://restcountries.com/data/deu.svg" />
      </MemoryRouter>
    );

    const link = screen.getByRole("link");
    expect(link).toHaveAttribute("href", "/country/Germany");
  });

  test("should have a background image matching the flag prop", () => {
    const flagUrl = "https://restcountries.com/data/deu.svg";

    render(
      <MemoryRouter>
        <CountryCard name="Germany" flag={flagUrl} />
      </MemoryRouter>
    );

    const link = screen.getByRole("link");

    expect(link).toHaveStyle(`background-image: url(${flagUrl})`);
  });
});