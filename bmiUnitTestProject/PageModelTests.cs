using System.Diagnostics;
using BMICalculator;
using BMICalculator.Pages;
using bmi2021.Pages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bmiUnitTestProject;

[TestClass]
public class PageModelTests
{
    [TestMethod]
    public void BmiPage_StoresTheBoundBmiModel()
    {
        var expected = new BMI();
        var page = new BmiModel { BMI = expected };

        Assert.AreSame(expected, page.BMI);
    }

    [TestMethod]
    [DataRow(null, false)]
    [DataRow("", false)]
    [DataRow("request-123", true)]
    public void ErrorPage_ShowRequestIdReflectsWhetherAnIdExists(string requestId, bool expected)
    {
        var page = CreateErrorModel();
        page.RequestId = requestId;

        Assert.AreEqual(expected, page.ShowRequestId);
    }

    [TestMethod]
    public void ErrorPage_OnGetUsesTheHttpTraceIdentifierWhenThereIsNoActivity()
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
    public void ErrorPage_OnGetPrefersTheCurrentActivityId()
    {
        using var activity = new Activity("unit-test").Start();
        var page = CreateErrorModel("trace-ignored");

        page.OnGet();

        Assert.AreEqual(activity.Id, page.RequestId);
    }

    [TestMethod]
    public void PrivacyPage_OnGetCompletesSuccessfully()
    {
        var page = new PrivacyModel(NullLogger<PrivacyModel>.Instance);

        page.OnGet();
    }

    private static ErrorModel CreateErrorModel(string traceIdentifier = "trace")
    {
        var context = new DefaultHttpContext { TraceIdentifier = traceIdentifier };
        return new ErrorModel(NullLogger<ErrorModel>.Instance)
        {
            PageContext = new PageContext { HttpContext = context }
        };
    }
}
