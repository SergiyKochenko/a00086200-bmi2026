using System.ComponentModel.DataAnnotations;
using BMICalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bmiUnitTestProject;

[TestClass]
public class BmiTests
{
    [TestMethod]
    public void BmiValue_ConvertsImperialMeasurements()
    {
        var bmi = CreateBmi(12, 0, 5, 10);

        Assert.AreEqual(24.1, Math.Round(bmi.BMIValue, 1));
    }

    [TestMethod]
    [DataRow(8, 0, 6, 0, BMICategory.Underweight)]
    [DataRow(12, 0, 5, 10, BMICategory.Normal)]
    [DataRow(14, 0, 5, 10, BMICategory.Overweight)]
    [DataRow(15, 0, 5, 10, BMICategory.Obese)]
    public void BmiCategory_MatchesTheCalculatedValue(
        int stone,
        int pounds,
        int feet,
        int inches,
        BMICategory expected)
    {
        var bmi = CreateBmi(stone, pounds, feet, inches);

        Assert.AreEqual(expected, bmi.BMICategory);
    }

    [TestMethod]
    [DataRow(5, 0, 4, 0)]
    [DataRow(50, 13, 7, 11)]
    public void Validation_AllowsBoundaryValues(int stone, int pounds, int feet, int inches)
    {
        var errors = Validate(CreateBmi(stone, pounds, feet, inches));

        Assert.HasCount(0, errors);
    }

    [TestMethod]
    [DataRow(nameof(BMI.WeightStones), 4, "Stone must be between 5 and 50")]
    [DataRow(nameof(BMI.WeightStones), 51, "Stone must be between 5 and 50")]
    [DataRow(nameof(BMI.WeightPounds), -1, "Pounds must be between 0 and 13")]
    [DataRow(nameof(BMI.WeightPounds), 14, "Pounds must be between 0 and 13")]
    [DataRow(nameof(BMI.HeightFeet), 3, "Feet must be between 4 and 7")]
    [DataRow(nameof(BMI.HeightFeet), 8, "Feet must be between 4 and 7")]
    [DataRow(nameof(BMI.HeightInches), -1, "Inches must be between 0 and 11")]
    [DataRow(nameof(BMI.HeightInches), 12, "Inches must be between 0 and 11")]
    public void Validation_RejectsMeasurementsOutsideTheirRange(
        string propertyName,
        int value,
        string expectedMessage)
    {
        var bmi = CreateBmi(12, 0, 5, 10);
        typeof(BMI).GetProperty(propertyName)!.SetValue(bmi, value);

        var errors = Validate(bmi);

        Assert.HasCount(1, errors);
        Assert.AreEqual(expectedMessage, errors[0].ErrorMessage);
        CollectionAssert.Contains(errors[0].MemberNames.ToArray(), propertyName);
    }

    [TestMethod]
    public void Validation_RequiresAllFourMeasurements()
    {
        var errors = Validate(new BMI());

        Assert.HasCount(4, errors);
    }

    private static BMI CreateBmi(int stone, int pounds, int feet, int inches) =>
        new()
        {
            WeightStones = stone,
            WeightPounds = pounds,
            HeightFeet = feet,
            HeightInches = inches
        };

    private static List<ValidationResult> Validate(BMI bmi)
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(bmi, new ValidationContext(bmi), errors, true);
        return errors;
    }
}
