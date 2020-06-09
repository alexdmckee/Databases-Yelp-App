namespace Yelp_GUI.YelpEngine.Queries
{
    using System;
    using Npgsql;

    class UserInformationQueries
    {
        public static void GetUserIDs(Action<NpgsqlDataReader> myf, string username)
        {
            string sqlStr = "SELECT user_id " +
                            "FROM _User " +
                            "WHERE user_name = '" + username + "'";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetUser(Action<NpgsqlDataReader> myf, string userID)
        {
            string sqlStr = "SELECT user_id, user_name, join_date, numFans, avg_stars, cool, funny, useful, total_tip_count, total_tip_likes, longitude, latitude " +
                            "FROM _User " +
                            "WHERE user_id = '" + userID + "'";
            YelpDatabaseQueries.ExecuteQuerySingle(sqlStr, myf);
        }

        public static void GetUserFriends(Action<NpgsqlDataReader> myf, string userID)
        {
            string sqlStr = "SELECT user_name, total_tip_likes, avg_stars, join_date " +
                            " FROM(Select friend_id FROM friends WHERE '" + userID + "' = user_ID) as Temp, _User " +
                            " WHERE Temp.friend_id = _User.user_id";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void GetUserFriendsLatestTips(Action<NpgsqlDataReader> myf, string userID)
        {
            string sqlStr = "Select a.user_name, business_name, city, c.text, c.tipdate " +
                            " FROM (SELECT user_name, _User.user_id " +
                                  " FROM (Select friend_id " +
                                          " FROM friends " +
                                          "  WHERE '" + userID + "'   = user_ID) as Temp, _User " +
                                          " WHERE Temp.friend_id = _User.user_id) as a, (SELECT tip.user_id, tip.business_id, tip.text, tip.tipdate " +
                                                                                        " FROM tip, (SELECT tip.user_id, max(tipdate) as maxdate " +
                                                                                         " FROM tip " +
                                                                                         " GROUP BY user_id) as b " +
                                                                                         " WHERE tip.user_ID = b.user_ID AND tip.tipdate = b.maxdate) as c, business " + 
                           " WHERE a.user_id = c.user_id AND c.business_id = business.business_ID";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void UpdateUser(string user_id, double lat, double longg)
        {
            string sqlStr = "UPDATE _user " +
                           " SET latitude = :latitude, longitude = :longitude" +
                           " WHERE user_id = '" + user_id + "'";

            YelpDatabaseQueries.ExecuteUserLatLongUpdate(sqlStr, lat, longg);
        }
    }
}