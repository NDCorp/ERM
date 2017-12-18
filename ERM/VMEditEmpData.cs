using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERM
{
    class VMEditEmpData : INotifyPropertyChanged
    {
        private VMEmployee vmEmp;
        private string message;
        private int empID;
        private string empName, empPosition;
        private decimal empSalary;

        #region Constructor
        public VMEditEmpData()
        {
            vmEmp = VMEmployee.GetInstance();
        }
        #endregion

        #region Targeted public properties
        public string EditFormMessage
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged(); }
        }
        public int EditID
        {
            get { return empID; }
            set { empID = value; NotifyPropertyChanged(); }
        }
        public string EditName
        {
            get { return empName; }
            set { empName = value; NotifyPropertyChanged(); }
        }
        public string EditPosition
        {
            get { return empPosition; }
            set { empPosition = value; NotifyPropertyChanged(); }
        }
        public decimal EditSalary
        {
            get { return empSalary; }
            set { empSalary = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Save/ Update Data
        public bool SaveData()
        {
            bool error = false;

            try
            {
                error = vmEmp.ReadData((short)VMEmployee.RequestType.UPDATE_ALL, this);
                EditFormMessage = "Successfully saved.";
            }
            catch (Exception ex)
            {
                error = true;
                EditFormMessage = ex.Message.ToString();
            }

            return error;
        }
        #endregion

        #region GetData
        public void GetData()
        {
            EditID = vmEmp.EmployeeID;
            EditName = vmEmp.Name;
            EditPosition = vmEmp.Position;
            EditSalary = vmEmp.Salary;
        }
        #endregion

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}