using System.Net;
using bmi2021;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bmiUnitTestProject;

[TestClass]
public class ApplicationTests
{
    [TestMethod]
    [DataRow("Development")]
    [DataRow("Production")]
    public async Task HomePage_LoadsInEachEnvironment(string environment)
    {
        using var host = Program.CreateHostBuilder([])
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseEnvironment(environment);
                webBuilder.UseTestServer();
            })
            .Build();

        await host.StartAsync();
        using var client = host.GetTestClient();
        client.BaseAddress = new Uri("https://localhost");
        using var response = await client.GetAsync("/");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "Check your body mass index");
    }

    [TestMethod]
    public async Task PrivacyPage_ExplainsHowMeasurementsAreHandled()
    {
        using var host = Program.CreateHostBuilder([])
            .ConfigureWebHost(webBuilder => webBuilder.UseTestServer())
            .Build();

        await host.StartAsync();
        using var client = host.GetTestClient();
        client.BaseAddress = new Uri("https://localhost");
        using var response = await client.GetAsync("/Privacy");
        var html = await response.Content.ReadAsStringAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        StringAssert.Contains(html, "does not create an account");
    }
}
