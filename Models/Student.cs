using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectX.Models
{
    public class Student
    {
        public int Id { get; set; }

        //constraints
        [Required]
        [StringLength (20, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 20 characters.")]
        public string Name { get; set; }

        [Range (20, 30, ErrorMessage = "Age must be between 20 and 30.")]
        public int Age { get; set; }

        [Required]
        [RegularExpression (@"[a-zA-Z0-9_]+@[a-zA-Z_]+.[a-zA-Z]{2,4}", ErrorMessage = "Invalid email.")]
        [Remote ("CheckEmailExistance", "Student", AdditionalFields = "Id", ErrorMessage = "Email already in use")]
        // remote takes to prameters (action name , controller name)
        public string Email { get; set; }

        [Required]
        [RegularExpression (@"[a-zA-Z0-9_]{8,}", ErrorMessage = "Password must be at least 8 characters long and contain only letters, numbers, and underscores.")]
        //datatype password will hide the password and help tag helper to hide it
        [DataType (DataType.Password)]
        public string password { get; set; }
        [Compare ("password", ErrorMessage = "Passwords do not match.")]
        [NotMapped] //not to be saved in database
        [DataType (DataType.Password)]
        public string Cpassword { get; set; }

        //navigation property
        [ForeignKey ("Department")]

        //?==> nullable (when i delete department and it have many student
        //it will not deleted and will set its deptId = null)

        public int? deptId { get; set; }
        public Department Department { get; set; }
    }
}
