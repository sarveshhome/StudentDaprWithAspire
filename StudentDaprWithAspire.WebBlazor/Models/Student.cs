using System.ComponentModel.DataAnnotations;

namespace StudentDaprWithAspire.WebBlazor.Models;

public class Student
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Age is required")]
    [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
    public int Age { get; set; }
}
