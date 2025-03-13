# FlagExplorerApp

## Overview

FlagExplorerApp is an API that provides country data. It fetches data from external APIs, caches it using memory cache.

## Getting Started

### Prerequisites

- .NET 9.0 SDK

### Configuration

1. **CountryApiBaseUrl**: Store your base url for.
2. **Redis**: Configure your Redis connection string in `appsettings.json`.

### Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/zngreg/FlagExplorer.git
   cd FlagExplorer
   ```

2. Restore the dependencies:

   ```sh
   dotnet restore
   ```

3. Build the project:
   ```sh
   dotnet build
   ```

### Running the Application

1. Update the `appsettings.Development.json` with your configuration.
2. Run the application:
   ```sh
   dotnet run
   ```

### API Endpoints

- **GET /api/countries**: Fetches countries data.
- **GET /api/countries/{name}**: Fetches country data for a given country name.

## Contributing

1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Commit your changes (`git commit -am 'Add new feature'`).
4. Push to the branch (`git push origin feature-branch`).
5. Create a new Pull Request.

For the instructions on how to use our Web Application **FlagExplorerApp.Web** please see [Getting Started with FlagExplorerApp.Web](src/FlagExplorerApp.Web/README.md)
