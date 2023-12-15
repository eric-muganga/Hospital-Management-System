using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem
{
    [Serializable]
    internal class Nurse : MedicalStaff
    {
        public Nurse(string name, string surname, string PESELNumber, string username, string password)
            : base(name, surname, PESELNumber, username, password)
        {
            //OnCallDuty = new List<DateTime>();
        }

       
    }
}
