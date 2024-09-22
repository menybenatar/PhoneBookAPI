# PhoneBookAPI

A simple .NET 6.0-based phone book API for managing contacts, built with Docker, PostgreSQL, and Redis.

## Table of Contents

- [About](#about)
- [Technologies](#technologies)
- [Requirements](#requirements)
- [Setup & Run](#setup--run)
  - [With Docker Compose](#with-docker-compose)
  - [Running Unit Tests](#running-unit-tests)
- [API Documentation](#api-documentation)
  - [Available Endpoints](#available-endpoints)
  - [Swagger](#swagger)
- [Development](#development)
- [Contributing](#contributing)
- [License](#license)

## About

The PhoneBook API is a RESTful service for managing contacts. The API supports creating, updating, deleting, and searching contacts. It uses:
- **.NET 6.0** as the web framework.
- **PostgreSQL** as the database for storing contacts.
- **Redis** for caching contact data.

## Technologies

- **.NET 6.0**
- **Entity Framework Core**
- **PostgreSQL**
- **Redis**
- **AutoMapper**
- **Docker & Docker Compose**

## Requirements

- [Docker](https://docs.docker.com/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)

## Setup & Run

### With Docker Compose

1. **Clone the repository:**
    ```bash
    git clone https://github.com/your-repo/PhoneBookAPI.git
    cd PhoneBookAPI
    ```

2. **Build and run the Docker containers:**
    ```bash
    docker-compose up --build
    ```

3. **Access the API:**
    - The API will be available at: `http://localhost:5000`
    - Swagger documentation is available at: `http://localhost:5000/swagger`

### Running Unit Tests

The unit tests for the project are written using xUnit. You can run the tests using the following command:

1. **In Visual Studio:**
    - Right-click on the test project `PhoneBookAPI.Tests` and select `Run Tests`.

2. **Or via CLI:**
    ```bash
    dotnet test
    ```

## API Documentation

The API provides a set of endpoints to manage contacts. Here's a brief overview of the available endpoints.

### Available Endpoints

1. **GET /api/contacts**
   - Retrieves a list of all contacts.
   - Supports pagination via query parameters: `pageNumber` and `pageSize`.

2. **GET /api/contacts/{id}**
   - Retrieves a single contact by its ID.

3. **POST /api/contacts**
   - Creates a new contact.

4. **PUT /api/contacts/{id}**
   - Updates an existing contact.

5. **DELETE /api/contacts/{id}**
   - Deletes a contact by its ID.

6. **GET /api/contacts/search?query={searchTerm}**
   - Searches for contacts by first name, last name, phone number, or address.

### Swagger

To explore the API in more detail, Swagger UI is available.

Once you have the server running, go to:
- **Swagger UI**: [http://localhost:5000/swagger](http://localhost:5000/swagger)
  
You can see all the available endpoints, input parameters, and make test API calls from there.

## Development

If you wish to run the application without Docker or for development:

1. **Install .NET SDK**: Make sure you have .NET SDK installed from [here](https://dotnet.microsoft.com/download).
2. **Run the application:**
    ```bash
    dotnet run --project PhoneBookAPI
    ```

## Contributing

Feel free to submit issues and feature requests. If you'd like to contribute, please fork the repository and submit a pull request.

## License

This project is open-source and available under the [MIT License](LICENSE).


