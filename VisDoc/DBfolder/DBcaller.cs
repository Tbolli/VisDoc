using System;
using System.Data.SqlClient;

namespace VisDoc
{
    public class DBcaller
    {
        // 
        static string conString = @"Server=.\SQLEXPRESS;Database=VisDoc;User Id=VisDocNew;Password=VisDocNew;";
        SqlConnection con = new SqlConnection(conString);

        //constructor, initalize connection, initalize instacne variable con to a SqlConnectino type
        public DBcaller()
        {
            con.Open();
        }
        //return SqlDataReader object
        public SqlDataReader Query(string command)
        {
            SqlCommand evry = new SqlCommand(command, con);
            SqlDataReader rdr = evry.ExecuteReader();
            return rdr;
        }

        //~DBcaller()
        //{
        //    con.Close();    
        //}


        //public Dictionary<int, string[]> Query(string command )
        //{
        //    SqlCommand evry = new SqlCommand("SELECT * FROM Document", con);
        //    SqlDataReader rdr = evry.ExecuteReader();
        //    var DataObj = new Dictionary<int, string[]>();
        //    
        //    while (rdr.Read())
        //    {
        //        int id = (int)rdr["id"];
        //        string Name = (string)rdr["Name"];
        //        string Path = (string)rdr["Path"];
        //        string? UploadedDateTime = rdr["UploadedDateTime"].ToString();
        //        string[] asd = { Name, Path, UploadedDateTime };
        //
        //        DataObj.Add(id, asd);
        //    }
        //    return DataObj; 
        //}

    }
}
