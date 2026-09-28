using System;
using System.Net;
using System.Threading.Tasks;
using bmi2021;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bmiUnitTestProject;

[TestClass]
public class ApplicationTests
{
    [TestMethod]
    [DataRow("Development")]
    [DataRow("Production")]
    public async Task Application_StartsAndServesTheBmiPageInEveryPipeline(string environment)
    {
        using var host = Program.CreateHostBuilder(Array.Empty<string>())
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseEnvironment(environment);
                webBuilder.UseTestServer();
            })
            .Build();

        await host.StartAsync();
        using var response = await host.GetTestClient().GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "BMI calculator");
    }

    [TestMethod]
    public void Startup_ExposesItsConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();

        var startup = new Startup(configuration);

        Assert.AreSame(configuration, startup.Configuration);
    }
}
