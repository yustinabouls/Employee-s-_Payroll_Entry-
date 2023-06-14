﻿// DataAccess.cs
//         Title: DataAccess - Data Access Layer for Piecework Payroll
// Last Modified: 
//    Written By: Yustina Bouls
// Based on code samples provided by Kyle Chapman
// 
// This is a module with a set of classes allowing for interaction between
// Piecework Worker data objects and a database.

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;

namespace Lab_3
{
    class DataAccess
    {

        #region "Connection String"

        /// <summary>
        /// Return connection string
        /// </summary>
        /// <returns>Connection string</returns>
        private static string GetConnectionString()
        {
            /* Somehow, we need to have a working connection string.
               This can be a local connecting string, like mine below,
               but you can't expect this to work on your machine (or
               anyone else's). If you use this, I will attempt to
               figure it out during grading.
               Other options may be available; you may want to experiment
               with remote databases but there are reasons I can't make
               this a requirement. */

            return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
                Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString() +
                "\\WorkerDatabase.mdf;Integrated Security=True";

            // The following is better - no absolute/fixed path - but it won't actually work while we're debugging. You can try a variant on this.
            //return "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" +
            //    Directory.GetCurrentDirectory() +
            //    "\\WorkerDatabase.mdf;Integrated Security=True;Connect Timeout=30";

        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Function that returns all workers in the database as a DataTable for display
        /// </summary>
        /// <returns>a DataTable containing all workers in the database</returns>
        internal static DataTable GetEmployeeList()
        {
            // Declare the SQL connection, SQL command, and SQL adapter
            SqlConnection dbConnection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("SELECT * FROM [tblEntries]", dbConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);

            // Declare a DataTable object that will hold the return value
            DataTable employeeTable = new DataTable();

            // Try to connect to the database, and use the adapter to fill the table
            try
            {
                dbConnection.Open();
                adapter.Fill(employeeTable);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                adapter.Dispose();
                dbConnection.Close();
            }

            // Return the populated DataTable
            return employeeTable;
        }

        /// <summary>
        /// Function to add a new worker to the worker database
        /// </summary>
        /// <param name="insertWorker">a worker object to be inserted</param>
        /// <returns>true if successful</returns>
        internal static bool InsertNewRecord(PieceworkWorker insertWorker)
        {
            // Create return value
            bool returnValue = false;

            // Declare the SQL connection
            SqlConnection dbConnection = new SqlConnection(GetConnectionString());

            // Create new SQL command and assign it paramaters
            SqlCommand command = new SqlCommand("INSERT INTO tblEntries VALUES(@firstName, @lastName, @messages, @pay, @entryDate)", dbConnection);
            command.Parameters.AddWithValue("@firstName", insertWorker.FirstName);
            command.Parameters.AddWithValue("@lastName", insertWorker.LastName);
            command.Parameters.AddWithValue("@messages", insertWorker.Messages);
            command.Parameters.AddWithValue("@pay", insertWorker.Pay);
            command.Parameters.AddWithValue("@entryDate", insertWorker.EntryDate);

            // The above SQL command is the same as the following:
            // SqlCommand command = new SqlCommand("INSERT INTO tblEntries VALUES(" + insertWorker.FirstName + ", " + insertWorker.LastName + ", " + insertWorker.Messages + ", " + insertWorker.Pay + ", " + insertWorker.EntryDate + ")", dbConnection);
            // Your choice if you think this version is nicer!

            // Try to insert the new record, return result
            try
            {
                dbConnection.Open();
                returnValue = (command.ExecuteNonQuery() == 1);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }

            // Return the true if this worked, false if it failed
            return returnValue;
        }

        /// <summary>
        /// Returns a total number of employees from the database.
        /// </summary>
        /// <returns>total employees, as a string</returns>
        internal static string GetTotalEmployees()
        {
            // Declare the SQL connection and the SQL command
            SqlConnection dbConnection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("SELECT COUNT(Id) FROM tblEntries", dbConnection);

            // Try to open a connection to the database and read the total. Return result.
            try
            {
                dbConnection.Open();
                return command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }
            // If something unexpected occured, set the result as error
            return "ERROR";
        }

        /// <summary>
        /// Returns a total number of messages from the database.
        /// </summary>
        /// <returns>total messages, as a string</returns>
        internal static string GetTotalMessages()
        {
            // Declare the SQL connection and the SQL command
            SqlConnection dbConnection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("SELECT SUM(Messages) FROM tblEntries", dbConnection);

            // Try to open a connection to the database and read the total. Return result.
            try
            {
                dbConnection.Open();
                return command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }
            // If something unexpected occured, set the result as error
            return "ERROR";
        }

        /// <summary>
        /// Returns a total pay among all employees from the database.
        /// </summary>
        /// <returns>total pay, as a string</returns>
        internal static string GetTotalPay()
        {
            // Declare the SQL connection and the SQL command
            SqlConnection dbConnection = new SqlConnection(GetConnectionString());
            SqlCommand command = new SqlCommand("SELECT SUM(Pay) FROM tblEntries", dbConnection);

            // Try to open a connection to the database and read the total. Return result.
            try
            {
                dbConnection.Open();
                return command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("A database error has been encountered: " + Environment.NewLine + ex.Message, "Database Error");
            }
            finally
            {
                dbConnection.Close();
            }
            // If something unexpected occured, set the result as error
            return "ERROR";
        }

        #endregion

    }
}
