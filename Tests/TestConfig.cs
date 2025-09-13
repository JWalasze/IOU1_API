using Microsoft.Extensions.Configuration;

namespace Tests;

public static class TestConfig {
    public static IConfiguration InitConfiguration() {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // important for tests
            .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        return config;
    }
}
