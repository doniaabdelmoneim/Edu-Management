using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projectX.Models
{
    public class Department
    {
        //data annotation
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)] // deptId is primary key and  auto generated
        public int deptId { get; set; }
        [Required]
        [StringLength (20, MinimumLength = 2)]
        public string DeptName { get; set; }
        [Range (20, 50)]
        [Required]
        public int capacity { get; set; }
        // for soft DELETE
        public bool status { get; set; } = true;

        public virtual List<Student> Students { get; set; }  = new List<Student> ();

    }
}
