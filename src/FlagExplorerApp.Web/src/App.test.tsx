import { render, screen } from "@testing-library/react";
import App from "./App";

// Mock AppRoutes component
jest.mock("./routes", () => ({
  __esModule: true,
  default: () => <div data-testid="app-routes-mock" />,
}));

describe("App Component", () => {
  test("renders the AppRoutes component", () => {
    render(<App />);
    
    // Verify AppRoutes is rendered
    const appRoutesElement = screen.getByTestId("app-routes-mock");
    expect(appRoutesElement).toBeInTheDocument();
  });

  test("renders div with correct class names", () => {
    const { container } = render(<App />);
    
    const wrapperDiv = container.firstChild;
    expect(wrapperDiv).toHaveClass("min-h-screen bg-gray-100 text-gray-900");
  });
});