using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using GRH.IPHR.GRCBase;
using HIEService.RequestHandlers;

namespace HIEService.DBHelper
{
    public class HIEPatientDocument
    {
        public string DocumentUniqueID
        {
            get;
            set;
        }

        public int PatientID
        {
            get;
            set;
        }

        public XElement Document
        {
            get;
            set;
        }

        public bool IsDeprecated
        {
            get;
            set;
        }

        public HIEPatientDocument(SqlDataReader reader)
        {
            if (reader.GetSchemaTable().Select("ColumnName='PatientID'").Length > 0)
            {
                PatientID = BasicConverter.DbToIntValue(reader["PatientID"]);
            }
            if (reader.GetSchemaTable().Select("ColumnName='Document'").Length > 0)
            {
                Document = BasicConverter.DbToXElementValue(reader["Document"]);
            }
            DocumentUniqueID = BasicConverter.DbToStringValue(reader["DocumentUniqueID"]);
            IsDeprecated = BasicConverter.DbToBoolValue(reader["IsDeprecated"]);
        }

        public static List<HIEPatientDocument> GetDocumentForUniqueID(List<DocumentInfo> documentUniqueIDList)
        {
            DataTable docIDTable = new DataTable();
            docIDTable.Columns.Add();
            foreach (DocumentInfo docUniqueID in documentUniqueIDList)
            {
                docIDTable.Rows.Add(docUniqueID.DocumentUniqueId);
            }
            return new SafeExecute<List<HIEPatientDocument>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                try
                {
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_GetDocumentsForUniqueID";

                    param = cmd.Parameters.Add("@UniqueIDList", SqlDbType.Structured);
                    param.Value = docIDTable;

                    reader = cmd.ExecuteReader();

                    List<HIEPatientDocument> retrievedDocs = new List<HIEPatientDocument>();
                    while (reader.Read())
                    {
                        retrievedDocs.Add(new HIEPatientDocument(reader));
                    }
                    return retrievedDocs;
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

        public static List<HIEPatientDocument> GetDocumentSummaryForEmpId(string EMPID)
        {
            return new SafeExecute<List<HIEPatientDocument>>(delegate(DBContext dbContext)
            {
                SqlDataReader reader = null;
                try
                {
                    SqlParameter param = null;
                    SqlCommand cmd = dbContext.ContextSqlCommand;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "Proc_GetDocumentUniqueIDList";

                    param = cmd.Parameters.Add("@EMPID", SqlDbType.NVarChar);
                    param.Value = EMPID;
                    reader = cmd.ExecuteReader();

                    List<HIEPatientDocument> uniqueIdList = new List<HIEPatientDocument>();
                    while (reader.Read())
                    {
                        uniqueIdList.Add(new HIEPatientDocument(reader));
                    }
                    return uniqueIdList;
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