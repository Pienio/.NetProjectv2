using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAccess;
using DatabaseAccess.Model;

namespace DataAccessService
{
    public class DoctorExtension: Doctor
    {

        public DoctorExtension(Doctor a):base()
        {
            this.User = a.User;
            this.FridayWorkingTime = a.FridayWorkingTime;
            MondayWorkingTime = a.MondayWorkingTime;
            TuesdayWorkingTime = a.TuesdayWorkingTime;
            WednesdayWorkingTime = a.WednesdayWorkingTime;
            ThursdayWorkingTime = a.ThursdayWorkingTime;
            this.Key = a.Key;
            this.Visits = a.Visits;
            this.Specialization = a.Specialization;
            this.Version = a.Version;

        }
        public DateTime GetFirstFreeSlot()
        {
            DateTime current = NextSlot(DateTime.Now.AddMinutes(60));
            //zmiana
            var visits = (from v in Visits
                          where v.Date >= current
                          select v.Date).ToList();
            visits.Sort((v1, v2) => DateTime.Compare(v1, v2));

            if (visits.Count == 0 || current < visits[0])
            {
                return current;
            }
            for (int i = 0; i < visits.Count - 1; i++)
            {
                current = NextSlot(visits[i].AddMinutes(30));
                if (current < visits[i + 1])
                    return current;
            }
            return NextSlot(visits[visits.Count - 1].AddMinutes(30));
        }

        private  DateTime NextSlot(DateTime date)
        {
            date = date.AddMinutes(30);
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute > 30 || date.Minute == 0 ? 30 : 0, 0);
            WorkingTime time;
            do
            {
                time = GetWorkingTime(date);
                if (time == null || date.Hour >= time.End)
                {
                    date = date.AddDays(1);
                    date = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                }
                else if (date.Hour < time.Start)
                {
                    return new DateTime(date.Year, date.Month, date.Day, time.Start, 0, 0);
                }
                else break;
            }
            while (true);
            return date;
        }

        /// <summary>
        /// Zwraca odpowiedni <see cref="WorkingTime"/> dla danego dnia tygodnia lub null w przypadku weekendu
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private WorkingTime GetWorkingTime(DateTime date)
        {
            DayOfWeek day = date.DayOfWeek;
            return (from p in typeof(Doctor).GetProperties()
                    where p.Name.Contains(day.ToString()) && p.PropertyType == typeof(WorkingTime)
                    select p.GetValue(this) as WorkingTime).FirstOrDefault();
        }
    }
}
