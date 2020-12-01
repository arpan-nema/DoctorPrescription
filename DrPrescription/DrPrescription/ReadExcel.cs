using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;

namespace DrPrescription
{
    public class ReadExcel
    {
        public static List<string> GetAllMedicines()
        {
            var currentPath=System.IO.Path.GetFullPath(".");
            currentPath += @"\Data\Meds.xlsx";
            var result = new List<string>();

            var connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES""",currentPath);
            //var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;"+
            //          "Data Source='" + currentPath +
            //          "';Extended Properties=\"Excel 12.0;HDR=YES;\"";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                //conn.Open();
                OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter
                ("select * from [Sheet1$]", conn);
                DataSet excelDataSet = new DataSet();
                objDA.Fill(excelDataSet);
                var table = excelDataSet.Tables[0];
                foreach (DataRow r in table.Rows)
                {
                    result.Add(r[1].ToString());
                }
                conn.Close();
            }
            return result;
        }
        public static Medicine GetDoze(string medicineName)
        {
            var currentPath = System.IO.Path.GetFullPath(".");
            currentPath += @"\Data\Meds.xlsx";
            var result = new Medicine() {
                Name=medicineName
            };

            var connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES""", currentPath);
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                //conn.Open();
                OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter
                ("select * from [Sheet1$]", conn);
                DataSet excelDataSet = new DataSet();
                objDA.Fill(excelDataSet);
                var table = excelDataSet.Tables[0];
                foreach (DataRow r in table.Rows)
                {
                    if (r[1].ToString() == medicineName)
                    {
                        result.Morning = r[2].ToString() != "0";
                        result.Evening = r[3].ToString() != "0";
                        result.Night = r[4].ToString() != "0";
                        result.Dose = "1";
                        if (r[2].ToString() != "0")
                        {
                            result.Dose = r[2].ToString();
                        }
                        else if (r[3].ToString() != "0")
                        {
                            result.Dose = r[3].ToString();
                        }
                        else if (r[4].ToString() != "0")
                        {
                            result.Dose = r[4].ToString();
                        }
                        break;
                    }
                    
                }
                conn.Close();
            }
            return result;
        }
        public static List<string> GetSymptoms()
        {
            var currentPath = System.IO.Path.GetFullPath(".");
            currentPath += @"\Data\Meds.xlsx";
            var connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES""", currentPath);
            var result = new List<string>();
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                //conn.Open();
                OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter
                ("select * from [Symptoms$]", conn);
                DataSet excelDataSet = new DataSet();
                objDA.Fill(excelDataSet);
                var table = excelDataSet.Tables[0];
               
                foreach (DataRow r in table.Rows)
                {
                    result.Add(r[1].ToString());
                }
                conn.Close();
            }
            return result;
        }
        public static string AddNewMedicine(string medicineName,string morning,string evening, string night)
        {
            var result = "Method just initiated";
            var allMeds=GetAllMedicines();
            if (allMeds.Any(x => x.ToUpper().Trim() == medicineName.ToUpper().Trim()))
            {
                result="Medicine already present.";
                return result;
            }


            var currentPath = System.IO.Path.GetFullPath(".");
            currentPath += @"\Data\Meds.xlsx";
            var connectionString = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES""", currentPath);
            //var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;"+
            //          "Data Source='" + currentPath +
            //          "';Extended Properties=\"Excel 12.0;HDR=YES;\"";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                string cmdTxt = string.Format("Insert into [Sheet1$] ([SNo],[MedicinesNames],[Morning],[Evening],[Night]) values({0},'{1}','{2}','{3}','{4}')", allMeds.Count + 1, medicineName, morning,evening,night);
                OleDbCommand cmd = new OleDbCommand(cmdTxt, conn);
                //cmd.Parameters.AddWithValue("@MedicinesNames", medicineName);
                //cmd.Parameters.AddWithValue("@SNo", (allMeds.Count + 1).ToString());
                //cmd.Parameters.AddWithValue("@Cause", cure);
                conn.Open();
                result = "Not able to add medicine";
                if (cmd.ExecuteNonQuery() > 0)
                {
                    result = "Medicine added successfuly.";
                }
                //OleDbDataAdapter objDA = new System.Data.OleDb.OleDbDataAdapter();
                //objDA.InsertCommand = cmd;
                //objDA.ex
                //DataSet excelDataSet = new DataSet();
                //objDA.Fill(excelDataSet);
                //var table = excelDataSet.Tables[0];
                conn.Close();
            }
            return result;
        }
    }
}
