namespace Yelp_GUI.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Npgsql;
    using Yelp_GUI.YelpEngine;
    using Yelp_GUI.YelpEngine.Objects;
    using Yelp_GUI.YelpEngine.Queries;

    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class UserInformation : Page
    {
        public UserInformationEngine UserInformationEngine = new UserInformationEngine();

        public UserInformation()
        {
            this.InitializeComponent();
            this.UpdateUser.IsEnabled = false;
            this.EditUser.IsEnabled = false;
        }

        private void AddUserIDs(NpgsqlDataReader reader)
        {
            this.ListOfUsers.Items.Add(reader.GetString(0));
        }

        private void AddFriends(NpgsqlDataReader reader)
        {
            this.Friends.Items.Add(new Friend()
            {
                Name = reader.GetString(0),
                TotalTipLikes = reader.GetInt32(1),
                AvgStars = reader.GetDouble(2),
                YelpingSince = reader.GetDate(3).ToString(),
            });

        }

        private void AddFriendsTips(NpgsqlDataReader reader)
        {

            this.FriendsTips.Items.Add(new FriendsTips()
            {
                Name = reader.GetString(0),
                Business = reader.GetString(1),
                City = reader.GetString(2),
                Text = reader.GetString(3),
                TipDate = reader.GetTimeStamp(4).ToString(),
            });
        }

        private void PopulateUserInformation(NpgsqlDataReader reader)
        {
            this.UserInformationEngine.SetUser(new User(reader.GetString(0), reader.GetString(1), reader.GetDate(2).ToString(), reader.GetInt32(3), reader.GetDouble(4), reader.GetInt32(5), reader.GetInt32(6), reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetDouble(10), reader.GetDouble(11)));
            this.Name.Text = reader.GetString(1);
            this.Stars.Text = reader.GetDouble(4).ToString();
            this.Fans.Text = reader.GetInt32(3).ToString();
            this.YelpingSince.Text = reader.GetDate(2).ToString();
            this.CoolVotes.Text = reader.GetInt32(5).ToString();
            this.FunnyVotes.Text = reader.GetInt32(6).ToString();
            this.UsefulVotes.Text = reader.GetInt32(7).ToString();
            this.TipCount.Text = reader.GetInt32(8).ToString();
            this.TotalTipLikes.Text = reader.GetInt32(9).ToString();
            this.Longitude.Text = reader.GetDouble(10).ToString();
            this.Latitude.Text = reader.GetDouble(11).ToString();
        }

        private void ClearUserInfo()
        {
            this.UserInformationEngine.ClearUser();
            this.Name.Text = string.Empty;
            this.Stars.Text = string.Empty;
            this.Fans.Text = string.Empty;
            this.YelpingSince.Text = string.Empty;
            this.CoolVotes.Text = string.Empty;
            this.FunnyVotes.Text = string.Empty;
            this.UsefulVotes.Text = string.Empty;
            this.TipCount.Text = string.Empty;
            this.TotalTipLikes.Text = string.Empty;
            this.Longitude.Text = string.Empty;
            this.Latitude.Text = string.Empty;
            this.Friends.Items.Clear();
            this.FriendsTips.Items.Clear();
        }

        private void EnteredUsername(object sender, TextChangedEventArgs e)
        {
            this.ListOfUsers.Items.Clear();
            UserInformationQueries.GetUserIDs(this.AddUserIDs, this.UsernameTextBox.Text);
        }

        private void SelectedUser(object sender, SelectionChangedEventArgs e)
        {
            if (this.ListOfUsers.SelectedIndex > -1)
            {
                this.ClearUserInfo();
                this.EditUser.IsEnabled = true;
                UserInformationQueries.GetUser(this.PopulateUserInformation, this.ListOfUsers.SelectedItem.ToString());
                UserInformationQueries.GetUserFriends(this.AddFriends, this.ListOfUsers.SelectedItem.ToString());
                UserInformationQueries.GetUserFriendsLatestTips(this.AddFriendsTips, this.ListOfUsers.SelectedItem.ToString());
            }
        }

        private void Edit_Clicked(object sender, RoutedEventArgs e)
        {
            this.Latitude.IsEnabled = true;
            this.Longitude.IsEnabled = true;
            this.EditUser.IsEnabled = false;
            this.UpdateUser.IsEnabled = true;
        }

        private void Update_Clicked(object sender, RoutedEventArgs e)
        {
            this.Latitude.IsEnabled = false;
            this.Longitude.IsEnabled = false;
            this.UpdateUser.IsEnabled = false;
            this.EditUser.IsEnabled = true;

            // update the users lat long in the database and the usersearchengine currentuser object.
            double lat = 0.0;
            double.TryParse(this.Latitude.Text, out lat);
            this.UserInformationEngine.setLatitude(lat);
            double longg = 0.0;
            double.TryParse(this.Longitude.Text, out longg);
            this.UserInformationEngine.setLongitude(longg);
            UserInformationQueries.UpdateUser(this.ListOfUsers.SelectedItem.ToString(), lat, longg);
        }
    }
}
