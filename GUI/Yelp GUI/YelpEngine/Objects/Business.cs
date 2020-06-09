namespace Yelp_GUI.YelpEngine
{
    public class Business
    {
        public Business()
        {

        }

        public Business(string businessID, string businessName, string address, string city, string state, string zipcode, double stars, int numCheckins, int numTips)
        {
            this.BusinessID = businessID;
            this.BusinessName = businessName;
            this.Address = address;
            this.City = city;
            this.State = state;
            this.Zipcode = zipcode;
            this.Stars = stars;
            this.NumCheckIns = numCheckins;
            this.NumTips = numTips;
        }

        public string BusinessID { get; set; }

        public string BusinessName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zipcode { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public double Stars { get; set; }

        public int NumCheckIns { get; set; }

        public int NumTips { get; set; }

        public string Distance { get; set; }
    }
}
