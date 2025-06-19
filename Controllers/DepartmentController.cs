using projectX.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projectX.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace projectX.Controllers
{
    //dependent
    public class DepartmentController : Controller
    {

        //DepartmentRepo DepartmentRepo = new DepartmentRepo ();   //dependancy
        IDepartmentRepo DepartmentRepo;  //=new DepartmentRepo ();   //dependancy
        public DepartmentController (IDepartmentRepo _DepartmentRepo)  //counstractor injection
        {
            DepartmentRepo = _DepartmentRepo;
        }
        //=================      DISPLAY     =======================================
        [Authorize (Roles = "Admin,Instructor ,Student")]
        public IActionResult Index ()
        {
            var depts = DepartmentRepo.getAll();
            return View (depts);
        }
        //=================      CREATE     =======================================
        [Authorize (Roles = "Admin")]
        public IActionResult Create ()
        {
            return View ();
        }
        [HttpPost] //action selector
        public IActionResult Create (Department department)
        {
            if (ModelState.IsValid)
            {
                DepartmentRepo.Insert (department);
                return RedirectToAction ("Index");
            }
            return View (department); // Return the view with the department model to show validation errors
        }



        //=================      DETAILS   =======================================
        [Authorize (Roles = "Admin,Instructor ,Student")]
        public IActionResult Details (int? id)
        {
            if (id == null)
            {
                return BadRequest ();
            }

            //var department = db.Departments.Include (d => d.Students).FirstOrDefault (d => d.deptId == id);
            var department = DepartmentRepo.getWithStudent (id.Value);
            if (department == null)
            {
                return NotFound ();
            }
            return View (department);
        }

        //=================      EDIT     ==========================================
        [Authorize (Roles = "Admin,Instructor")]

        public IActionResult Edit (int? id)
        {
            if (id == null)
            {
                return BadRequest ();
            }
            var department = DepartmentRepo.getById(id.Value);
            //var department = db.Departments.FirstOrDefault (d => d.deptId == id);
            if (department == null)
            {
                return NotFound ();
            }
            return View (department);
        }
        [HttpPost]
        public IActionResult Edit (Department department)
        {
            DepartmentRepo.Update (department);
            //db.Entry (department).State = EntityState.Modified;
            //db.SaveChanges ();
            return RedirectToAction ("Index");
        }

        //=================      DELETE     =========================================
        [Authorize (Roles = "Admin")]
        public IActionResult Delete (int? id)
        {
            if (id == null)
            {
                return BadRequest ();
            }
            var department =  DepartmentRepo.getById (id.Value);
            //var department = db.Departments.FirstOrDefault (d => d.deptId == id);
            if (department == null)
            {
                return NotFound ();
            }
            return View (department);
        }
        [HttpPost]
        public IActionResult Delete (Department department)
        {
            //db.Entry (department).State = EntityState.Deleted;
            //db.SaveChanges ();
            DepartmentRepo.DeleteById (department.deptId);
            return RedirectToAction ("Index");
        }

    }
}
