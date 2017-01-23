using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GRH.IPHR.GRCBase;

namespace HIEService.DBHelper
{
    public class HIEPatientContactNumber
    {
        public int ContactNumberID
        {
            get;
            set;
        }

        public int PatientID
        {
            get;
            set;
        }

        public String ContactType
        {
            get;
            set;
        }

        public String ContactNumber
        {
            get;
            set;
        }

        public HIEPatientContactNumber(SqlDataReader reader)
        {
            ContactNumberID = BasicConverter.DbToIntValue(reader["ContactNumberID"]);
            PatientID = BasicConverter.DbToIntValue(reader["PatientID"]);
            ContactType = BasicConverter.DbToStringValue(reader["ContactType"]);
            ContactNumber = BasicConverter.DbToStringValue(reader["ContactNumber"]);
        }

        public static List<HIEPatient> GetContactNumbers(List<HIEPatient> patientList)
        {
            DataTable patientIDList = new DataTable();
            patientIDList.Columns.Add();
            foreach (int patientID in patientList.Select(p => p.PatientID).ToList())
            {
                patientIDList.Rows.Add(patientID);
            }

            List<HIEPatientContactNumber> contactNumberList = GetContactNumbersForPatientIDList(patientIDList);
            foreach (HIEPatient patient in patientList)
            {
                patient.ContactNumbers = contactNumberList.Where(contact => contact.PatientID == patient.PatientID).ToList();
            }
            return patientList;
        }

        private static List<HIEPatientContactNumber> GetContactNumbersForPatientIDList(DataTable patientIDList)
        {
            return new SafeExecute<List<HIEPatientContactNumber>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                try
                {
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_GetContactNumbersForPatientList";

                    param = cmd.Parameters.Add("@PatientList", SqlDbType.Structured);
                    param.Value = patientIDList;

                    reader = cmd.ExecuteReader();

                    List<HIEPatientContactNumber> retList = new List<HIEPatientContactNumber>();

                    while (reader.Read())
                    {
                        retList.Add(new HIEPatientContactNumber(reader));
                    }
                    return retList;
                }
                finally
                {
                    if (reader != null)
                    {
                        DBContext.CloseReader(reader);
                    }
                }
            }, ConfigurationManager.AppSettings["DBConnectionString"]).DoExecute();
        }

    }
}