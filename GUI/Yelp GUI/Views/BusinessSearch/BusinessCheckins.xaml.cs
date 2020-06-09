namespace Yelp_GUI.Views
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using Npgsql;
    using Yelp_GUI.YelpEngine;
    using Yelp_GUI.YelpEngine.Objects;
    using Yelp_GUI.YelpEngine.Queries;

    /// <summary>
    /// Interaction logic for BusinessCheckins.xaml
    /// </summary>
    public partial class BusinessCheckins : Window, INotifyPropertyChanged
    {
        private Business business = null;
        private int[] checkins = new int[12];

        public event PropertyChangedEventHandler PropertyChanged;

        public BusinessCheckins(Business business)
        {
            this.business = business;
            this.InitializeComponent();
            this.GraphSetup();
        }

        public void AddMonthToGraph(NpgsqlDataReader reader)
        {
            string month = reader.GetString(0);
            int num = reader.GetInt32(1);
            this.checkins[int.Parse(reader.GetString(0)) - 1] += reader.GetInt32(1);
        }

        private void GraphSetup()
        {
            List<KeyValuePair<string, int>> graphData = new List<KeyValuePair<string, int>>();
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "Decemeber" };
            int[] checkins = new int[12];

            for (int i = 0; i < 12; i++)
            {
                this.checkins[i] = 0;
            }

            if (this.business != null)
            {
                BusinessCheckinsQueries.GetBusinessCheckin(this.AddMonthToGraph, this.business.BusinessID);

                for (int i = 0; i < 12; i++)
                {
                    graphData.Add(new KeyValuePair<string, int>(months[i], this.checkins[i]));
                }
            }
            else
            {
                this.CheckInButton.IsEnabled = false;
            }

            this.checkinChart.DataContext = graphData;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Checkin checkin = new Checkin(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.ToString("hh:mm:ss"), this.business.BusinessID);
            BusinessCheckinsQueries.InsertCheckin(checkin);
            this.PropertyChanged?.DynamicInvoke(null, new PropertyChangedEventArgs(string.Empty)); // notify the businessSearch
            this.checkinChart.DataContext = null;
            this.GraphSetup(); // refresh the graaph
        }
    }
}
