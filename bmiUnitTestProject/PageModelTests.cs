using System.Diagnostics;
using BMICalculator;
using BMICalculator.Pages;
using bmi2021.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bmiUnitTestProject;

[TestClass]
public class PageModelTests
{
    [TestMethod]
    public void BmiPage_ShowsAResultForValidInput()
    {
        var page = new BmiModel
        {
            BMI = new BMI
            {
                WeightStones = 12,
                WeightPounds = 0,
                HeightFeet = 5,
                HeightInches = 10
            }
        };

        page.OnPost();

        Assert.IsTrue(page.HasResult);
    }

    [TestMethod]
    public void BmiPage_HidesTheResultWhenValidationFails()
    {
        var page = new BmiModel();
        page.ModelState.AddModelError("BMI.WeightStones", "Enter a weight");

        page.OnPost();

        Assert.IsFalse(page.HasResult);
    }

    [TestMethod]
    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("request-123", true)]
    public void ErrorPage_OnlyShowsARequestIdWhenOneExists(string? requestId, bool expected)
    {
        var page = CreateErrorModel();
        page.RequestId = requestId;

        Assert.AreEqual(expected, page.ShowRequestId);
    }

    [TestMethod]
    public void ErrorPage_UsesTheTraceIdentifierWhenThereIsNoActivity()
    {
        var previousActivity = Activity.Current;
        Activity.Current = null;

        try
        {
            var page = CreateErrorModel("trace-456");

            page.OnGet();

            Assert.AreEqual("trace-456", page.RequestId);
        }
        finally
        {
            Activity.Current = previousActivity;
        }
    }

    [TestMethod]
    public void ErrorPage_UsesTheCurrentActivityIdWhenAvailable()
    {
        using var activity = new Activity("test request").Start();
        var page = CreateErrorModel("trace-ignored");

        page.OnGet();

        Assert.AreEqual(activity.Id, page.RequestId);
    }

    private static ErrorModel CreateErrorModel(string traceIdentifier = "trace")
    {
        var context = new DefaultHttpContext { TraceIdentifier = traceIdentifier };
        return new ErrorModel
        {
            PageContext = new PageContext { HttpContext = context }
        };
    }
}
