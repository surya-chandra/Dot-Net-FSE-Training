# Docker Lab 02 — Dockerizing a .NET API

## Objective

Containerize an ASP.NET Core 8 Web API using a Dockerfile with production-oriented configuration.

## Concepts Covered

- Dockerfile
- ASP.NET Core image layering
- Port mapping
- Environment variables
- Container execution

## Folder Structure

```
DockerLab02-DockerizingDotNetAPI/
├── Dockerfile
├── appsettings.json
├── README.md
```

## Prerequisites

- Docker Desktop
- .NET 8 SDK

## Installation

Create the API project and place the Dockerfile in the project root.

## How to Run

Build and run the container using the commands below:

```bash
docker build -t dotnet-api-demo .
docker run -d -p 8080:8080 --name dotnet-api-demo dotnet-api-demo
```

## Expected Output

The application should be accessible at http://localhost:8080.

## Learning Outcomes

- Create a Dockerfile for .NET 8
- Map local ports to container ports
- Run a containerized API successfully
