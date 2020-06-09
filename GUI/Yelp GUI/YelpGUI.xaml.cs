namespace Yelp_GUI
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Yelp_GUI.Views;
    using Yelp_GUI.YelpEngine;
    using Yelp_GUI.YelpEngine.Queries;

    public partial class YelpGUI : Window
    {
        public YelpGUI()
        {
            this.InitializeComponent();
            UserInformation userInformation = new UserInformation();
            BusinessSearch businessSearch = new BusinessSearch();
            userInformation.UserInformationEngine.PropertyChanged += businessSearch.UserChanged;
            this.UserInformationFrame.Content = userInformation;
            this.BusinessInformationFrame.Content = new BusinessInformation();
            this.BusinessSearchFrame.Content = businessSearch;
        }
    }
}