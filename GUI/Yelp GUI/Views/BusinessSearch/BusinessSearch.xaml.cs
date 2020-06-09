namespace Yelp_GUI.Views
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.DataVisualization.Charting;
    using System.Windows.Data;
    using Npgsql;
    using Yelp_GUI.YelpEngine;
    using Yelp_GUI.YelpEngine.Queries;

    /// <summary>
    /// Interaction logic for BusinessSearch.xaml
    /// </summary>
    public partial class BusinessSearch : Page
    {
        private BusinessMap businessMapWindow = null;
        private BusinessCheckins businessCheckinsWindow = null;
        private BusinessTipsWindow businessTipsWindow = null;
        public BusinessSearchEngine businessSearchEngine = null;

        public BusinessSearch()
        {
            this.InitializeComponent();
            this.DistinctStatesSetup();
            this.SortResultsBySetup();
            this.businessSearchEngine = new BusinessSearchEngine();
        }

        public void UserChanged(object sender, PropertyChangedEventArgs e)
        {
            this.businessSearchEngine.businessSearch.CurrentUser = (User)sender;
        }

        private void SortResultsBySetup()
        {
            this.SortResultsByComboBox.Items.Add("Name (default)");
            this.SortResultsByComboBox.Items.Add("Highest Rating");
            this.SortResultsByComboBox.Items.Add("Most Tips");
            this.SortResultsByComboBox.Items.Add("Most Check-ins");
            this.SortResultsByComboBox.Items.Add("Nearest");
        }

        private void DistinctStatesSetup()
        {
            BusinessSearchQueries.GetDistinctStates(this.AddStates);
        }

        private void AddStates(NpgsqlDataReader reader)
        {
            this.StateList.Items.Add(reader.GetString(0));
        }

        private void AddCity(NpgsqlDataReader reader)
        {
            this.CityList.Items.Add(reader.GetString(0));
        }

        private void AddZipcode(NpgsqlDataReader reader)
        {
            this.ZipcodeList.Items.Add(reader.GetInt32(0));
        }

        private void AddBusinessCategory(NpgsqlDataReader reader)
        {
            this.BusinessCategoriesList.Items.Add(reader.GetString(0));
        }

        private void AddSelectedBusinessCategory(NpgsqlDataReader reader)
        {
            this.SelectedBusinessCategoriesListBox.Items.Add(reader.GetString(0));
        }

        private void AddSelectedBusinessAttribute(NpgsqlDataReader reader)
        {
            this.SelectedBusinessAttributesListBox.Items.Add(reader.GetString(0) + ": " + reader.GetString(1));
        }

        private void AddSearchResult(NpgsqlDataReader reader)
        {
            Business newBusiness = new Business()
            {
                BusinessID = reader.GetString(0),
                BusinessName = reader.GetString(1),
                Address = reader.GetString(2),
                City = reader.GetString(3),
                State = reader.GetString(4),
                Zipcode = reader.GetInt32(5).ToString(),
                Longitude = reader.GetDouble(6),
                Latitude = reader.GetDouble(7),
                Stars = reader.GetDouble(8),
                NumCheckIns = reader.GetInt32(9),
                NumTips = reader.GetInt32(10),
            };

            if (reader.GetDouble(11) == -1)
            {
                newBusiness.Distance = "N/A";
            }
            else
            {
                newBusiness.Distance = reader.GetDouble(11).ToString();
            }

            this.SearchResults.Items.Add(newBusiness);
            this.businessSearchEngine.AddSearchResult(newBusiness);
        }

        private void ShowBusinessHours(NpgsqlDataReader reader)
        {
            string openTime = this.TimeFormatter(reader.GetString(0));
            string closeTime = this.TimeFormatter(reader.GetString(1));
            this.BusinessHoursTextBox.Content = "Today (" + DateTime.Today.DayOfWeek.ToString() + "): Opens at " + openTime + " and Closes at " + closeTime;
        }

        private string TimeFormatter(string time)
        {
            if (time.Length != 5)
            {
                if (time.Split(':')[0].Length < 2)
                {
                    time = "0" + time;
                }

                if (time.Split(':')[1].Length < 2)
                {
                    time = time + "0";
                }
            }

            return time;
        }

        private void ClearSelectedBusiness()
        {
            this.BusinessCategoriesList.Items.Clear();
            this.SelectedBusinessCategoriesList.Items.Clear();
            this.SearchResults.Items.Clear();
            this.ClearSelectedBusinessInfo();
            this.businessSearchEngine.businessSearch.SelectedBusiness = null;
            this.businessSearchEngine.businessSearch.SearchResults.Clear();
        }

        private void SelectedStateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the Lists
            this.CityList.Items.Clear();
            this.ZipcodeList.Items.Clear();
            this.ClearSelectedBusiness();
            this.businessSearchEngine.ClearBusinessLocation();

            if (this.StateList.SelectedIndex > -1)
            {
                this.businessSearchEngine.SetBusinessLocation(this.StateList.SelectedItem.ToString(), string.Empty, string.Empty);
                BusinessSearchQueries.GetCitiesInSelectedState(this.AddCity, this.StateList.SelectedItem.ToString());
            }
        }

        private void SelectedCityChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the Lists
            this.ZipcodeList.Items.Clear();
            this.ClearSelectedBusiness();
            this.businessSearchEngine.ClearBusinessLocation();

            if (this.CityList.SelectedIndex > -1)
            {
                this.businessSearchEngine.SetBusinessLocation(this.StateList.SelectedItem.ToString(), this.CityList.SelectedItem.ToString(), string.Empty);
                BusinessSearchQueries.GetZipcodesInSelectedCity(this.AddZipcode, this.StateList.SelectedItem.ToString(), this.CityList.SelectedItem.ToString());
            }
        }

        private void SelectedZipcodeChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the Lists
            this.ClearSelectedBusiness();
            this.businessSearchEngine.ClearBusinessLocation();

            if (this.ZipcodeList.SelectedIndex > -1)
            {
                this.businessSearchEngine.SetBusinessLocation(this.StateList.SelectedItem.ToString(), this.CityList.SelectedItem.ToString(), this.ZipcodeList.SelectedItem.ToString());
                BusinessSearchQueries.GetBusinessCategoriesInSelectedZipcode(this.AddBusinessCategory, this.StateList.SelectedItem.ToString(), this.CityList.SelectedItem.ToString(), this.ZipcodeList.SelectedItem.ToString());
            }
        }

        private void AddBusinessCategory(object sender, RoutedEventArgs e)
        {
            string selectedCategory = (string)this.BusinessCategoriesList.SelectedItem;
            if (selectedCategory != null)
            {
                if (!this.businessSearchEngine.ContainsBusinessCategory(selectedCategory))
                {
                    this.SelectedBusinessCategoriesList.Items.Add(selectedCategory);
                }

                this.businessSearchEngine.AddSelectedBusinessCategory(selectedCategory);
            }
        }

        private void RemoveBusinessCategory(object sender, RoutedEventArgs e)
        {
            if (this.SelectedBusinessCategoriesList.SelectedItem == null)
            {
                return;
            }

            string selectedCategory = this.SelectedBusinessCategoriesList.SelectedItem.ToString();

            if (selectedCategory != null)
            {
                this.businessSearchEngine.RemoveSelectedBusinessCategory(selectedCategory);
                this.SelectedBusinessCategoriesList.Items.Remove(selectedCategory);
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            this.RefreshSearch();
        }

        private void BusinessRowSelected(object sender, SelectionChangedEventArgs e)
        {
            if (this.SearchResults.Items.Count != 0)
            {
                this.ClearSelectedBusinessInfo();
                this.businessSearchEngine.AddSelectedBusinss((Business)this.SearchResults.CurrentCell.Item);
                this.BusinessNameTextBox.Content = this.businessSearchEngine.GetSelectedBusiness().BusinessName;
                this.BusinessAddressTextBox.Content = this.businessSearchEngine.GetSelectedBusiness().Address;
                BusinessSearchQueries.GetBusinessHours(this.ShowBusinessHours, this.businessSearchEngine.GetSelectedBusiness().BusinessID, DateTime.Today.DayOfWeek.ToString());
                BusinessSearchQueries.GetBusinessCategories(this.AddSelectedBusinessCategory, this.businessSearchEngine.GetSelectedBusiness().BusinessID);
                BusinessSearchQueries.GetBusinessAttributes(this.AddSelectedBusinessAttribute, this.businessSearchEngine.GetSelectedBusiness().BusinessID);
            }
        }

        private void ClearSelectedBusinessInfo()
        {
            this.BusinessNameTextBox.Content = "Business Name";
            this.BusinessAddressTextBox.Content = "Address";
            this.BusinessHoursTextBox.Content = "Today: Opens / Closes";
            this.SelectedBusinessCategoriesListBox.Items.Clear();
            this.SelectedBusinessAttributesListBox.Items.Clear();
        }

        private void MapButtonClicked(object sender, RoutedEventArgs e)
        {
            this.businessMapWindow = new BusinessMap(this.businessSearchEngine.GetSearchResults(), this.businessSearchEngine.GetCurrentUser());
            this.businessMapWindow.Show();
        }

        private void ShowTipsClicked(object sender, RoutedEventArgs e)
        {
            // TO DO
            this.businessTipsWindow = new BusinessTipsWindow(this.businessSearchEngine.GetSelectedBusiness(), this.businessSearchEngine.GetCurrentUser());
            this.businessTipsWindow.Show();
            this.businessTipsWindow.PropertyChanged += this.UpdateSearch;
            //this.ShowCheckinsButton.IsEnabled = false; // prevent multiple views from being opened.
        }

        private void ShowCheckinsClicked(object sender, RoutedEventArgs e)
        {
            this.businessCheckinsWindow = new BusinessCheckins(this.businessSearchEngine.GetSelectedBusiness());
            this.businessCheckinsWindow.Show();
            this.businessCheckinsWindow.PropertyChanged += this.UpdateSearch;
        }

        private void SelectedSortingMethodChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SortResultsByComboBox.SelectedIndex > -1)
            {
                this.businessSearchEngine.SetSortingMethod(this.SortResultsByComboBox.SelectedItem.ToString());
                if (this.businessSearchEngine.businessSearch.City != null && this.businessSearchEngine.businessSearch.State != null && this.businessSearchEngine.businessSearch.Zipcode != null)
                {
                    this.RefreshSearch();
                }
            }
        }

        private void RefreshSearch()
        {
            if (!string.IsNullOrEmpty(this.businessSearchEngine.businessSearch.State) && !string.IsNullOrEmpty(this.businessSearchEngine.businessSearch.City) && !string.IsNullOrEmpty(this.businessSearchEngine.businessSearch.Zipcode))
            {
                this.SearchResults.Items.Clear();
                this.businessSearchEngine.ClearSearchResults();
                BusinessSearchQueries.GetBusinesses(this.AddSearchResult, this.businessSearchEngine.GetBusinessSearch());
            }
        }

        private void UpdateSearch(object sender, PropertyChangedEventArgs e)
        {
            this.RefreshSearch();
        }

        private void SingleDollarChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsPriceRange2", "1");
            this.DoubleDollar.IsEnabled = false;
            this.TripleDollar.IsEnabled = false;
            this.QuadrupleDollar.IsEnabled = false;
            this.RefreshSearch();
        }

        private void SingleDollarUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsPriceRange2");
            this.DoubleDollar.IsEnabled = true;
            this.TripleDollar.IsEnabled = true;
            this.QuadrupleDollar.IsEnabled = true;
            this.RefreshSearch();
        }

        private void DoubleDollarChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsPriceRange2", "2");
            this.SingleDollar.IsEnabled = false;
            this.TripleDollar.IsEnabled = false;
            this.QuadrupleDollar.IsEnabled = false;
            this.RefreshSearch();
        }

        private void DoubleDollarUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsPriceRange2");
            this.SingleDollar.IsEnabled = true;
            this.TripleDollar.IsEnabled = true;
            this.QuadrupleDollar.IsEnabled = true;
            this.RefreshSearch();
        }

        private void TripleDollarChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsPriceRange2", "3");
            this.SingleDollar.IsEnabled = false;
            this.DoubleDollar.IsEnabled = false;
            this.QuadrupleDollar.IsEnabled = false;
            this.RefreshSearch();
        }

        private void TripleDollarUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsPriceRange2");
            this.SingleDollar.IsEnabled = true;
            this.DoubleDollar.IsEnabled = true;
            this.QuadrupleDollar.IsEnabled = true;
            this.RefreshSearch();
        }

        private void QuadrupleDollarChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsPriceRange2", "4");
            this.SingleDollar.IsEnabled = false;
            this.DoubleDollar.IsEnabled = false;
            this.TripleDollar.IsEnabled = false;
            this.RefreshSearch();
        }

        private void QuadrupleDollarUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsPriceRange2");
            this.SingleDollar.IsEnabled = true;
            this.DoubleDollar.IsEnabled = true;
            this.TripleDollar.IsEnabled = true;
            this.RefreshSearch();
        }

        private void AcceptsCreditCardChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("BusinessAcceptsCreditCards", "True");
            this.RefreshSearch();
        }

        private void AcceptsCreditCardsUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("BusinessAcceptsCreditCards");
            this.RefreshSearch();
        }

        private void TakesReservationsChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsReservations", "True");
            this.RefreshSearch();
        }

        private void TakeReservsationsUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsReservations");
            this.RefreshSearch();
        }

        private void WheelchairAccessibleChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("WheelchairAccessible", "True");
            this.RefreshSearch();
        }

        private void WheelchairAccessibleUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("WheelchairAccessible");
            this.RefreshSearch();
        }

        private void OutdoorSeatingChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("OutdoorSeating", "True");
            this.RefreshSearch();
        }

        private void OutdoorSeatingUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("OutdoorSeating");
            this.RefreshSearch();
        }

        private void GoodForKidsChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("GoodForKids", "True");
            this.RefreshSearch();
        }

        private void GoodForKidsUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("GoodForKids");
            this.RefreshSearch();
        }

        private void GoodForGroupsChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsGoodForGroups", "True");
            this.RefreshSearch();
        }

        private void GoodForGroupsUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsGoodForGroups");
            this.RefreshSearch();
        }

        private void DeliveryChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsDelivery", "True");
            this.RefreshSearch();
        }

        private void DeliveryUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsDelivery");
            this.RefreshSearch();
        }

        private void TakeOutChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("RestaurantsTakeOut", "True");
            this.RefreshSearch();
        }

        private void TakeOutUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("RestaurantsTakeOut");
            this.RefreshSearch();
        }

        private void FreeWiFiChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("WiFi", "free");
            this.RefreshSearch();
        }

        private void FreeWifiUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("WiFi");
            this.RefreshSearch();
        }

        private void BikeParkingChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("BikeParking", "True");
            this.RefreshSearch();
        }

        private void BikeParkingUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("BikeParking");
            this.RefreshSearch();
        }

        private void BreakfastChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("breakfast", "True");
            this.RefreshSearch();
        }

        private void BreakfastUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("breakfast");
            this.RefreshSearch();
        }

        private void LunchChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("lunch", "True");
            this.RefreshSearch();
        }

        private void LunchUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("lunch");
            this.RefreshSearch();
        }

        private void BrunchChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("brunch", "True");
            this.RefreshSearch();
        }

        private void BrunchUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("brunch");
            this.RefreshSearch();
        }

        private void DinnerChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("dinner", "True");
            this.RefreshSearch();
        }

        private void DinnerUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("dinner");
            this.RefreshSearch();
        }

        private void DessertChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("dessert", "True");
            this.RefreshSearch();
        }

        private void DessertUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("dessert");
            this.RefreshSearch();
        }

        private void LateNightChecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.AddAttributeToSortBy("latenight", "True");
            this.RefreshSearch();
        }

        private void LateNightUnchecked(object sender, RoutedEventArgs e)
        {
            this.businessSearchEngine.RemoveAttributeToSortBy("latenight");
            this.RefreshSearch();
        }
    }
}