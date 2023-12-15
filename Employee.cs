using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HospitalManagementSystem
{
    [Serializable]
    public abstract class Employee
    {
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public string PESELNumber { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public Employee(string name, string surname, string PESELNumber, string username, string password)
        {
            this.Name = name;
            this.Surname = surname;
            this.PESELNumber = PESELNumber;
            this.Username = username;
            this.Password = password;
        }

        public virtual void Updated(string n, string s, string p, string u, string pa, Hospital h)
        {
            if(n == string.Empty) throw new ArgumentException("Name is empty");
            if (s == string.Empty) throw new ArgumentException("Name is empty");
            if (p == string.Empty) throw new ArgumentException("Name is empty");
            if (u == string.Empty) throw new ArgumentException("Name is empty");
            if (pa == string.Empty) throw new ArgumentException("Name is empty");

            if (u != this.Username && h.UserExists(u)) throw new ArgumentException("Username Already Exits");
            this.Name = n;
            this.Surname = s;
            this.PESELNumber = p;
            this.Username = u;
            this.Password = pa;
        }
    }
}
