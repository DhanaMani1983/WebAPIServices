using log4net;
using Saga.Gmd.WebApiServices.Models.Customer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{
    public class MciSourceDataAccess : IMciSourceDataAccess
    {
 
        private string MciCrDbConnectionString { get; }

        private static  ILog _logger = LogManager.GetLogger(typeof(MciSourceDataAccess));

        public MciSourceDataAccess()
        {          
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
        }

        public List<string> GetAllSourceKeys()
        {
            List<string> sourceKeyList = null;

            try
            {
                var sourceKeyQuery =
                    $"(Select distinct (Code) from dbo.MCI_SOURCE_KEY) Union (Select System_Source from dbo.MCI_SOURCE_KEY)";
                var adapter = new SqlDataAdapter(sourceKeyQuery, MciCrDbConnectionString);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                sourceKeyList = BuildSourceKey(ds.Tables[0]);
            }
            //catch (SqlException ex)
            //{
            //    _logger.Error("MciSourceDataAccess - GetAllSourceKeys (SQLException):" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                
            //    throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            //}
            catch (Exception ex)
            {
                _logger.Error("MciSourceDataAccess - GetAllSourceKeys:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return sourceKeyList;
        }

        private List<string> BuildSourceKey(DataTable table)
        {
            List<string> lists = new List<string>();
            try
            {
                foreach (DataRow row in table.Rows)
                {
                    lists.Add(row[0].ToString());
                }
            }
            catch (SqlException ex)
            {
                _logger.Error("MciSourceDataAccess - BuildSourceKey (SQLException):" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            catch (Exception ex)
            {
                _logger.Error("MciSourceDataAccess - BuildSourceKey:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }

            return lists;
        }
    }
}
