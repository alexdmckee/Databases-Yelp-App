namespace Yelp_GUI.YelpEngine.Objects
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class BusinessSearch
    {
        public List<Business> SearchResults = new List<Business>();
        public Dictionary<string, string> BusinessAttributes = new Dictionary<string, string>();
        public List<string> BusinessCategories = new List<string>();

        public BusinessSearch()
        {
            this.SortingMethod = "Name (default)";
        }

        public User CurrentUser { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string Zipcode { get; set; }

        public string SortingMethod { get; set; }

        public Business SelectedBusiness { get; set; }
    }
}
