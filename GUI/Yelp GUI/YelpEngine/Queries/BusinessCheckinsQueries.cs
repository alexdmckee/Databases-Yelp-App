namespace Yelp_GUI.YelpEngine.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Npgsql;
    using Yelp_GUI.YelpEngine.Objects;

    internal static class BusinessCheckinsQueries
    {
        public static void GetBusinessCheckin(Action<NpgsqlDataReader> myf, string businessID)
        {
            string sqlStr = "SELECT month, count(month) AS numCheckins " +
                            "FROM checkins " +
                            "WHERE business_id = '" + businessID + "' " +
                            "GROUP BY month " +
                            "ORDER BY month ASC";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void InsertCheckin(Checkin checkin)
        {
            string sqlStr = "INSERT INTO Checkins(year, month, day, time, business_id) " +
                            "VALUES ('" + checkin.Year + "', '" + checkin.Month + "', '" + checkin.Day + "', '" + checkin.Time + "', '" + checkin.BusinessID + "');";
            YelpDatabaseQueries.ExecuteNonQuery(sqlStr);
        }
    }
}
