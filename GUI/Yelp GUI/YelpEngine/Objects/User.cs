namespace Yelp_GUI.YelpEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class User
    {
        public User(string id)
        {
            this.UserID = id;
        }

        public User(string userID, string name, string yelpingSince, int fans, double star, int cool, int funny, int useful, int total_tip_count, int total_tip_likes, double longitude, double latitude)
        {
            this.UserID = userID;
            this.Name = name;
            this.YelpingSince = yelpingSince;
            this.Fans = fans;
            this.Star = star;
            this.CoolVotes = cool;
            this.FunnyVotes = funny;
            this.UsefulVotes = useful;
            this.TipCount = total_tip_count;
            this.TotalTipLikes = total_tip_likes;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        public string UserID { get; set; }

        public string Name { get; set; }

        public double Star { get; set; }

        public int Fans { get; set; }

        public string YelpingSince { get; set; }

        public int FunnyVotes { get; set; }

        public int CoolVotes { get; set; }

        public int UsefulVotes { get; set; }

        public int TipCount { get; set; }

        public int TotalTipLikes { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}
