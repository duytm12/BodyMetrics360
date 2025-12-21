using Microsoft.Extensions.Configuration;

namespace TestProject;

public class TestDbConnectionString
{
    private static string GetWebAppPath()
    {
        var current = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(current))
        {
            var candidate = Path.Combine(current, "WebApp");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            current = Directory.GetParent(current)?.FullName;
        }

        throw new DirectoryNotFoundException("Could not locate WebApp folder relative to test output.");
    }

    [Fact]
    public void DbConnectionString_CanBeReadFromConfiguration()
    {
        // Arrange - Build configuration similar to WebApp
        var webAppPath = GetWebAppPath();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(webAppPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets("BodyMetrics360-WebApp-Secrets") // Match UserSecretsId from WebApp.csproj
            .AddEnvironmentVariables()
            .Build();

        // Act - Get connection string
        var connectionString = configuration.GetConnectionString("DbConnectionString");

        // Assert - Verify connection string is configured
        Assert.NotNull(connectionString);
        Assert.NotEmpty(connectionString);

        // Verify it contains expected SQL Server connection string parts
        Assert.Contains("Data Source", connectionString);
        Assert.Contains("Initial Catalog", connectionString);
    }

    [Fact]
    public void DbConnectionString_IsNotInAppSettingsJson()
    {
        // Arrange - Build configuration without user secrets
        var webAppPath = GetWebAppPath();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(webAppPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        // Act - Get connection string (should be empty from appsettings.json)
        var connectionString = configuration.GetConnectionString("DbConnectionString");

        // Assert - Should be null or empty (since we removed it from appsettings.json)
        // This verifies that the connection string is NOT stored in appsettings.json
        Assert.True(string.IsNullOrEmpty(connectionString),
            "Connection string should not be in appsettings.json for security");
    }

    [Fact]
    public void DbConnectionString_CanBeReadFromUserSecrets()
    {
        // Arrange - Build configuration with user secrets only
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets("BodyMetrics360-WebApp-Secrets")
            .Build();

        // Act - Get connection string
        var connectionString = configuration.GetConnectionString("DbConnectionString");

        // Assert - Verify connection string is loaded from user secrets
        Assert.NotNull(connectionString);
        Assert.NotEmpty(connectionString);
        Assert.Contains("Data Source", connectionString);
    }

    [Fact]
    public void DbConnectionString_ConfigurationPriority_UserSecretsOverrideAppSettings()
    {
        // Arrange - Build configuration with both appsettings and user secrets
        // User secrets should override appsettings.json
        var webAppPath = GetWebAppPath();
        var configuration = new ConfigurationBuilder()
            .SetBasePath(webAppPath)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets("BodyMetrics360-WebApp-Secrets")
            .AddEnvironmentVariables()
            .Build();

        // Act - Get connection string
        var connectionString = configuration.GetConnectionString("DbConnectionString");

        // Assert - User secrets should provide the value (not the empty one from appsettings.json)
        Assert.NotNull(connectionString);
        Assert.NotEmpty(connectionString);

        // Verify it's a valid SQL Server connection string
        Assert.Contains("Data Source", connectionString);
        Assert.Contains("Initial Catalog", connectionString);
    }
}

