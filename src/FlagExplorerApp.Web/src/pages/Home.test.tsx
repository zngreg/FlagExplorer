import { render, screen } from "@testing-library/react";
import Home from "./Home";

jest.mock("../components/CountryGrid", () => ({
  __esModule: true,
  default: () => <div data-testid="country-grid-mock" />
}));

describe("Home Component", () => {
  test("should display the heading 'Country List'", () => {
    render(<Home />);
    expect(screen.getByText("Country List")).toBeInTheDocument();
  });

  test("should render the CountryGrid component", () => {
    render(<Home />);
    expect(screen.getByTestId("country-grid-mock")).toBeInTheDocument();
  });
});