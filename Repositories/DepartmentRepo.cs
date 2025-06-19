using Microsoft.EntityFrameworkCore;
using projectX.Models;

namespace projectX.Repositories
{
    public interface IDepartmentRepo
    {

            List<Department> getAll ();
            Department getById (int id);
            Department getWithStudent (int id);
            Department Insert (Department dept);
            Department Update (Department dept);

            Department DeleteById (int id);


    }
    public class DepartmentRepo: IDepartmentRepo
    {
        //injecting the DbContext using class name
        entitiesDbContext db;
        public DepartmentRepo (entitiesDbContext _db)
        {
            db = _db;
        }

        public Department DeleteById (int id)
        {
            Department dept = db.Departments.FirstOrDefault (d => d.deptId == id);
            //soft delete
            dept.status = false;
            //hard delete
            // db.Departments.Remove (dept);
            db.SaveChanges ();
            return dept;
        }

        public List<Department> getAll ()
        {
            return db.Departments.Where(d=>d.status==true).ToList ();
        }

        public Department getById (int id)
        {
            return db.Departments.FirstOrDefault(d=>d.deptId==id);
        }

        public Department getWithStudent (int id)
        {
            return db.Departments.Include (d => d.Students).FirstOrDefault (d => d.deptId == id);
        }

        public Department Insert (Department dept)
        {
            db.Departments.Add (dept);
            db.SaveChanges ();
            return dept;

        }

        public Department Update (Department dept)
        {
            db.Departments.Update (dept);
            db.SaveChanges ();
            return dept;

        }

       
    }

    }
