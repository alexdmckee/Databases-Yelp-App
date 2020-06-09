namespace Yelp_GUI.Views
{
    using System.Collections.Generic;
    using System.Windows;
    using Microsoft.Maps.MapControl.WPF;
    using Yelp_GUI.YelpEngine;

    /// <summary>
    /// Interaction logic for BusinessMap.xaml
    /// </summary>
    public partial class BusinessMap : Window
    {
        public BusinessMap(List<Business> businesses, User user)
        {
            this.InitializeComponent();
            this.PutBusinessesOnMap(businesses);
            this.PutUserOnMap(user);

            if (user != null)
            {
                this.GlobalMap.Center = new Location(user.Latitude, user.Longitude);
            }
            else
            {
                this.GlobalMap.Center = new Location(0.0, 0.0);
            }
        }

        private void PutBusinessesOnMap(List<Business> businesses)
        {
            foreach (Business business in businesses)
            {
                Pushpin pin = new Pushpin();
                pin.Location = new Location(business.Latitude, business.Longitude);
                this.GlobalMap.Children.Add(pin);
            }
        }

        private void PutUserOnMap(User user)
        {
            if (user != null)
            {
                Pushpin pin = new Pushpin();
                Location userLocation = new Location(user.Latitude, user.Longitude);
                pin.Location = userLocation;
                this.GlobalMap.Children.Add(pin);
                this.GlobalMap.SetView(userLocation, 0);
            }
        }
    }
}
