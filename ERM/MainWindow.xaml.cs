using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VMEmployee vmEmp;
        private Edit editForm;
        private PropertyInfo[] propValues;
        private int itemIndex;
        private bool error;
        private const int NO_INDEX_SELECTED = -1;

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            vmEmp = VMEmployee.GetInstance();
            DataContext = vmEmp;
            editForm = null;

            error = vmEmp.ReadData((short)VMEmployee.RequestType.SELECT_ALL);
            ChangeMessageColor(error);
        }
        #endregion 

        #region Edit Record
        private void BtnEditRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (editForm == null)
                {
                    editForm = new Edit();
                    editForm.Owner = this;
                    editForm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    editForm.Closed += EditForm_Closed;
                }

                editForm.Show();
                editForm.BringIntoView();
            }
            catch (Exception ex)
            {
                LblMessage.Foreground = Brushes.DarkRed;
                LblMessage.Content = ex.Message.ToString();
            }
        }

        private void EditForm_Closed(object sender, EventArgs e)
        {
            //Refresh Dataview from DB
            Refresh();
            LsvEmployees.SelectedIndex = (itemIndex > NO_INDEX_SELECTED) ? itemIndex : NO_INDEX_SELECTED;
            //Destroy object
            editForm = null;
        }
        #endregion

        #region Refresh GridView
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        private void Refresh()
        {
            error = vmEmp.ReadData((short)VMEmployee.RequestType.SELECT_ALL);
            LsvEmployees.Items.Refresh();
            ChangeMessageColor(error);
        }
        #endregion 

        #region Change Message Color
        private void ChangeMessageColor(bool err)
        {
            if (err)
                LblMessage.Foreground = Brushes.DarkRed;
            else
                LblMessage.Foreground = Brushes.DarkGreen;
        }
        #endregion

        #region ListView Selection Changed
        private void LsvEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listOfEmp = (sender as ListView);

            itemIndex = LsvEmployees.SelectedIndex;
            Type recordSel = (itemIndex > NO_INDEX_SELECTED) ? listOfEmp.SelectedItem.GetType(): null;

            if (recordSel != null)
            {        
                //Array of values in the selected line
                propValues = recordSel.GetProperties();

                //Give these values to the VM class
                vmEmp.EmployeeID = (int)propValues[(short)VMEmployee.FieldNbr.ONE].GetValue(listOfEmp.SelectedItem, null);
                vmEmp.Name = propValues[(short)VMEmployee.FieldNbr.TWO].GetValue(listOfEmp.SelectedItem, null).ToString();
                vmEmp.Position = propValues[(short)VMEmployee.FieldNbr.THREE].GetValue(listOfEmp.SelectedItem, null).ToString();
                vmEmp.Salary = (decimal)propValues[(short)VMEmployee.FieldNbr.FOUR].GetValue(listOfEmp.SelectedItem, null);

                //Get values from ListView (main form)
                if (editForm != null)
                    editForm.GetDataFromListView();
            }
        }
        #endregion
    }
}
