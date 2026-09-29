using System.ComponentModel.DataAnnotations;

namespace BMICalculator;

public enum BMICategory
{
    Underweight,
    Normal,
    Overweight,
    Obese
}

public class BMI
{
    private const int PoundsPerStone = 14;
    private const int InchesPerFoot = 12;
    private const double PoundsToKilograms = 0.453592;
    private const double InchesToMetres = 0.0254;

    [Display(Name = "Weight (stone)")]
    [Required(ErrorMessage = "Enter your weight in stone")]
    [Range(5, 50, ErrorMessage = "Stone must be between 5 and 50")]
    public int? WeightStones { get; set; }

    [Display(Name = "Additional pounds")]
    [Required(ErrorMessage = "Enter 0 if there are no additional pounds")]
    [Range(0, 13, ErrorMessage = "Pounds must be between 0 and 13")]
    public int? WeightPounds { get; set; }

    [Display(Name = "Height (feet)")]
    [Required(ErrorMessage = "Enter your height in feet")]
    [Range(4, 7, ErrorMessage = "Feet must be between 4 and 7")]
    public int? HeightFeet { get; set; }

    [Display(Name = "Additional inches")]
    [Required(ErrorMessage = "Enter 0 if there are no additional inches")]
    [Range(0, 11, ErrorMessage = "Inches must be between 0 and 11")]
    public int? HeightInches { get; set; }

    [Display(Name = "Your BMI")]
    [DisplayFormat(DataFormatString = "{0:F1}")]
    public double BMIValue
    {
        get
        {
            var weightInPounds = WeightStones!.Value * PoundsPerStone + WeightPounds!.Value;
            var heightInInches = HeightFeet!.Value * InchesPerFoot + HeightInches!.Value;
            var weightInKilograms = weightInPounds * PoundsToKilograms;
            var heightInMetres = heightInInches * InchesToMetres;

            return weightInKilograms / Math.Pow(heightInMetres, 2);
        }
    }

    [Display(Name = "Category")]
    public BMICategory BMICategory
    {
        get
        {
            if (BMIValue < 18.5)
            {
                return BMICategory.Underweight;
            }

            if (BMIValue < 25)
            {
                return BMICategory.Normal;
            }

            if (BMIValue < 30)
            {
                return BMICategory.Overweight;
            }

            return BMICategory.Obese;
        }
    }
}
