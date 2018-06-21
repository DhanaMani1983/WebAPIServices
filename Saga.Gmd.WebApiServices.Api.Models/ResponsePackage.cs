using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace Saga.Gmd.WebApiServices.Api.Models
{
    public class ResponsePackage
    {
        /// <summary>
        /// Lis of Errors
        /// </summary>
        /// 
        [DataMember]
        public List<string> Errors { get; set; }
        /// <summary>
        /// Http Status Code
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int HttpStatusCode { get; set; }
        /// <summary>
        /// Result set
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public object Data { get; set; }

        /// <summary>
        /// Is Request succeded
        /// </summary>
        [DataMember]
        public bool Success { get; set; }

        /// <summary>
        /// Actual Http Status HttpMessage
        /// </summary>
        [DataMember]
        public string HttpMessage { get; set; }

        /// <summary>
        /// Helpful information if avaiable
        /// </summary>
        [DataMember]
        public string Info { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data">This field contains the json data </param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        /// <param name="info"></param>
        /// <param name="errors">This field contains list of errors</param>
        public ResponsePackage(object data, HttpStatusCode statusCode, string message, string info, List<string> errors)
        {
            Errors = errors;
            HttpStatusCode = (int)statusCode;
            HttpMessage = message;
            Data = data;
            Success = HttpStatusCode >= 200 && HttpStatusCode <= 299;
            Info = info;
        }
    }
}