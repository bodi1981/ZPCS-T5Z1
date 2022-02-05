using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comments { get; set; }
        public string Maths { get; set; }
        public string Technology { get; set; }
        public string Physics { get; set; }
        public string Polish { get; set; }
        public string Foreign { get; set; }
        public bool IsAddedClass { get; set; }
        public StudentClasses StudentClass { get; set; }
    }
}
