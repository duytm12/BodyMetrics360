# BodyMetrics360

Production-ready body metrics calculator built with Clean Architecture. Calculates Body metrics like BMI, BMR (Body Maintenance Requirements), TDEE (Total Daily Energy Expenditure), BFP (Body Fat Percentage), LBM (Lean Body Mass), and WtHR (Waist to Height ratio) with personalized recommendations.

🌐 **Live Application**: [https://metrics360webapp-bmambyahbzfsgxd2.centralus-01.azurewebsites.net](https://metrics360webapp-bmambyahbzfsgxd2.centralus-01.azurewebsites.net)

## 🛠 Tech Stack

- **.NET 10.0** - ASP.NET Core MVC
- **Entity Framework Core 10** - SQL Server data access
- **xUnit** - Unit testing
- **Bootstrap 5** - Responsive UI

## 🏗 Architecture

**Clean Architecture** with strict layer separation:

```
Domain          → Entities, Interfaces, Business Logic (No dependencies)
Application     → Use Cases, DTOs, Application Services
Infrastructure  → EF Core, Repositories, External Services
WebApp          → Controllers, Views, Presentation Layer
```



## 📁 Project Structure

```
BodyMetrics360/
├── Domain/              # Core business logic, entities, interfaces
│   ├── Entities/        # Input, Output, Recommendation
│   ├── Interfaces/      # Repository & service contracts
│   └── Services/        # Domain services (GetBMI, GetRecommendation, etc.)
├── Application/         # Application layer
│   ├── UseCases/        # Business use cases
│   ├── DTOs/            # Data transfer objects
│   └── Services/        # Application services
├── Infrastructure/      # Data access & external services
│   ├── SQLServer/       # EF Core DbContext & repositories
│   └── Migrations/      # Database migrations
├── WebApp/              # Presentation layer
│   ├── Controllers/     # MVC controllers
│   └── Views/           # Razor views
└── TestProject/         # Unit tests (Domain, Application, Infrastructure)
```

## 🚀 Azure Deployment

### Infrastructure

- **Azure App Service** - Hosts ASP.NET Core application
- **Azure SQL Database** - Managed SQL Server instance
- **Azure Key Vault** - Secure connection strings & secrets
- **Application Insights** - Monitoring & diagnostics

### CI/CD Pipeline

```yaml
# GitHub Actions / Azure DevOps
1. Build → dotnet build
2. Test → dotnet test
3. Deploy → Azure App Service

# Note: Run migrations manually before deployment
# dotnet ef database update --project Infrastructure --startup-project WebApp
```

### Configuration

- Connection strings stored in Azure Key Vault
- Environment-specific `appsettings.Production.json`
- EF Core migrations run manually before deployment to create/update database schema
- Session state configured for distributed scenarios

## 🧪 Testing

- **Domain Tests** - Business logic validation
- **Application Tests** - Use case integration with in-memory repositories
- **Infrastructure Tests** - Repository operations with EF Core InMemory provider

## 📊 Features

- **Metrics Calculation** - BMI, BMR, TDEE, BFP, LBM, WtHR
- **Personalized Recommendations** - Category-based advice per metric
- **Session Management** - User-specific data tracking
- **Data Persistence** - Input/output history with recommendations

---

Built with Clean Architecture principles for maintainability, testability, and scalability.
