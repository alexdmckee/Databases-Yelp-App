namespace Yelp_GUI.YelpEngine.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class Tip
    {
        public Tip()
        {

        }

        public Tip(string userID, string businessID, string tipDate, string text, int numberOfLikes)
        {
            this.UserID = userID;
            this.BusinessID = businessID;
            this.TipDate = tipDate;
            this.Text = text;
            this.NumberOfLikes = numberOfLikes;
        }

        public string UserID { get; set; }

        public string BusinessID { get; set; }

        public string TipDate { get; set; }

        public string Text { get; set; }

        public int NumberOfLikes { get; set; }

        public string Username { get; set; }
    }
}
