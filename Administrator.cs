using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HospitalManagementSystem
{
    [Serializable]
    internal class Administrator : Employee
    {
        public Administrator(string name, string surname, string PESELNumber, string username, string password)
            : base(name, surname, PESELNumber, username, password)
        {

        }

       
    }
}
