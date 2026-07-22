# DevOps Lab 02 — GitHub Actions

## Objective

Create a practical GitHub Actions workflow for a .NET 8 solution that restores packages, builds the application, runs tests, and publishes artifacts.

## Concepts Covered

- GitHub Actions
- Workflow triggers
- Build steps
- Test execution
- Artifact publishing

## Folder Structure

```
DevOpsLab02-GitHubActions/
├── README.md
└── ../../.github/workflows/dotnet.yml
```

## Prerequisites

- GitHub account
- .NET 8 SDK
- Repository with a .NET solution

## Installation

Create a repository and push the project to GitHub. The workflow will run automatically on pushes and pull requests.

## How to Run

Open the workflow file and review the steps for restore, build, test, and publish. Commit the changes and inspect the Actions tab.

## Expected Output

You should see the workflow run successfully and generate a publishable artifact.

## Learning Outcomes

- Configure a GitHub Actions workflow
- Explain each workflow stage
- Publish build outputs as artifacts
