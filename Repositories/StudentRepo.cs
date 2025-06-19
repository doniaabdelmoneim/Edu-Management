using Microsoft.EntityFrameworkCore;
using projectX.Models;

namespace projectX.Repositories
{
    public interface IStudentRepo
    {
        List<Student> getAll ();
        Student getById (int id);
        Student Insert (Student student);
        Student Update (Student student);
        Student DeleteById (int id);
        Student studentWithEmail (string email);
        Student existingStudent (int id);
        Student studentWithSameEmail (string email, int id);


    }
    public class StudentRepo : IStudentRepo
    {
        entitiesDbContext db;
        public StudentRepo (entitiesDbContext _db)
        {
            db = _db;
        }

        public Student DeleteById (int id)
        {
            Student student = db.students.FirstOrDefault (s => s.Id == id);
            db.students.Remove (student);
            db.SaveChanges ();
            return student;
        }

        public Student existingStudent (int id)
        {
            return db.students.Find (id);
        }

        public List<Student> getAll ()
        {
            return db.students.Include (s => s.Department).ToList ();
        }

        public Student getById (int id)
        {
            return db.students.Include (s => s.Department).FirstOrDefault (s => s.Id == id);    
        }

        public Student Insert (Student student)
        {
            db.students.Add (student);
            db.SaveChanges ();
            return student;

        }

        public Student studentWithEmail (string email)
        {
            return db.students.FirstOrDefault (s => s.Email == email); ;
        }

        public Student studentWithSameEmail (string email, int id)
        {
            return db.students.FirstOrDefault (s => s.Email == email && s.Id !=id);
        }

        public Student Update (Student student)
        {
            db.students.Update (student);
            db.SaveChanges ();
            return student;

        }
    }
}

