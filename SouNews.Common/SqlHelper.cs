using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouNews.Common
{
    public class SqlHelper
    {
        private static readonly string str = ConfigurationManager.ConnectionStrings["conStr"].ConnectionString;

        public static DataTable ExecuteTable(string sql, params SqlParameter[] ps)
        {
            DataTable dt=new DataTable();
            using (SqlDataAdapter adapter=new SqlDataAdapter(sql,str))
            {
                adapter.Fill(dt);
            }
            return dt;
        }

        public static object ExecuteScalar(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection con=new SqlConnection(str))
            {
                con.Open();
                using (SqlCommand cmd=new SqlCommand(sql,con))
                {
                    if (ps!=null)
                    {
                        cmd.Parameters.AddRange(ps);
                    }
                    return cmd.ExecuteScalar();
                }
            }
        }

        public static int ExecuteNonQuery(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection con=new SqlConnection(str))
            {
                con.Open();
                using (SqlCommand cmd=new SqlCommand(sql,con))
                {
                    if (ps!=null)
                    {
                        cmd.Parameters.AddRange(ps);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
