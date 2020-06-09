namespace Yelp_GUI.YelpEngine.Queries
{
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class BusinessInformationQueries
    {
        public static void GetBusinessIDs(Action<NpgsqlDataReader> myf, string businessName)
        {
            string sqlStr = "SELECT business_id " +
                            "FROM Business " +
                            "WHERE business_name = '" + businessName + "'";

            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetBusiness(Action<NpgsqlDataReader> myf, string businessID)
        {
            string sqlStr = "SELECT business_id, business_name, address, city, state, zipcode, stars, numCheckins, numTips " +
                            "FROM Business " +
                            "WHERE business_id = '" + businessID + "'";

            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetRecentCheckinTimes(Action<NpgsqlDataReader> myf, string businessID)
        {
            string sqlStr = "SELECT month, day, year, time " +
                            "FROM CheckIns " +
                            "WHERE business_id = '" + businessID + "' " +
                            "ORDER BY year DESC, month DESC, day DESC";

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
                            "FROM BusinessAttributes " +
                            "WHERE business_id = '" + businessID + "'";

            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetBusinessLatestTips(Action<NpgsqlDataReader> myf, string businessID)
        {
            string sqlStr = "SELECT u.user_name, t.tipDate, t.numLikes, t.text " +
                            "FROM _User as u, Business as b, Tip as t " +
                            "WHERE b.Business_id = '" + businessID + "'" + " AND b.business_id = t.business_id AND u.user_id = t.user_id " +
                            "ORDER BY t.tipDate DESC";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }
    }
}