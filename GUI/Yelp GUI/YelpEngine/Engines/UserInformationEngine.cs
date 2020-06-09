namespace Yelp_GUI.YelpEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserInformationEngine : INotifyPropertyChanged
    {
        private User currentUser = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetUser(User user)
        {
            this.currentUser = user;
            this.PropertyChanged?.DynamicInvoke(user, new PropertyChangedEventArgs(string.Empty));
        }

        public User GetUser()
        {
            return this.currentUser;
        }

        public void ClearUser()
        {
            this.currentUser = null;
        }

        public void setLatitude(double lat)
        {
            this.currentUser.Latitude = lat;
        }

        public void setLongitude(double longg)
        {
            this.currentUser.Longitude = longg;
        }
    }
}
