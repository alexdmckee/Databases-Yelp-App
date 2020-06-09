namespace Yelp_GUI
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using Npgsql;
    using Yelp_GUI.YelpEngine;
    using Yelp_GUI.YelpEngine.Objects;
    using Yelp_GUI.YelpEngine.Queries;

    public partial class BusinessTipsWindow : Window, INotifyPropertyChanged
    {
        private Business selectedBusiness = null;
        private User selectedUser = null;
        private Tip selectedTip = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public BusinessTipsWindow(Business business, User user)
        {
            this.InitializeComponent();

            if (business != null)
            {
                this.selectedBusiness = business;
                this.GetBusinessTips();
            }

            if (user != null && business != null)
            {
                this.selectedUser = user;
                this.GetFriendsTips();
            }
            else
            {
                this.LikeButton.IsEnabled = false;
                this.AddTipButton.IsEnabled = false;
            }
        }

        public void AddTips(NpgsqlDataReader reader)
        {
            this.SelectedBusinessTips.Items.Add(new Tip()
            {
                UserID = reader.GetString(0),
                BusinessID = reader.GetString(1),
                TipDate = reader.GetTimeStamp(2).ToString(),
                NumberOfLikes = reader.GetInt32(4),
                Text = reader.GetString(3),
                Username = reader.GetString(5),
            });
        }

        public void AddFriendTips(NpgsqlDataReader reader)
        {
            this.FriendsBusinessTips.Items.Add(new Tip()
            {
                TipDate = reader.GetTimeStamp(0).ToString(),
                Text = reader.GetString(1),
                Username = reader.GetString(2),
            });
        }

        private void GetBusinessTips()
        {
            this.SelectedBusinessTips.Items.Clear();
            BusinessTipsQueries.GetBusinessTips(this.AddTips, this.selectedBusiness.BusinessID);
        }

        private void GetFriendsTips()
        {
            this.FriendsBusinessTips.Items.Clear();
            BusinessTipsQueries.GetFriendsTips(this.AddFriendTips, this.selectedBusiness.BusinessID, this.selectedUser.UserID);
        }

        private void AddTip(object sender, RoutedEventArgs e)
        {
            if (this.selectedBusiness != null && this.selectedUser != null && this.EnteredTip.Text != string.Empty)
            {
                BusinessTipsQueries.InsertTip(new Tip(this.selectedUser.UserID, this.selectedBusiness.BusinessID, string.Empty, this.EnteredTip.Text, 0));
            }

            this.GetBusinessTips(); // refresh the tips
            this.PropertyChanged?.DynamicInvoke(null, new PropertyChangedEventArgs(string.Empty)); // notify the businessSearch to refresh
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.selectedTip != null)
            {
                BusinessTipsQueries.LikeTip(this.selectedTip);
                this.SelectedBusinessTips.Items.Clear();
                this.FriendsBusinessTips.Items.Clear();
                this.GetBusinessTips();
                this.GetFriendsTips();
            }
        }

        private void SelectedBusinessTips_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedBusinessTips.Items.Count != 0)
            {
                this.selectedTip = (Tip)this.SelectedBusinessTips.CurrentCell.Item;
            }
        }
    }
}
