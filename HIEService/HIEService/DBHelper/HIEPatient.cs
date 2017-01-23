using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using GRH.IPHR.GRCBase;
using HIEService.RequestHandlers;

namespace HIEService.DBHelper
{
    public class HIEPatient
    {
        public int PatientID
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }

        public string FamilyName
        {
            get;
            set;
        }

        public char Gender
        {
            get;
            set;
        }

        public DateTime DateOfBirth
        {
            get;
            set;
        }

        public String SSN
        {
            get;
            set;
        }

        public String EMPID
        {
            get;
            set;
        }

        public String IPHRID
        {
            get;
            set;
        }

        public List<HIEPatientAddress> PatientAddresses
        {
            get;
            set;
        }

        public List<HIEPatientContactNumber> ContactNumbers
        {
            get;
            set;
        }

        public HIEPatient(SqlDataReader reader)
        {
            PatientID = BasicConverter.DbToIntValue(reader["PatientID"]);
            FirstName = BasicConverter.DbToStringValue(reader["Firstname"]);
            MiddleName = BasicConverter.DbToStringValue(reader["MiddleName"]);
            FamilyName = BasicConverter.DbToStringValue(reader["FamilyName"]);
            Gender = BasicConverter.DbToCharValue(reader["Gender"]);
            DateOfBirth = BasicConverter.DbToDateValue(reader["DateOfBirth"]);
            SSN = BasicConverter.DbToStringValue(reader["SSN"]);
            EMPID = BasicConverter.DbToStringValue(reader["EMPID"]);
            IPHRID = BasicConverter.DbToStringValue(reader["IPHRID"]);
        }

        public bool CheckPatientMatchWithFilter(PatientAddress addressFilter,DateTime? DOB)
        {
            if (DOB.HasValue && DateOfBirth != DOB.Value)
                return false;
            foreach (HIEPatientAddress address in PatientAddresses)
            {
                if (address.CheckMatchingWithFilter(addressFilter))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<HIEPatient> GetPatientList(String SSN, String firstName, String familyName)
        {
            if (String.IsNullOrEmpty(SSN) && (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(familyName)))
            {
                throw new Exception("Insufficient data for searching patient");
            }
            List<HIEPatient> patientList = SearchPatientWithNameOrSSN(SSN, firstName, familyName);
            patientList = HIEPatientAddress.GetPatientAddresses(patientList);
            patientList = HIEPatientContactNumber.GetContactNumbers(patientList);
            return patientList;
        }

        private static List<HIEPatient> SearchPatientWithNameOrSSN(String SSN, String firstName, String familyName)
        {
            return new SafeExecute<List<HIEPatient>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                try
                {
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_SearchPatientWithNameOrSSN";

                    param = cmd.Parameters.Add("@SSN", SqlDbType.NVarChar);
                    param.Value = SSN;

                    param = cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar);
                    param.Value = firstName;

                    param = cmd.Parameters.Add("@FamilyName", SqlDbType.NVarChar);
                    param.Value = familyName;

                    reader = cmd.ExecuteReader();

                    List<HIEPatient> retList = new List<HIEPatient>();

                    while (reader.Read())
                    {
                        retList.Add(new HIEPatient(reader));
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