using SampleConApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataAccessingApp
{
    static class Database
    {
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3321;Integrated Security=True";
        const string STRQUERY = "SELECT * FROM tblEmployee";
        const string STRDQUERY = " SELECT * FROM tblDept";
        

        public static DataTable GetAllRecords()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRQUERY, con);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                DataTable table = new DataTable("EmpRecords");
                table.Load(reader);
                return table;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public static DataTable GetAllRecords1()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRDQUERY, con);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                DataTable table = new DataTable("EmpRecords");
                table.Load(reader);
                return table;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
    }

    class Ex01ConnectionDemo
    {
        static void Main(string[] args)
        {
            //readingData();
            //displayTable();
            //displayDetails("Megha");
            //displayDetailsUsingParameters("Megha");
            //addNewRecord("Priyanka", "Udupi", 5677, 3);
            //displayDetails("Priyanka");
            //addNewRecordFromInputs();
            //displayTable();
            addNewRecordUsingStoredProcedure("Meghuuuu", "CKM", 3456, 4);
        }
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3321;Integrated Security=True";
        

        private static void displayDetails(string name)
        {
            string query = $"Select * from tblEmployee where EmpName like '%{name}%'";
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(query, con);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"EmpName : {reader[1]}\nEmpAddress : {reader[2]}\nEmpSalary : {reader[3]:c}\n" );
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        const string STRQUERY = "SELECT * FROM tblEmployee";
        private static void readingData()
        {
            SqlConnection sqlCon = new SqlConnection();
            sqlCon.ConnectionString = STRCONNECTION;

            SqlCommand sqlCommand = sqlCon.CreateCommand();
            sqlCommand.CommandText = STRQUERY;

            try
            {
                sqlCon.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["EmpName"]} from {reader["EmpAddress"]}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (sqlCon.State == System.Data.ConnectionState.Open)
                    sqlCon.Close();
            }
        }

        private static void displayTable()
        {
            try
            {
                var table = Database.GetAllRecords();
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"EmpName: {row[1]}\n EmpAddress: {row[2]}\n EmpSalary: {row[3]:c}\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        const string STRFIND = "SELECT * FROM tblEmployee WHERE EmpName = @name";

        private static void displayDetailsUsingParameters(string name)
        {
            SqlCommand cmd = new SqlCommand(STRFIND, new SqlConnection(STRCONNECTION));
            try
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"EmpName: {reader[1]}\nEmpAddress: {reader[2]}\nEmpSalary: {reader[3]:c}\n");
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        const string STRINSERT = "INSERT INTO tblEmployee VALUES(@NAME,@ADDRESS,@SALARY,@DEPTID)";
        private static void addNewRecord(string name, string address, int salary, int deptId)
        {
            SqlConnection sqlcon = new SqlConnection(STRCONNECTION);
            SqlCommand sqlcmd = new SqlCommand(STRINSERT, sqlcon);
            sqlcmd.Parameters.AddWithValue("@NAME", name);
            sqlcmd.Parameters.AddWithValue("@ADDRESS", address);
            sqlcmd.Parameters.AddWithValue("@SALARY", salary);
            sqlcmd.Parameters.AddWithValue("@DEPTID", deptId);

            try
            {
                sqlcon.Open();
                var rowsAffected = sqlcmd.ExecuteNonQuery();
                if(rowsAffected != 1)
                {
                    throw new Exception("Failed to add the record to the database");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlcon.Close();
            }
        }

        
        public static void displayDeptDetails()
        {
           // SqlConnection con = new SqlConnection(STRCONNECTION);
            //SqlCommand cmd = new SqlCommand(STRDQUERY, con);
            try
            {
                var table = Database.GetAllRecords1();
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"DeptId: {row[0]}    DeptName: {row[1]}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        const string STRINSERTPROC = "InsertEmployees ";
        private static void addNewRecordFromInputs()
        {
            Utilities.Prompt("Enter the name of the person");
            Utilities.Prompt("Enter the address of the person");
            Utilities.GetNumber("Enter the salary");
            displayDeptDetails();
        }

        private static void addNewRecordUsingStoredProcedure(string name, string address, int salary, int deptId)
        {
            int empId = 0;
            SqlConnection sqlcon = new SqlConnection(STRCONNECTION);
            SqlCommand sqlcmd = new SqlCommand(STRINSERTPROC, sqlcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@empName", name);
            sqlcmd.Parameters.AddWithValue("@empAddress", address);
            sqlcmd.Parameters.AddWithValue("@empSalary", salary);
            
            sqlcmd.Parameters.AddWithValue("@deptId", deptId);
            sqlcmd.Parameters.AddWithValue("@empId", empId);
            sqlcmd.Parameters[4].Direction = ParameterDirection.Output;

            try
            {
                sqlcon.Open();
                sqlcmd.ExecuteNonQuery();
                empId = Convert.ToInt32(sqlcmd.Parameters[4].Value);
                Console.WriteLine("The EmpId of the newly added Employee is "+ empId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                sqlcon.Close();
            }
        }
    }
}
