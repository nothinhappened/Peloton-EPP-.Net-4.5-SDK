﻿using System.Collections.Generic;
using PelotonEppSdk.RequestsAndResponses;

namespace PelotonEppSdk.Models
{
    /// <summary>
    /// General Response
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Common response for API transactions
        /// </summary>
        public Response()
        {
        }

        /// <summary>
        /// True if an Successful, False otherwise.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// This field will return an Message regarding the operation in language specified in the request.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The code associated with this message.
        /// </summary>
        public int MessageCode { get; set; }

        /// <summary>
        /// This field will return a value for any transaction that occurs, otherwise will be empty.
        /// </summary>
        public string TransactionRefCode { get; set; }

        /// <summary>
        /// This field will return any validation errors that occured
        /// </summary>
        public ICollection<string> Errors { get; set; }

        public static explicit operator Response(response r)
        {
            return new Response
            {
                Success = r.success,
                Message = r.message,
                Errors = r.errors,
                MessageCode = r.message_code,
                TransactionRefCode = r.transaction_ref_code
            };
        }

        public static explicit operator response(Response r)
        {
            return new response
            {
                errors = r.Errors,
                message = r.Message,
                message_code = r.MessageCode,
                success = r.Success,
                transaction_ref_code = r.TransactionRefCode
            };
        }
    }
}