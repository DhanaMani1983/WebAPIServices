using System;

namespace Saga.Gmd.WebApiServices.Api.Components.StrategyParts
{

    public enum StrategyWriterPartImplementations
    {
        UpdateMembershipWriterPart, UpdateMembershipWriterPartV2, CancellMembershipWriterPart, ProductSoldMembershipWriterPart
    }


    /// <summary>
    /// Interface to expose strategy writer parts 
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="T3"></typeparam>
    /// <typeparam name="TR1"></typeparam>
    public interface IStrategyWriterPart<T1, T2, T3, TR1> 
    {
        TR1 Process(T1 param1);
        TR1 Process(T1 param1, T2 param2);

        TR1 Process(T1 param1, T2 param2, T3 param3);

    }

}
