using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using GRH.IPHR.GRCBase;
using HIEService.RequestHandlers;

namespace HIEService.DBHelper
{
    public class HIEPatientAddress
    {
        public int AddressID
        {
            get;
            set;
        }

        public int PatientID
        {
            get;
            set;
        }

        public String StreetAddressLine
        {
            get;
            set;
        }

        public String City
        {
            get;
            set;
        }

        public String State
        {
            get;
            set;
        }

        public String Country
        {
            get;
            set;
        }

        public String PostalCode
        {
            get;
            set;
        }

        public HIEPatientAddress(SqlDataReader reader)
        {
            AddressID = BasicConverter.DbToIntValue(reader["AddressID"]);
            PatientID = BasicConverter.DbToIntValue(reader["PatientID"]);
            StreetAddressLine = BasicConverter.DbToStringValue(reader["StreetAddressLine"]);
            City = BasicConverter.DbToStringValue(reader["City"]);
            State = BasicConverter.DbToStringValue(reader["StateOrProvince"]);
            Country = BasicConverter.DbToStringValue(reader["County"]);
            PostalCode = BasicConverter.DbToStringValue(reader["PostalCode"]);
        }

        public bool CheckMatchingWithFilter(PatientAddress addressFilter)
        {
            if (!String.IsNullOrEmpty(addressFilter.streetAddressLine) && string.Equals(addressFilter.streetAddressLine,StreetAddressLine,StringComparison.OrdinalIgnoreCase))
                return false;
            if (!String.IsNullOrEmpty(addressFilter.city) && string.Equals(addressFilter.city,City,StringComparison.OrdinalIgnoreCase))
                return false;
            if (!String.IsNullOrEmpty(addressFilter.state) && string.Equals(addressFilter.state, State, StringComparison.OrdinalIgnoreCase))
                return false;
            if (!String.IsNullOrEmpty(addressFilter.PostalCode) && string.Equals(addressFilter.PostalCode, PostalCode,StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public static List<HIEPatient> GetPatientAddresses(List<HIEPatient> patientList)
        {
            DataTable patientIDList = new DataTable();
            patientIDList.Columns.Add();
            foreach (int patientID in patientList.Select(p => p.PatientID).ToList())
            {
                patientIDList.Rows.Add(patientID);
            }

            List<HIEPatientAddress> addressList = GetAddressesForPatientIDList(patientIDList);
            foreach (HIEPatient patient in patientList)
            {
                patient.PatientAddresses = addressList.Where(addr => addr.PatientID == patient.PatientID).ToList();
            }
            return patientList;
        }

        private static List<HIEPatientAddress> GetAddressesForPatientIDList(DataTable patientIDList)
        {
            return new SafeExecute<List<HIEPatientAddress>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                try
                {
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_GetAddressesForPatientList";

                    param = cmd.Parameters.Add("@PatientList", SqlDbType.Structured);
                    param.Value = patientIDList;

                    reader = cmd.ExecuteReader();

                    List<HIEPatientAddress> retList = new List<HIEPatientAddress>();

                    while (reader.Read())
                    {
                        retList.Add(new HIEPatientAddress(reader));
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