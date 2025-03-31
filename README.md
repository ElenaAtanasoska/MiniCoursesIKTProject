# MiniCoursesIKTProject - Development Setup

## Running the Database with Docker Compose (Development)

To start only the PostgreSQL database for local development, use the following command:

```bash
docker-compose -f compose.dev.yml up
```

This will start the database in a Docker container, allowing you to connect to it from your local development environment.

### Make sure to run migrations (migrate and update)

```bash
dotnet ef database update
```

## Running the Full Application with Docker Compose

To build and run the complete application (including the database) using Docker Compose, use the following command:

```bash
docker-compose up
```

This command will:  
Build the Docker image for the application.
Start the PostgreSQL database container.
Start the application container, connecting it to the database.
The application will be accessible at http://localhost:8080.
