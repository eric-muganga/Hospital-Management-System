using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem
{
    [Serializable]
    abstract class MedicalStaff : Employee
    {
        private List<DateTime> _OnCallDuty;
        public List<DateTime> OnCallDuty
        {
            get
            {
                return _OnCallDuty;
            }
            set
            {
                _OnCallDuty = value;
            }
        }

        public MedicalStaff(string name, string surname, string PESELNumber, string username, string password)
            : base(name, surname, PESELNumber, username, password)
        {
            OnCallDuty = new List<DateTime>();

        }

        public override void Updated(string n, string s, string p, string u, string pa, Hospital h)
        {
            base.Updated(n, s, p, u, pa, h);
        }

        public void RemoveDuty(DateTime date)
        {
            OnCallDuty.Remove(date);
        }

        //public void AddDuty(DateTime date)
        //{
        //    if (CheckDutyInMonth(date))
        //    {
        //        if (DutyDayCheck(date) && !DutyExists(date))
        //            OnCallDuty.Add(date.Date);
        //    }
        //}



        //public bool DutyExists(DateTime date)
        //{
        //    date = date.Date;
        //    for (int i = 0; i < OnCallDuty.Count; i++)
        //    {
        //        if (date.Date == OnCallDuty[i].Date) return true;
        //    }
        //    return false;
        //}
        //private bool DutyDayCheck(DateTime date)
        //{
        //    for (int i = 0; i < OnCallDuty.Count; i++)
        //    {
        //        if (date.Date.AddDays(-1) == OnCallDuty[i].Date)
        //        {
        //            throw new Exception("Cant add duty after another duty");
        //        }
        //        else if (date.Date == OnCallDuty[i].Date)
        //        {
        //            throw new Exception("Cant add duty that already exists");
        //        }
        //        else if (date.Date.AddDays(1) == OnCallDuty[i].Date)
        //        {
        //            throw new Exception("Cant add duty thats before another duty");
        //        }
        //    }
        //    return true;
        //}
        //private bool CheckDutyInMonth(DateTime date)
        //{
        //    int month = date.Month;
        //    int year = date.Year;
        //    int count = 1;
        //    for (int i = 0; i < OnCallDuty.Count; i++)
        //    {
        //        if (OnCallDuty[i].Month == month && OnCallDuty[i].Year == year)
        //        {
        //            count++;
        //        }
        //    }
        //    //Check if the limit is exceeded
        //    if (count > 10) throw new Exception("Amount of duties for this month is above 10");
        //    return true;
        //}
    }
}
