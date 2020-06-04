using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace sampleapp
{
    class Program
    {
        static string ServerName = "Your Server Name";
        static string DatabaseName = "Your Database Name";
        
        static void Main(string[] args)
        {
            SqlConnection tmpConn;
            string sqlCreateDBQuery;
            tmpConn = new SqlConnection();
            tmpConn.ConnectionString = "SERVER = " + ServerName + "; Trusted_Connection=True;";
            sqlCreateDBQuery = " CREATE DATABASE " + DatabaseName;

            SqlCommand myCommand = new SqlCommand(sqlCreateDBQuery, tmpConn);
            try
            {
                tmpConn.Open();
                Console.WriteLine(sqlCreateDBQuery);
                var result = myCommand.ExecuteNonQuery();
                Console.WriteLine(result);
                Console.WriteLine("Database has been created successfully!");

                if (result == -1)
                {
                    RunScript(DatabaseName);
                }
                tmpConn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void RunScript(string dbName)
        {
            SqlConnection tmpConn;
            tmpConn = new SqlConnection();
            try
            {                
                FileInfo file = new FileInfo("C:\\Users\\ADMIN\\Desktop\\WFHDocuments\\DbScripts\\Generate schema\\Script.sql");
                string script = file.OpenText().ReadToEnd();

                script = script.Replace("GO", "");
                script = Regex.Replace(script, "([/*][*]).*([*][/])", "");
                script = Regex.Replace(script, "\\s{2,}", " ");                

                tmpConn.ConnectionString = "Server=LAPTOP-Pxxxxxxx\\SQLEXPRESS;" + "Database=" + dbName + "; Trusted_Connection=True;";
                

                using (SqlConnection connection = new SqlConnection(tmpConn.ConnectionString))
                {
                    Server server = new Server(new ServerConnection(connection));

                    try
                    {
                        server.ConnectionContext.ExecuteNonQuery(script);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }
            
        }

    }
}
