using projectX.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectX.Repositories;
using projectX.CustomFilter;
using Microsoft.AspNetCore.Authorization;

namespace projectX.Controllers
{
    public class StudentController : Controller
    {
        //StudentRepo StudentRepo = new StudentRepo ();
        //DepartmentRepo DepartmentRepo = new DepartmentRepo ();
        IDepartmentRepo DepartmentRepo;  //=new DepartmentRepo ();   //dependancy
        IStudentRepo StudentRepo;  //=new StudentRepo ();   //dependancy
        public StudentController (IStudentRepo _StudentRepo, IDepartmentRepo _DepartmentRepo)  //counstractor injection
        {
            StudentRepo = _StudentRepo;
            DepartmentRepo = _DepartmentRepo;
        }

        //=======================> CRUD OPERATIONS <========================== 

        //=================      DISPLAY     =======================================\
         [Authorize]
        public IActionResult Index()
        {
            //list of students
            var students = StudentRepo.getAll ();
            return View (students);
        }


        //=================      CREATE     =======================================

        //=====> sudent/create
        [Authorize(Roles = "Admin")]
        public IActionResult Create ()
        {
            var depts = DepartmentRepo.getAll ();
            ViewBag.depts = depts;
            return View ();
        }

        [HttpPost] //action selector
        public IActionResult Create (Student student) //model binding
        {
            // to check if the model data is valid
            // ModelState ===> it is a dictionary that contains both the state of the model and a model's validation errors
            //ModelState.IsValid ===> it is a boolean property that returns true if the model is valid  against the validation of data annotations in model

            if (ModelState.IsValid)
            {
                //db.students.Add (student);
                //db.SaveChanges ();
                StudentRepo.Insert (student);
                return RedirectToAction ("Index");
            }
            //if the model is not valid , return the same view with the model data

            //to get the departments from the database
            //var depts = db.Departments.ToList ();
            var depts = DepartmentRepo.getAll ();
            ViewBag.depts = depts;
            return View (student);
            }

        //==> APPLY REMOTE  :: to check if the email is exist or not
        public JsonResult CheckEmailExistance (string email, int id)
        {
            // Find the student with this email (if any)
            //var studentWithEmail = db.students.FirstOrDefault (s => s.Email == email);
            var studentWithEmail = StudentRepo.studentWithEmail (email);

            // Valid if:
            // 1. No student has this email, OR
            // 2. The student with this email is the current student (id matches)
            if (studentWithEmail == null || studentWithEmail.Id == id)
            {
                return Json (true);
            }

            return Json ($"Email {email} is already in use by another student.");
        }


        //=================      DETAILS     =======================================

        //==> sudent/details/1
        [Authorize (Roles = "Admin,Instructor ,Student")]
        public IActionResult Details (int? id)
        {
            if (id == null)
            {
                return BadRequest ();
            }
            var student = StudentRepo.getById (id.Value);
            if (student == null)
            {
                return NotFound ();
            }
            return View (student);
        }

        //=================      UPDATE     =======================================

        //==> sudent/edit/1
        [Authorize (Roles = "Admin,Instructor")]


        public IActionResult Edit (int? id)
        {
            if (id == null)
            {
                return BadRequest ();
            }

            //var student = db.students.Include (s => s.Department).FirstOrDefault (s => s.Id == id);
            var student =StudentRepo.getById (id.Value);
            if (student == null)
            {
                return NotFound ();
            }
            //var depts = db.Departments.ToList ();
            //ViewBag.depts = depts;
            var depts = DepartmentRepo.getAll ();
            ViewBag.depts = depts;
            return View (student);
        }

        [HttpPost]
        [Authorize (Roles = "Admin")]

        public IActionResult Edit (Student student)
        {
            if (ModelState.IsValid)
            {
                // First check if email is being changed and if it's already used

                var existingStudent = StudentRepo.existingStudent(student.Id);
                var studentWithSameEmail =StudentRepo.studentWithSameEmail(student.Email, student.Id);

                if (studentWithSameEmail != null)
                {
                    ModelState.AddModelError ("Email", $"Email {student.Email} is already in use by another student.");
                    ViewBag.depts = DepartmentRepo.getAll ();
                    return View (student);
                }

                // Update the student if email is valid
                existingStudent.Name = student.Name;
                existingStudent.Age = student.Age;
                existingStudent.Email = student.Email;
                existingStudent.deptId = student.deptId;

                if (!string.IsNullOrEmpty (student.password))
                {
                    existingStudent.password = student.password;
                }
                StudentRepo.Update (existingStudent);
                return RedirectToAction ("Index");
            }

            ViewBag.depts = DepartmentRepo.getAll ();
            return View (student);
        }
        //=================      DELETE     =======================================

        //==> sudent/delete/1
        [Authorize (Roles = "Admin")]
        public IActionResult Delete (int? id)
        {
            if (id == null)
            {
                return BadRequest ();
            }
            var student = StudentRepo.getById (id.Value);
            if (student == null)
            {
                return NotFound ();
            }
            
            return View (student);
        }

        [HttpPost]
        public IActionResult Delete (Student student)
        {
            StudentRepo.DeleteById (student.Id);
            return RedirectToAction ("Index");
        }
    }
}
