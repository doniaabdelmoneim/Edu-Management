﻿namespace projectX.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole> ();
        
    }
}
