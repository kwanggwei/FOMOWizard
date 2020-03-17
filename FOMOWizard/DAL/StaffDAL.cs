﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FOMOWizard.Models;
using System.Data;

namespace FOMOWizard.DAL
{
    public class StaffDAL
    {
        private IConfiguration Configuration { get; set; }
        private SqlConnection conn;
        //Constructor
        public StaffDAL()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            //Read ConnectionString from appsettings.json file 
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("FOMOConnectionString");

            //Instantiate a SqlConnection object with the Connection String read. 
            conn = new SqlConnection(strConn);
        }

        public List<Staff> GetAllStaff()
        {
            //Instantiate a SqlCommand object, supply it with a 
            //SELECT SQL statement that operates against the database, 
            //and the connection object for connecting to the database. 
            SqlCommand cmd = new SqlCommand("SELECT * FROM Staff ORDER BY StaffID", conn);
            //Instantiate a DataAdapter object and pass the 
            //SqlCommand object created as parameter. 
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Create a DataSet object to contain records get from database 
            DataSet result = new DataSet();
            //Open a database connection 
            conn.Open();
            //Use DataAdapter, which execute the SELECT SQL through its 
            //SqlCommand object to fetch data to a table "StaffDetails" 
            //in DataSet "result". 
            da.Fill(result, "StaffDetails");
            //Close the database connection 
            conn.Close();
            //Transferring rows of data in DataSet’s table to “Staff” objects 
            List<Staff> staffList = new List<Staff>();
            foreach (DataRow row in result.Tables["StaffDetails"].Rows)
            {
                staffList.Add(
                    new Staff
                    {
                        StaffID = Convert.ToInt32(row["StaffID"]),
                        Name = row["Name"].ToString(),
                        Email = row["Email"].ToString(),
                        Password = row["Password"].ToString(),
                        Department = row["Department"].ToString(),
                        Approved = Convert.ToBoolean(row["Approved"])
                    }
            );
            }
            return staffList;
        }

        public Staff GetStaff(string loginID)
        {
            //Instantiate a SqlCommand object, supply it with a 
            //SELECT SQL statement that operates against the database, 
            //and the connection object for connecting to the database. 
            SqlCommand cmd = new SqlCommand("SELECT * FROM Staff WHERE Email = '" + loginID + "'", conn);
            //Instantiate a DataAdapter object and pass the 
            //SqlCommand object created as parameter. 
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //Create a DataSet object to contain records get from database 
            DataSet result = new DataSet();

            //Open a database connection 
            conn.Open();
            //Use DataAdapter, which execute the SELECT SQL through its 
            //SqlCommand object to fetch data to a table "StaffDetails" 
            //in DataSet "result". 
            da.Fill(result, "StaffDetails");
            //Close the database connection 
            conn.Close();
            Staff staff = new Staff() { };
            foreach (DataRow row in result.Tables["StaffDetails"].Rows)
            {
                staff = new Staff
                {
                    StaffID = Convert.ToInt32(row["StaffID"]),
                    Name = row["Name"].ToString(),
                    Password = Convert.ToString(row["Password"]),
                    Email = row["Email"].ToString(),
                    Department = Convert.ToString(row["Department"]),
                    Approved = Convert.ToBoolean(row["Approved"])
                };
            }
            return staff;
        }
    }
}