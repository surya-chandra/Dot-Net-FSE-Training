# DevOps

## Objective

This section introduces the practices and tools required to build, test, and deploy modern .NET applications in a reliable and repeatable way.

## Concepts Covered

- What is DevOps
- SDLC lifecycle
- CI/CD pipeline
- Git workflow
- Build automation
- Release pipeline
- Deployment concepts
- Azure DevOps overview
- GitHub Actions

## Folder Structure

```
DevOps/
├── DevOpsLab01-CI-CD-Basics/
├── DevOpsLab02-GitHubActions/
└── DevOpsLab03-BuildAndDeploy/
```

## Prerequisites

- .NET 8 SDK
- Git
- GitHub account
- Visual Studio Code or Visual Studio

## Installation

Install the .NET SDK and configure Git before running the sample workflow.

## How to Run

Open the workflow file in the repository and review each stage. Build the solution locally with:

```bash
dotnet build
```

## Expected Output

You should see the solution build successfully and the GitHub Actions workflow execute each stage in order.

## Learning Outcomes

- Understand DevOps fundamentals
- Explain CI/CD concepts
- Create a GitHub Actions workflow for a .NET project
- Publish build artifacts
