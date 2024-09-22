PhoneBookAPI
A simple .NET 6.0-based phone book API for managing contacts, built with Docker, PostgreSQL, and Redis.

Table of Contents
About
Technologies
Requirements
Setup & Run
With Docker Compose
Running Unit Tests
API Documentation
Available Endpoints
Swagger
Development
Contributing
License
About
The PhoneBook API is a RESTful service for managing contacts. The API supports creating, updating, deleting, and searching contacts. It uses:

.NET 6.0 as the web framework.
PostgreSQL as the database for storing contacts.
Redis for caching contact data.
Technologies
.NET 6.0
Entity Framework Core
PostgreSQL
Redis
AutoMapper
Docker & Docker Compose
Requirements
Docker
Docker Compose
Setup & Run
With Docker Compose
Clone the repository:

bash
Copy code
git clone https://github.com/your-repo/PhoneBookAPI.git
cd PhoneBookAPI
Build and run the Docker containers:

bash
Copy code
docker-compose up --build
Access the API:

The API will be available at: http://localhost:5000
Swagger documentation is available at: http://localhost:5000/index.html
