# PhoneBookAPI

A simple .NET 6.0-based phone book API for managing contacts, built with Docker, PostgreSQL, and Redis.

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
    git clone https://github.com/menybenatar/PhoneBookAPI.git
    cd PhoneBookAPI
    ```

2. **Build and run the Docker containers:**
    ```bash
    docker-compose up --build
    ```

3. **Access the API:**
    - The API will be available at: `http://localhost:5000`
    - Swagger documentation is available at: `http://localhost:5000/index.html`

### Running Unit Tests

The unit tests for the project are written using xUnit. You can run the tests using the following command:

 **In Visual Studio:**
   - Right-click on the test project `PhoneBookAPI.Tests` and select `Run Tests`.

## API Documentation

The API provides a set of endpoints to manage contacts. Here's a brief overview of the available endpoints.

### Available Endpoints

1. **GET /api/contacts**
   - Retrieves a list of all contacts.
   - Supports pagination via query parameters: `pageNumber` and `pageSize`.

2. **POST /api/contacts/AddContact**
   - Creates a new contact.

3. **PUT /api/contacts/EditContact**
   - Updates an existing contact.

4. **DELETE /api/contacts/DeleteContact{id}**
   - Deletes a contact by its ID.

5. **GET /api/contacts/search?query={searchTerm}**
   - Searches for contacts by first name, last name, phone number, or address.




