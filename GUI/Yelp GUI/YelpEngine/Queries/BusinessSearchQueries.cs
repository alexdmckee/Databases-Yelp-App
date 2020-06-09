namespace Yelp_GUI.YelpEngine.Queries
{
    using System;
    using System.Collections.Generic;
    using Npgsql;
    using Yelp_GUI.YelpEngine.Objects;

    internal class BusinessSearchQueries
    {
        public static void GetDistinctStates(Action<NpgsqlDataReader> myf)
        {
            string sqlStr = "SELECT distinct state " +
                            "FROM business " +
                            "ORDER BY state";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetCitiesInSelectedState(Action<NpgsqlDataReader> myf, string selectedState)
        {
            string sqlStr = "SELECT distinct city " +
                            "FROM business " +
                            "WHERE state = '" + selectedState + "' " +
                            "ORDER BY city";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetZipcodesInSelectedCity(Action<NpgsqlDataReader> myf, string selectedState, string selectedCity)
        {
            string sqlStr = "SELECT distinct zipcode " +
                            "FROM business " +
                            "WHERE state = '" + selectedState + "' AND city = '" + selectedCity + "' " +
                            "ORDER BY zipcode";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetBusinessCategoriesInSelectedZipcode(Action<NpgsqlDataReader> myf, string selectedState, string selectedCity, string selectedZipcode)
        {
            string sqlStr = "SELECT distinct category_name " +
                            "FROM Business, Categories " +
                            "WHERE state = '" + selectedState + "' AND city = '" + selectedCity + "' AND zipcode = '" + selectedZipcode + "' AND Business.business_id = Categories.business_id ";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetBusinesses(Action<NpgsqlDataReader> myf, BusinessSearch businessSearch)
        {
            string sqlStr = "SELECT DISTINCT Business.business_id, Business.business_name, Business.address, Business.city, Business.state, Business.zipcode, Business.longitude, Business.latitude, Business.stars, Business.numCheckins, Business.numTips, ";

            if (businessSearch.CurrentUser != null)
            {
                sqlStr += "Distance(" + businessSearch.CurrentUser.Latitude + ", " + businessSearch.CurrentUser.Longitude + ", Business.latitude, Business.longitude) AS distance ";
            }
            else
            {
                sqlStr += "-1 as distance ";
            }

            sqlStr += "FROM Business";
            int temp = 0;
            foreach (string category in businessSearch.BusinessCategories)
            {
                sqlStr += " JOIN Categories AS Category" + temp + " ON Category" + temp++ + ".business_id = Business.business_id";
            }

            temp = 0;
            foreach (string attribute in businessSearch.BusinessAttributes.Keys)
            {
                sqlStr += " JOIN BusinessAttributes AS Attributes" + temp + " ON Attributes" + temp++ + ".business_id = Business.business_id";
            }

            sqlStr += " WHERE Business.state = '" + businessSearch.State + "' AND Business.city = '" + businessSearch.City + "' AND Business.zipcode = '" + businessSearch.Zipcode + "'";

            temp = 0;
            foreach (string category in businessSearch.BusinessCategories)
            {
                sqlStr += " AND Category" + temp++ + ".category_name = '" + category + "'";
            }

            temp = 0;
            foreach (string attribute in businessSearch.BusinessAttributes.Keys)
            {
                sqlStr += " AND Attributes" + temp + ".attribute_name = '" + attribute + "' AND Attributes" + temp++ + ".value = '";

                string attributeValue = string.Empty;
                switch (attribute)
                {
                    case "RestaurantsPriceRange2":
                        if (businessSearch.BusinessAttributes.TryGetValue(attribute, out attributeValue))
                        {
                            sqlStr += attributeValue;
                        }

                        break;
                    case "WiFi":
                        sqlStr += "free";
                        break;
                    default:
                        sqlStr += "True";
                        break;
                }

                sqlStr += "'";
            }

            sqlStr += " ORDER BY ";

            switch (businessSearch.SortingMethod)
            {
                case "Name (default)":
                    sqlStr += "Business.business_name ASC";
                    break;
                case "Highest Rating":
                    sqlStr += "Business.stars DESC";
                    break;
                case "Most Tips":
                    sqlStr += "Business.numTips DESC";
                    break;
                case "Most Check-ins":
                    sqlStr += "Business.numCheckins DESC ";
                    break;
                case "Nearest":
                    if (businessSearch.CurrentUser != null)
                    {
                        sqlStr += "distance ASC";
                    }
                    else
                    {
                        sqlStr += "Business.business_name ASC";
                    }

                    break;
            }

            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetBusinessHours(Action<NpgsqlDataReader> myf, string businessID, string dayOfTheWeek)
        {
            string sqlStr = "SELECT open, close " +
                            "FROM Hours " +
                            "WHERE day_of_the_week = '" + dayOfTheWeek + "' AND business_id = '" + businessID + "'";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetBusinessCategories(Action<NpgsqlDataReader> myf, string businessID)
        {
            string sqlStr = "SELECT category_name " +
                            "FROM Categories " +
                            "WHERE business_id = '" + businessID + "'";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetBusinessAttributes(Action<NpgsqlDataReader> myf, string businessID)
        {
            string sqlStr = "SELECT attribute_name, value " +
                            "FROM businessattributes " +
                            "WHERE business_id = '" + businessID + "'";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }
    }
}
