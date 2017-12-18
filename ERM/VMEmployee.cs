using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ERM
{
    class VMEmployee : INotifyPropertyChanged
    {
        private static VMEmployee employeeObject;
        private SqlConnection conn;
        private SqlCommand cmd;
        private List<string> requests;
        private const string CONNECTION_STRING = "Server=.\\SQLEXPRESS; Database=Personnel; Trusted_Connection=Yes;"; //"Data Source=Personnel; User ID=Joseph; Password=";
        private string message;
        private List<EmpRecord> emp;
        private int empID;
        private string empName, empPosition;
        private decimal empSalary;
        private const string MAIN_MESSAGE = "List of employees";

        #region Contructor
        private VMEmployee()
        {
            emp = new List<EmpRecord>();
            requests = new List<string>
            {
                "SELECT * FROM Employee",
                "UPDATE Employee SET Name = @name, position = @pos, payRateByHour = @payRate WHERE employeeID = "
            };
        }
        #endregion

        #region Enum RequestType and FieldNbr
        public enum RequestType
        {
            SELECT_ALL,
            UPDATE_ALL
        }

        public enum FieldNbr
        {
            ONE,
            TWO,
            THREE,
            FOUR
        }
        #endregion

        #region Targeted public properties
        public string Message
        {
            get { return message; }
            set { message = value; NotifyPropertyChanged(); }
        }
        public List<EmpRecord> Employee
        {
            get { return emp; }
            set { emp = value; NotifyPropertyChanged(); }
        }
        public int EmployeeID
        {
            get { return empID; }
            set { empID = value; NotifyPropertyChanged(); }
        }
        public string Name
        {
            get { return empName; }
            set { empName = value; NotifyPropertyChanged(); }
        }
        public string Position
        {
            get { return empPosition; }
            set { empPosition = value; NotifyPropertyChanged(); }
        }
        public decimal Salary
        {
            get { return empSalary; }
            set { empSalary = value; NotifyPropertyChanged(); }
        }
        #endregion

        #region Read or Update Data
        public bool ReadData(short requestID, VMEditEmpData empData = null)
        {
            bool error = false;

            try
            {
                using (conn = new SqlConnection(CONNECTION_STRING))
                {
                    EmpRecord record;

                    conn.Open();
                    cmd = new SqlCommand();
                    cmd.Connection = conn;

                    switch (requestID)
                    {
                        case (short)RequestType.SELECT_ALL:
                            Employee = null;
                            Employee = new List<EmpRecord>();

                            cmd.CommandText = requests[(short)RequestType.SELECT_ALL];
                            SqlDataReader rd = cmd.ExecuteReader();

                            foreach (IDataRecord dataRow in rd)
                            {
                                empID = dataRow.GetInt32((short)FieldNbr.ONE);
                                empName = dataRow.GetString((short)FieldNbr.TWO);
                                empPosition = dataRow.GetString((short)FieldNbr.THREE);
                                empSalary = dataRow.GetDecimal((short)FieldNbr.FOUR);

                                record = new EmpRecord() { ID = empID, Name = empName, Position = empPosition, Salary = empSalary };
                                emp.Add(record);
                            }

                            break;
                        case (short)RequestType.UPDATE_ALL:
                            cmd.CommandText = requests[(short)RequestType.UPDATE_ALL] + empData.EditID;
                            cmd.Parameters.AddWithValue("@name", empData.EditName);
                            cmd.Parameters.AddWithValue("@pos", empData.EditPosition);
                            cmd.Parameters.AddWithValue("@payRate", empData.EditSalary);
                            cmd.ExecuteNonQuery();

                            break;
                    }
                }

                Message = MAIN_MESSAGE;
            }
            catch (Exception ex)
            {
                error = true;
                Message = ex.Message.ToString();
            }

            return error;
        }
        #endregion

        #region Get Object Instance
        public static VMEmployee GetInstance()
        {
            if (employeeObject == null)
                employeeObject = new VMEmployee();

            return employeeObject;
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
