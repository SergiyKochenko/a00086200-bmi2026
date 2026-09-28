using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BMICalculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace bmiUnitTestProject;

[TestClass]
public class BmiTests
{
    [TestMethod]
    public void BMIValue_ConvertsImperialMeasurementsAndCalculatesBmi()
    {
        var bmi = CreateBmi(weightStones: 12, weightPounds: 0, heightFeet: 5, heightInches: 10);

        Assert.AreEqual(24.10522306758899, bmi.BMIValue, 0.00000000000001);
    }

    [TestMethod]
    [DataRow(8, 0, 6, 0, BMICategory.Underweight)]
    [DataRow(12, 0, 5, 10, BMICategory.Normal)]
    [DataRow(14, 0, 5, 10, BMICategory.Overweight)]
    [DataRow(15, 0, 5, 10, BMICategory.Obese)]
    public void BMICategory_ReturnsCategoryForEveryRange(
        int weightStones,
        int weightPounds,
        int heightFeet,
        int heightInches,
        BMICategory expectedCategory)
    {
        var bmi = CreateBmi(weightStones, weightPounds, heightFeet, heightInches);

        Assert.AreEqual(expectedCategory, bmi.BMICategory);
    }

    [TestMethod]
    [DataRow(5, 0, 4, 0)]
    [DataRow(50, 13, 7, 11)]
    public void Validation_AcceptsInclusiveBoundaryValues(
        int weightStones,
        int weightPounds,
        int heightFeet,
        int heightInches)
    {
        var bmi = CreateBmi(weightStones, weightPounds, heightFeet, heightInches);

        var results = Validate(bmi);

        Assert.HasCount(0, results);
    }

    [TestMethod]
    [DataRow(nameof(BMI.WeightStones), 4, "Stones must be between 5 and 50")]
    [DataRow(nameof(BMI.WeightStones), 51, "Stones must be between 5 and 50")]
    [DataRow(nameof(BMI.WeightPounds), -1, "Pounds must be between 0 and 13")]
    [DataRow(nameof(BMI.WeightPounds), 14, "Pounds must be between 0 and 13")]
    [DataRow(nameof(BMI.HeightFeet), 3, "Feet must be between 4 and 7")]
    [DataRow(nameof(BMI.HeightFeet), 8, "Feet must be between 4 and 7")]
    [DataRow(nameof(BMI.HeightInches), -1, "Inches must be between 0 and 11")]
    [DataRow(nameof(BMI.HeightInches), 12, "Inches must be between 0 and 11")]
    public void Validation_RejectsEveryOutOfRangeMeasurement(
        string propertyName,
        int invalidValue,
        string expectedMessage)
    {
        var bmi = CreateBmi(weightStones: 12, weightPounds: 0, heightFeet: 5, heightInches: 10);
        typeof(BMI).GetProperty(propertyName)!.SetValue(bmi, invalidValue);

        var results = Validate(bmi);

        Assert.HasCount(1, results);
        Assert.AreEqual(expectedMessage, results.Single().ErrorMessage);
        CollectionAssert.Contains(results.Single().MemberNames.ToArray(), propertyName);
    }

    private static BMI CreateBmi(int weightStones, int weightPounds, int heightFeet, int heightInches) =>
        new()
        {
            WeightStones = weightStones,
            WeightPounds = weightPounds,
            HeightFeet = heightFeet,
            HeightInches = heightInches
        };

    private static List<ValidationResult> Validate(BMI bmi)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(bmi, new ValidationContext(bmi), results, validateAllProperties: true);
        return results;
    }
}
