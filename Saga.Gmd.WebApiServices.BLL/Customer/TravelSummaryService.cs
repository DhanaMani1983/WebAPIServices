using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Models;


namespace Saga.Gmd.WebApiServices.BLL.Customer
{
    public interface ITravelSummaryService
    {
        Models.Customer.TravelSummary GetTravelSummary(int numeroId, NameAndAddress nameAndAddress = null);
    }

    public class TravelSummaryService : ITravelSummaryService
    {
        private readonly ITravelSummaryDataAccess _travelSummaryDataAccess;
        public TravelSummaryService(ITravelSummaryDataAccess travelSummaryDataAccess)
        {
            _travelSummaryDataAccess = travelSummaryDataAccess;
        }
        public Models.Customer.TravelSummary GetTravelSummary(int numeroId, NameAndAddress nameAndAddress = null)
        {
            return _travelSummaryDataAccess.GetTravelSummary(numeroId, nameAndAddress);
        }
    }
}
