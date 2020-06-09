namespace Yelp_GUI.YelpEngine.Queries
{
    using System;
    using Npgsql;
    using Yelp_GUI.YelpEngine.Objects;

    internal class BusinessTipsQueries
    {
        public static void GetBusinessTips(Action<NpgsqlDataReader> myf, string businessID)
        {
            string sqlStr = "SELECT Tip.user_id, Tip.business_id, Tip.tipDate, Tip.text, Tip.numLikes, _User.user_name " +
                            "FROM Tip, _User " +
                            "WHERE Tip.business_id = '" + businessID + "' AND _User.user_id = Tip.user_id " +
                            "ORDER BY Tip.tipDate DESC";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }
        
        public static void GetFriendsTips(Action<NpgsqlDataReader> myf, string businessID, string userID)
        {
            string sqlStr = "SELECT Tip.tipDate, Tip.text, _User.user_name " +
                            "FROM Tip, _User " +
                            "WHERE Tip.business_id = '" + businessID + "' AND Tip.user_id IN (SELECT friend_id FROM Friends WHERE Friends.user_id = '" + userID + "') AND _User.user_id = Tip.user_id " +
                            "ORDER BY Tip.tipDate DESC";
            YelpDatabaseQueries.ExecuteQuery(sqlStr, myf);
        }

        public static void InsertTip(Tip tip)
        {
            string sqlStr = "INSERT INTO Tip(user_id, business_id, tipDate, text, numLikes) " +
                            "VALUES ('" + tip.UserID + "', '" + tip.BusinessID + "', current_timestamp, '" + tip.Text + "', " + tip.NumberOfLikes + ");";
            YelpDatabaseQueries.ExecuteNonQuery(sqlStr);
        }

        public static void LikeTip(Tip tip)
        {
            string sqlStr = "UPDATE tip " +
                            $"SET numlikes = 1 + (SELECT numLikes FROM tip WHERE tip.user_id = '{tip.UserID}' AND tip.business_id = '{tip.BusinessID}' AND tip.tipdate = '{tip.TipDate}') " +
                            $"WHERE tip.user_id = '{tip.UserID}' AND tip.business_id = '{tip.BusinessID}' AND tip.tipdate = '{tip.TipDate}'";
            YelpDatabaseQueries.ExecuteNonQuery(sqlStr);
        }
    }
}