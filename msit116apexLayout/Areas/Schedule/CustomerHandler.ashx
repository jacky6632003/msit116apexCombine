

using System;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Configuration;

public class CustomerHandler : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        string term = "a";
        if (context.Request.QueryString["term"] != null)
        {
            term = context.Request.QueryString["term"];
        }
        String strConn = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;
        List<String> suggestions = new List<String>();
        using (SqlConnection conn = new SqlConnection(strConn))
        {
            string strCmd = "select CustomerID from Customers where CustomerID like @CustomerID order by CustomerID";
            SqlCommand cmd = new SqlCommand(strCmd, conn);
            cmd.Parameters.AddWithValue("@CustomerID", term + "%");
            conn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                suggestions.Add(dr[0].ToString());
            }
        }


        context.Response.ContentType = "application/json";
        context.Response.Write(JsonConvert.SerializeObject(suggestions));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}