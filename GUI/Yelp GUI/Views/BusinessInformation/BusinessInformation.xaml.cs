namespace Yelp_GUI.Views
{
    using System.Windows.Controls;
    using Npgsql;
    using Yelp_GUI.YelpEngine;
    using Yelp_GUI.YelpEngine.Objects;
    using Yelp_GUI.YelpEngine.Queries;

    /// <summary>
    /// Interaction logic for BusinessInformation.xaml
    /// </summary>
    public partial class BusinessInformation : Page
    {
        private BusinessInformationEngine businessInformationEngine = new BusinessInformationEngine();

        public BusinessInformation()
        {
            this.InitializeComponent();
        }

        private void AddBusinessIDs(NpgsqlDataReader reader)
        {
            this.ListOfBusiness.Items.Add(reader.GetString(0));
        }

        private void EnteredBusiness(object sender, TextChangedEventArgs e)
        {
            this.ListOfBusiness.Items.Clear();
            BusinessInformationQueries.GetBusinessIDs(this.AddBusinessIDs, this.BusinessTextBox.Text);
        }

        private void SelectedBusiness(object sender, SelectionChangedEventArgs e)
        {
            if (this.ListOfBusiness.SelectedIndex > -1)
            {
                this.AttributesListBox.Items.Clear();
                this.CategoriesListBox.Items.Clear();
                this.BusinessTips.Items.Clear();
                this.RecentCheckInTimes.Items.Clear();
                this.businessInformationEngine.ClearBusiness();
                BusinessInformationQueries.GetBusiness(this.PopulateBusinessInformation, this.ListOfBusiness.SelectedItem.ToString());

                // run query for recent check in times
                BusinessInformationQueries.GetRecentCheckinTimes(this.AddRecentCheckinTimes, this.ListOfBusiness.SelectedItem.ToString());

                // run query for business categories
                BusinessInformationQueries.GetBusinessCategories(this.AddBusinessCategory, this.ListOfBusiness.SelectedItem.ToString());

                // run query for business attributes
                BusinessInformationQueries.GetBusinessAttributes(this.AddBusinessAttribute, this.ListOfBusiness.SelectedItem.ToString());

                // run query for latest tips
                BusinessInformationQueries.GetBusinessLatestTips(this.AddBusinessLatestTip, this.ListOfBusiness.SelectedItem.ToString());
            }
        }

        private void PopulateBusinessInformation(NpgsqlDataReader reader)
        {
            this.businessInformationEngine.SetBusiness(new Business(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetInt32(5).ToString(), reader.GetDouble(6), reader.GetInt32(7), reader.GetInt32(8)));
            this.Name.Text = reader.GetString(1);
            this.Address.Text = reader.GetString(2);
            this.City.Text = reader.GetString(3);
            this.State.Text = reader.GetString(4);
            this.Zipcode.Text = reader.GetInt32(5).ToString();
            this.Stars.Text = reader.GetDouble(6).ToString();
            this.Checkins.Text = reader.GetInt32(7).ToString();
            this.TipCount.Text = reader.GetInt32(8).ToString();
        }

        private void AddRecentCheckinTimes(NpgsqlDataReader reader)
        {
            string newTime = reader.GetString(0) + "/" + reader.GetString(1) + "/" + reader.GetString(2) + " at " + reader.GetString(3);
            this.RecentCheckInTimes.Items.Add(newTime);
        }

        private void AddBusinessCategory(NpgsqlDataReader reader)
        {
            this.CategoriesListBox.Items.Add(reader.GetString(0));
        }

        private void AddBusinessAttribute(NpgsqlDataReader reader)
        {
            if (reader.GetString(0) == "RestaurantsPriceRange2")
            {
                string money = string.Empty;
                for (int i = 0; i < int.Parse(reader.GetString(1)); i++)
                {
                    money += "$";
                }

                this.Price.Text = money;
            }

            this.AttributesListBox.Items.Add(reader.GetString(0) + ": " + reader.GetString(1));
        }

        private void AddBusinessLatestTip(NpgsqlDataReader reader)
        {
            this.BusinessTips.Items.Add(new Tip()
            {
                Username = reader.GetString(0),
                TipDate = reader.GetTimeStamp(1).ToString(),
                NumberOfLikes = reader.GetInt32(2),
                Text = reader.GetString(3),
            });
        }
    }
}
