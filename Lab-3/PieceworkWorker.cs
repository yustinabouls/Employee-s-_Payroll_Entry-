// PieceworkWorker.cs
//         Title: IncInc Payroll (Piecework)
// Last Modified: November 1, 2020
//    Written By: Yustina Bouls
// Adapted from PieceworkWorker by Kyle Chapman, September 2019
// 
// This is a class representing individual worker objects. Each stores
// their own name and number of messages and the class methods allow for
// calculation of the worker's pay and for updating of shared summary
// values. Name and messages are received as strings.
// This is being used as part of a piecework payroll application.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab_3
{
    class PieceworkWorker
    {

        #region "Variable declarations"

        // Instance variables
        private string WorkerFirstName;
        private string WorkerLastName;
        private int employeeMessages;
        private decimal employeePay;

        // Shared summary value variables
        private static int totalEmployees = 0;
        private static int totalMessages = 0;
        private static decimal totalPay = 0M;

        #endregion

        #region "Constructors"

        /// <summary>
        /// PieceworkWorker constructor: accepts a worker's name and number of
        /// messages, sets and calculates values as appropriate.
        /// </summary>
        /// <param name="nameValue">the worker's name</param>
        /// <param name="messageValue">a worker's number of messages sent</param>
        public PieceworkWorker(string firstNameValue, string lastNameValue, string messagesValue)
        {
            // Validate and set the worker's name
            this.WorkerFirstName = firstNameValue;
            // Validate and set the worker's last name
            this.WorkerLastName = lastNameValue;
            // Validate Validate and set the worker's number of messages
            this.Messages = messagesValue;
            // Calculcate the worker's pay and update all summary values
            this.EntryDate = DateTime.Now;
            FindPay();

            DataAccess.InsertNewRecord(this);

        }

        /// <summary>
        /// PieceworkWorker constructor: empty constructor used strictly for inheritance and instantiation
        /// </summary>
        public PieceworkWorker()
        {

        }



        #endregion

        #region "Class methods"

        /// <summary>
        /// Currently called in the constructor, the findPay() method is
        /// used to calculate a worker's pay using threshold values to
        /// change how much a worker is paid per message. This also updates
        /// all summary values.
        /// </summary>
        private void FindPay()
        {

            // Declare a large bank of constants describing the pay structure.
            const int LowestMessageThreshold = 1000;
            const int LowMessageThreshold = 2000;
            const int MediumMessageThreshold = 3000;
            const int HighMessageThreshold = 4000;

            // Declare a large bank of constants for the various pay rates.
            const decimal LowestPayRate = 0.021M;
            const decimal LowPayRate = 0.028M;
            const decimal MediumPayRate = 0.035M;
            const decimal HighPayRate = 0.040M;
            const decimal HighestPayRate = 0.045M;

            // Identify the worker's payment level and calculate their pay.
            if (employeeMessages < LowestMessageThreshold)
            {
                Pay = employeeMessages * LowestPayRate;
            }
            else if (employeeMessages < LowMessageThreshold)
            {
                Pay = employeeMessages * LowPayRate;
            }
            else if (employeeMessages < MediumMessageThreshold)
            {
                Pay = employeeMessages * MediumPayRate;
            }
            else if (employeeMessages < HighMessageThreshold)
            {
                Pay = employeeMessages * HighPayRate;
            }
            else
            {
                Pay = employeeMessages * HighestPayRate;
            }

            // Update all shared summary values.
            TotalPay += employeePay;
           TotalMessages += employeeMessages;
        }
            TotalWorkers++;

        #endregion

        #region "Property Procedures"

        /// <summary>
        /// Gets and sets a worker's name
        /// </summary>
        /// <returns>an employee's name</returns>
        internal string FirstName
        {
            get
            {
                return WorkerFirstName;
            }
            set
            {
                // Check if the value is empty.
                if (value.Trim() != String.Empty)
                {
                    // Set the value.
                    WorkerFirstName = value;
                }
                else
                {
                    // Value was empty; report as an error.
                    throw new ArgumentNullException("Name", "You must enter a name to proceed.");
                }
            }
        }
        internal string LastName
        {
            get
            {
                return WorkerLastName;
            }
            set
            {
                // Check if the value is empty.
                if (value.Trim() != String.Empty)
                {
                    // Set the value.
                    WorkerLastName = value;
                }
                else
                {
                    // Value was empty; report as an error.
                    throw new ArgumentNullException("Name", "You must enter a firstname to proceed.");
                }
            }
        }

        /// <summary>
        /// Gets and sets the number of messages sent by a worker
        /// </summary>
        /// <returns>an employee's number of messages</returns>
        internal string Messages
        {
            get
            {
                return employeeMessages.ToString();
            }
            set
            {
                const int MinimumMessages = 1;
                const int MaximumMessages = 10000;

                // Check if the value is an integer, and attempt to store it.
                if (!int.TryParse(value, out employeeMessages))
                {
                    // Value was not a whole number; report as an error.
                    throw new ArgumentException("You must enter the messages as a whole number.", "Messages");
                }
                // If the stored value is not a positive number, report an error.
                else if (employeeMessages < MinimumMessages || employeeMessages > MaximumMessages)
                {
                    throw new ArgumentOutOfRangeException("Messages", "Messages must be entered between " + MinimumMessages + " and " + MaximumMessages + ".");
                }
            }
        }

        /// <summary>
        /// Gets the worker's pay
        /// </summary>
        /// <returns>a worker's pay</returns>
        internal decimal Pay { get; set; }
        /// <summary>
        /// Gets the overall pay for all workers
        /// </summary>
        /// <returns>the overall pay total for all workers</returns>

        internal static decimal TotalPay { get; private set; } = 0M;

        /// <summary>
        /// Gets the overall number of workers
        /// </summary>
        /// <returns>the overall number of workers</returns>
        internal static int TotalWorkers { get; private set; } = 0;


        /// <summary>
        /// Gets the overall number of messages sent
        /// </summary>
        /// <returns>the overall number of messages sent</returns>
        internal static int TotalMessages { get; private set; } = 0;
       

        /// <summary>
        /// Calculates and returns an average pay among all workers
        /// </summary>
        /// <returns>the average pay among all workers</returns>
        internal static decimal AveragePay
        {
            get
            {
                if (TotalWorkers == 0)
                {
                    return 0;
                }
                else
                {
                    return TotalPay / TotalWorkers;
                }
            }
        }

        internal DateTime EntryDate { get; set; }


        /// <summary>
        /// Gets the pay for the workers
        /// </summary>
        /// <returns>workers pay</returns>
        /// 

        internal static DataTable AllWorkers
        {
            get
            {
                return DataAccess.GetEmployeeList();
            }
        }

        #endregion
    }
}

