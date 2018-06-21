using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Models;
using TravelSummary = Saga.Gmd.WebApiServices.Models.Customer.TravelSummary;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Saga.Gmd.WebApiServices.Common;
using log4net;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{
    public interface ITravelSummaryDataAccess
    {
        dynamic GetTravelSummary(int numeroId, NameAndAddress nameAndAddress = null);
    }

    public class TravelSummaryDataAccess : ITravelSummaryDataAccess
    {
        private readonly ILog _logger;
        public string MciCrDbConnectionString { get; set; }
        private ICustomerDataAccess _customerDataAccess;

        public TravelSummaryDataAccess(ILog logger, ICustomerDataAccess customerDataAccess)
        {
            _customerDataAccess = customerDataAccess;
            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
            _logger = logger;
        }


        public dynamic GetTravelSummary(int numeroId, NameAndAddress nameAndAddress = null)
        {
            //TravelSummaryStub stub = new TravelSummaryStub();
            //return stub.Get();
            int? pKey = 0;
            if (nameAndAddress != null)
            {
                pKey = GetPersistantKey(nameAndAddress);
            }

            using (var conn = new SqlConnection(MciCrDbConnectionString))
            {
                using (var cmd = new SqlCommand("[MCC].[get_travel_summary]", conn))
                {
                    try
                    {
                        cmd.Connection.Open();

                        var param = new SqlParameter("@p_numero_id", SqlDbType.Int);
                        param.SqlValue = numeroId;
                        cmd.Parameters.Add(param);

                        var param1 = new SqlParameter("@p_pkey", SqlDbType.Int);
                        param1.SqlValue = pKey;
                        cmd.Parameters.Add(param1);

                        cmd.CommandType = CommandType.StoredProcedure;

                        var ds = new DataSet();
                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        if (ds.Tables.Count == 0)
                        {
                            return "No Travel summary records found for the customer";
                        }

                        return BuildTravelSummary(ds);

                    }
                    catch (SqlException ex)
                    {

                        _logger.Info("TravelSummary.GetTravelSummary (SqlException) :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));

                    }
                    catch (Exception ex)
                    {
                        _logger.Info("TravelSummary.GetTravelSummary :" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                        throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
                    }

                }
            }

        }


        private int? GetPersistantKey(NameAndAddress nameAndAddress)
        {
            CustomerMatchParameter param = new CustomerMatchParameter();
            param.FirstName = nameAndAddress.FirstName;
            param.Surname = nameAndAddress.Surname;
            param.Dob = nameAndAddress.Dob;
            param.Title = nameAndAddress.Title;
            param.Email = nameAndAddress.Email;
            param.Phone = nameAndAddress.Phone;
            param.Address1 = nameAndAddress.Address.Address1;
            param.Address2 = nameAndAddress.Address.Address2;
            param.Address3 = nameAndAddress.Address.Address3;
            param.Address4 = nameAndAddress.Address.Address4;
            param.Postcode = nameAndAddress.Address.Postcode;
            param.MatchType = "NAME_AND_ADDRESS";

            var matchedCustomers = _customerDataAccess.GetMatchingCustomers(param).FirstOrDefault();

            return matchedCustomers?.CustomerId;
        }

        private TravelSummary BuildTravelSummary(DataSet ds)
        {
            //List<TravelSummary> travelSummaryList = new List<TravelSummary>();


            TravelSummary travelSummary = new TravelSummary();
            try
            {
                var table = ds.Tables[0];
                foreach (DataRow row in table.Rows)
                {


                    travelSummary.NumberOfNightsOnBoard = row["bc_accumulated_nights"] == null ? 0 : row["bc_accumulated_nights"].ToString().ToIntParsed();
                    travelSummary.SagaMagazineHolderFlag = row["saga_magazine_holder"] == null ? false : row["saga_magazine_holder"].ToString().ToBool();
                    travelSummary.SagaPlatinumCardHolderFlag = row["saga_platinum_card_holder"] == null ? false : row["saga_platinum_card_holder"].ToString().ToBool();
                    travelSummary.SuppressionsFlag = row["suppression_flag"] == null ? false : row["suppression_flag"].ToString().ToBool();
                    travelSummary.MySagaCustomerFlag = row["my_saga_customer_flag"] == null ? false : row["my_saga_customer_flag"].ToString().ToBool();
                    travelSummary.BritanniaClubMember = row["britannia_club_member"]?.ToString();
                    travelSummary.SagaMembershipStatus = row["saga_membership_status"]?.ToString();
                    travelSummary.BlockedCustomerFlag = row["blocked_customer_flag"] == null ? false : row["blocked_customer_flag"].ToString().ToBool();
                    // travelSummaryList.Add(travelSummary);
                }
            }
            catch (Exception ex)
            {
                _logger.Info("TravelSummaryDataAccess.BuildTravelSummary:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return travelSummary;
        }
    }

    //public class TravelSummaryStub
    //{
    //    public TravelSummary Get()
    //    {
    //        var travelSummary = new List<TravelSummary>();
    //        travelSummary.Add(new TravelSummary
    //        {
    //            BlockedCustomerFlag = true,
    //            BritanniaClubMember = "Silver",
    //            MySagaCustomerFlag = true,
    //            NumberOfNightsOnBoard = 7,
    //            SagaMagazineHolderFlag = true,
    //            SagaMembershipStatus = "Active",
    //            SagaPlatinumCardHolderFlag = true

    //        });
    //        travelSummary.Add(new TravelSummary
    //        {
    //            BlockedCustomerFlag = false,
    //            BritanniaClubMember = "Bronze",
    //            MySagaCustomerFlag = false,
    //            NumberOfNightsOnBoard = 9,
    //            SagaMagazineHolderFlag = false,
    //            SagaMembershipStatus = "Active",
    //            SagaPlatinumCardHolderFlag = true
    //        });
    //        travelSummary.Add(new TravelSummary
    //        {
    //            BlockedCustomerFlag = false,
    //            BritanniaClubMember = "Gold",
    //            MySagaCustomerFlag = true,
    //            NumberOfNightsOnBoard = 12,
    //            SagaMagazineHolderFlag = true,
    //            SagaMembershipStatus = "Active",
    //            SagaPlatinumCardHolderFlag = true
    //        });

    //        Random rnd = new Random();
    //        return travelSummary[rnd.Next(0, 3)];
    //    }
    //}
}
