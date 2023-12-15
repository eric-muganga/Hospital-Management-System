using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem
{
    [Serializable]
    public class Hospital
    {
        private List<Employee> _employees;

        public List<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; }
        }
        public Hospital()
        {
            Employees = new List<Employee>()
            {
                new Administrator("Eric", "Muganga", "0234456750", "admin", "admin"),
                new Doctor("Derek", "Gisa", "303094499", "derek", "derek", "12304", "Cardiologist")

            };
        }

        
        public void Addemployee(Employee employee)
        {
            Employees.Add(employee);
        }

        public bool UserExists(string username)
        {
            foreach (Employee empl in Employees)
            {
                if (empl.Username == username) return true;
            }
            return false;
        }

    }
}
