using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BMICalculator.Pages;

public class BmiModel : PageModel
{
    [BindProperty]
    public BMI BMI { get; set; } = new();

    public bool HasResult { get; private set; }

    public void OnPost()
    {
        HasResult = ModelState.IsValid;
    }
}
