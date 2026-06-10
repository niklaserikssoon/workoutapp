# ADR Documentation

# ADR — Architecture Decision Record

**Author:** Niklas Eriksson

**Date:** 2026-06-07

---

# Context

As part of the K5 project, the team needed to further develop and deploy a full-stack application in Azure. This document describes the design decisions made regarding hosting, CI/CD, security, Key Vault, monitoring, and AI integration.

---

# Decision 1: Hosting

## Options Considered

* Azure App Service
* Azure Container Apps
* Azure Virtual Machine

## Decision

Azure Container Apps was selected.

## Reasoning

Container Apps enables container-based deployment, automatic scaling, and seamless integration with other Azure resources. Since the backend was already running in Container Apps, this created a consistent and unified architecture.

## Consequences

* Simple deployment through Docker images
* Strong support for scaling
* Requires Azure Container Registry

---

# Decision 2: CI/CD Pipeline

## Decision

GitHub Actions is used for automated build and deployment.

## Pipeline Steps

1. Code is pushed to GitHub
2. Tests are executed
3. A Docker image is built
4. The image is pushed to Azure Container Registry
5. The Azure Container App is updated

## Reasoning

Automation reduces the risk of manual errors and creates a smooth development workflow.

---

# Decision 3: Security Strategy

## Decision

Secrets are stored outside the application, and access is managed through Azure RBAC and Managed Identity.

## Reasoning

Sensitive information should never be hardcoded in the source code.

### Security Measures

* HTTPS
* RBAC
* Managed Identity
* GitHub Secrets
* Azure Key Vault
* JWT-based authentication

### Authentication

The application uses JWT tokens for authentication. When a user logs in, a token is generated and included in subsequent API requests. Protected features, such as AI-generated workout plans, require a valid token and can therefore only be accessed by authenticated users.

---

# Decision 4: Key Vault and Identity

## Decision

Azure Key Vault is used for secure secret storage.

## Reasoning

Azure Key Vault stores API keys and other sensitive information securely instead of keeping them directly in the codebase.

## Stored Secrets

* JWT Secret
* Database Connection String
* AI API Key

## Identity

Managed Identity is used to grant the application access to Key Vault without storing credentials.

---

# Decision 5: Monitoring

## Decision

Azure Application Insights is used for monitoring.

## Reasoning

Provides visibility into performance, errors, and application usage.

## Monitored Data

* Exceptions
* Response Times
* Availability
* Requests

---

### Decision 6: AI Integration

### Decision

Groq was used to generate workout plans and provide AI-related functionality within the application.

### Reasoning

Groq was primarily chosen because it offers a free tier and a simple API. Based on our own testing, we concluded that Groq's free version was sufficient for our intended use case.

### Flow

1. The user submits a request in the application.
2. The backend receives and processes the request.
3. A request is sent to the Groq API.
4. Groq generates a response.
5. The response is returned to the user and displayed in the interface.

### Security

* The Groq API key is stored in Azure Key Vault.
* The API key is never exposed to the frontend.
* All communication takes place over HTTPS.
* Only the backend has access to the Groq API.

### Consequences

**Positive:**

* Fast AI responses
* Simple API integration
* Ability to generate personalized workout recommendations

**Negative:**

* Dependency on an external AI service
* AI-generated responses may occasionally be inaccurate or require adjustment
* Responses may sometimes contain unusual wording or phrasing that can be difficult to interpret

---

# Summary

The solution uses Azure Container Apps for hosting, GitHub Actions for CI/CD, Azure Key Vault and Managed Identity for security, Application Insights for monitoring, and Groq for AI functionality. These decisions provide a secure, scalable, and maintainable solution.
