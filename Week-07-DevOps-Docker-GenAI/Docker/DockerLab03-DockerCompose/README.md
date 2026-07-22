# Docker Lab 03 — Docker Compose

## Objective

Use Docker Compose to orchestrate an API container and a SQL Server container with a persistent volume.

## Concepts Covered

- Docker Compose
- Container networking
- Environment variables
- Persistent storage
- Multi-container orchestration

## Folder Structure

```
DockerLab03-DockerCompose/
├── docker-compose.yml
├── README.md
```

## Prerequisites

- Docker Desktop
- Docker Compose plugin

## Installation

Ensure Docker Desktop is running and the Compose plugin is available.

## How to Run

```bash
docker compose up --build
docker compose down
```

## Expected Output

Both the API and SQL Server containers should start and remain connected through the Docker network.

## Learning Outcomes

- Create and run a Docker Compose file
- Understand service-based container orchestration
- Configure environment variables and volumes
