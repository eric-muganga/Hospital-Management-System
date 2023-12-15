using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem
{
    [Serializable]
    internal class Doctor : MedicalStaff
    {
        public string PWZNumber { get; private set; }
        public string Specialist { get; private set; }
        public Doctor(string name, string surname, string PESELNumber, string username, string password, string pWZNumber, string speciality)
            : base(name, surname, PESELNumber, username, password)
        {
            this.PWZNumber = pWZNumber;
            //this.spe = spe;
            this.Specialist = speciality;
        }

        public void Updated(string n, string s, string p, string u,
            string pa, string pwz, string spec,  Hospital h)
        {
            base.Updated(n, s, p, u, pa, h);
            if (pwz == string.Empty) throw new ArgumentException("Name is empty");
            if (spec == string.Empty) throw new ArgumentException("Name is empty");
            
           this.PWZNumber = pwz;
            this.Specialist = spec;
        }
    }
}
