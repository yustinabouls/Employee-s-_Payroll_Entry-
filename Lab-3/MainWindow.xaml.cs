// MainWindow.xaml.cs
// Title: MainWindow (Piecwwork)
// Written By: Yustina Bouls
// Adapted from PieceworkWorker by Kyle Chapman, November 2020
// 
// This is a tab controltab which are (Payroll Entry, Summary, Employee List) and this lab is a continuation of lab 1
// 2. 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab_3
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Make a new instance of the worker object.
                PieceworkWorker workerObject = new PieceworkWorker(txtWorkerFirstName.Text,txtWorkerLastName.Text, txtMessagesSent.Text);

                // Display the worker's pay as currency.
                txtWorkerPay.Text = workerObject.Pay.ToString("c");

                // Disable input controls as per requirements.
                txtWorkerFirstName.IsEnabled = false;
                txtWorkerLastName.IsEnabled = false;
                txtMessagesSent.IsEnabled = false;
                btnCalculate.IsEnabled = false;

                // Clear error messages
                lblNameError.Content = "";
                lblMessagesError.Content = "";

                // Set focus to the Clear button to facilitate data entry.
                btnClear.Focus();
            }
            catch (ArgumentException ex)
            {
                // Depending on which field is in error, report and highlight the appropriate field.
                if (ex.ParamName == "Name")
                {
                    lblNameError.Content = "Entry error! " + ex.Message;
                    //HighlightTextbox(txtWorkerName);
                }
                else if (ex.ParamName == "Messages")
                {
                    lblMessagesError.Content = "Entry error! " + ex.Message;
                    //HighlightTextbox(txtMessagesSent);
                }
            }
            // Catch other arbitrary exceptions!
            catch (Exception ex)
            {
                // Absurd, lengthy, detailed error message.
                MessageBox.Show("Critical error! Please contact the IT department and provide the following information:\n\nType: " + ex.GetType() + "\nSource: " + ex.Source + "\nMessage: " + ex.Message + "\n\n" + ex.StackTrace, "Unknown Error!");
            }


        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            SetDefaults();
        }
        private void SetDefaults()
        {
            // Clear all input and output fields
            txtWorkerFirstName.Clear();
            txtWorkerLastName.Clear();
            txtMessagesSent.Clear();
            txtWorkerPay.Clear();

            // Clear error messages
            lblNameError.Content = "";
            lblMessagesError.Content = "";

            // Re-enable any controls that may be disabled.
            btnCalculate.IsEnabled = true;
            txtWorkerFirstName.IsEnabled = true;
            txtMessagesSent.IsEnabled = true;

            // Set focus to the first entry field.
            txtWorkerFirstName.Focus();
        }
        
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void tbcPayrollInterface_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tbcPayrollInterface.SelectedItem == tbiEmployeeList)
            {
                dgemployeeview.ItemsSource = PieceworkWorker.AllWorkers.DefaultView;
            }
            else if (tbcPayrollInterface.SelectedItem == tbiSummary)
                {
                    txtTotalWorkers.Text = PieceworkWorker.TotalWorkers.ToString();
                    txtTotalMessages.Text = PieceworkWorker.TotalMessages.ToString();
                    txtTotalPay.Text = PieceworkWorker.TotalPay.ToString("c");
                    txtAveragePay.Text = PieceworkWorker.AveragePay.ToString("c");
                }
            
           


        }

    }
}


    




