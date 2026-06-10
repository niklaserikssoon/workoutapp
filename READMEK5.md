# Workout App
<img width="1872" height="863" alt="Skärmbild 2026-06-07 215143" src="https://github.com/user-attachments/assets/1e113559-3449-4fab-aebd-b49e4083efc8" />

[Workoutapp](https://ca-frontend-prod.wonderfulpebble-02d3e465.italynorth.azurecontainerapps.io/) Live site

## Architecture Overview

The application consists of:

* Frontend (HTML, CSS, JavaScript)
* User API (.NET)
* Workout API (.NET)
* Azure SQL Database
* Groq AI Integration
* Azure Container Apps

### Component Communication

The frontend communicates with the backend APIs through HTTP requests.

```text
Frontend
    |
    v
User API -------- Azure SQL Database
    |
    v
Workout API ------ Groq API & SQL Database
```

### Main Endpoints

#### User API

| Endpoint       | Description                                  |
| -------------- | -------------------------------------------- |
| POST /register | Register a new user                          |
| POST /login    | Authenticate a user and generate a JWT token |

#### Workout API

| Endpoint               | Description                                                |
| ---------------------- | ---------------------------------------------------------- |
| GET /workouts          | Retrieve saved workouts                                    |
| POST /generate-workout | Generate a personalized workout plan using AI              |
| POST /exercise         | Create and save a custom exercise                          |
| POST /workout          | Create and save a workout based on selected exercises      |

---

# Running the System Locally

## Requirements

* .NET 9 SDK
* Docker
* SQL Server
* Git

## Clone the Repository

```bash
git clone <repository-url>
```

## Start the Backend APIs

```bash
dotnet run
```

## Start the Frontend

Open the frontend project in your preferred local web server or run it through Visual Studio.

## Configuration

Add required values to:

```json
appsettings.Development.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  }
}
```

---

# CI/CD Pipeline

The project uses GitHub Actions for Continuous Integration and Continuous Deployment.

## Pipeline Triggers

The workflow is triggered when code is pushed to:

* main
* dev

## Pipeline Steps

1. Checkout repository
2. Build the application
3. Run tests
4. Build Docker images
5. Push images to Azure Container Registry
6. Deploy to Azure Container Apps

## Deployment

Deployments are performed automatically through GitHub Actions using Azure service principal credentials stored as GitHub Secrets.

---

# Secret Management

## Local Development

During development, secrets are stored using:

* appsettings.Development.json
* .NET User Secrets

## Production Environment

Sensitive values are stored in Azure Key Vault.

Examples:

* JWT Secret Key
* Database Connection Strings
* Groq API Key

The application retrieves secrets through Managed Identity instead of storing credentials directly in the application.

---

# Monitoring and Logging

## Monitoring

The system uses Azure Application Insights and Azure Monitor for monitoring and diagnostics.

## Logged Information

The following information is logged:

* Application errors
* API requests
* Exceptions
* Performance metrics

## Troubleshooting

If issues occur, check:

1. Azure Container App logs
2. GitHub Actions workflow logs
3. Application Insights
4. Azure Portal diagnostics

---

# AI Functionality

## Purpose

The AI feature generates personalized workout plans based on user input and preferences.

## Request Flow

1. The user submits a workout request.
2. The backend validates the user and JWT token.
3. The backend sends a request to the Groq API.
4. Groq generates a workout plan.
5. The response is returned to the frontend and displayed to the user.

## Security

* Only authenticated users can access AI functionality.
* A valid JWT token is required.
* The Groq API key is stored in Azure Key Vault.
* API keys are never exposed to the frontend.
* Communication is secured using HTTPS.
