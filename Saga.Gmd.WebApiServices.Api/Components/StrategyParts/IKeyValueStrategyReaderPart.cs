using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{
    public interface IKeyValueStrategyReaderPart
    {
        object Process(KeyValueParameter keyValueParameter);
    }

}
