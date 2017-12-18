using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERM
{
    /// <summary>
    /// Interaction logic for Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        private VMEditEmpData vmEmpEdit;

        #region Constructor
        public Edit()
        {
            InitializeComponent();
            vmEmpEdit = new VMEditEmpData();
            DataContext = vmEmpEdit;

            //Get values from ListView (main form)
            this.GetDataFromListView();
        }
        #endregion

        #region GotKeyboardFocus
        private void TxtEdit_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox currObject = (sender as TextBox);
            currObject.SelectAll();
        }
        #endregion

        #region GotMouseCapture
        private void TxtEdit_GotMouseCapture(object sender, MouseEventArgs e)
        {
            TextBox currObject = (sender as TextBox);
            currObject.SelectAll();
        }
        #endregion

        #region Clear Button
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (TextBox txtEdit in GrdEditMain.Children.OfType<TextBox>())
            {
                txtEdit.Clear();
            }
        }
        #endregion

        #region Save/ Update button
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            bool error;

            error = vmEmpEdit.SaveData();
            ChangeMessageColor(error);
        }
        #endregion

        #region Change Message Color
        private void ChangeMessageColor(bool err)
        {
            if (err)
                LblEditMessage.Foreground = Brushes.DarkRed;
            else
                LblEditMessage.Foreground = Brushes.DarkSeaGreen;
        }
        #endregion

        #region Get Data From ListView
        public void GetDataFromListView()
        {
            //Get data from the main form 
            vmEmpEdit.GetData();
        }
        #endregion
    }
}
