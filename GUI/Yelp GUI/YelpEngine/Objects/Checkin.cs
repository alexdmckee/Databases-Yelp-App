namespace Yelp_GUI.YelpEngine.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Checkin
    {
        public Checkin(string year, string month, string day, string time, string business_id)
        {
            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Time = time;
            this.BusinessID = business_id;
        }

        public string Year { get; set; }

        public string Month { get; set; }

        public string Day { get; set; }

        public string Time { get; set; }

        public string BusinessID { get; set; }
    }
}
