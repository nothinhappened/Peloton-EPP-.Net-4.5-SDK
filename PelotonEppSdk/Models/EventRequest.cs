﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using PelotonEppSdk.Classes;
using PelotonEppSdk.Enums;

namespace PelotonEppSdk.Models
{
    public class EventRequest : RequestBase
    {
        [StringLength(32, MinimumLength = 32, ErrorMessage = nameof(EventToken) + " must be 32 characters long.")]
        public string EventToken { get; set; }

        /// <summary>
        /// ClientToken for the target peloton account
        /// </summary>
        //[Required]
        [StringLength(32, MinimumLength = 32, ErrorMessage = "AccountToken must be 32 characters long.")]
        public string AccountToken { get; set; }

        //[StringLength(32, MinimumLength = 32, ErrorMessage = "Name must be 32 characters long.")]
        public string Name { get; set; }

        public string FriendlyUrlPath { get; set; }

        public string Description { get; set; }

        //[Required]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public State State { get; set; }

        public ICollection<EventItem> Items { get; set; }

        public string TermsAndConditionsContent { get; set; }

        public string RefundPolicyContent { get; set; }

        /// <exception cref="HttpException"><see cref="HttpStatusCode"/> is not <c>2XX Success</c>.</exception>
        public async Task<EventResponse> GetAsync()
        {
            var client = new PelotonClient();
            var parameter = "?token=" + EventToken;
            parameter += "&languageCode=" + LanguageCode;
            var result = await client.GetAsync<event_response>((event_request) this, ApiTarget.Events, parameter).ConfigureAwait(false);
            return (EventResponse)result;
        }

        /// <exception cref="HttpException"><see cref="HttpStatusCode"/> is not <c>2XX Success</c>.</exception>
        public async Task<EventResponse> PostAsync()
        {
            var client = new PelotonClient();
            var request = (event_request)this;
            var result = await client.PostAsync<event_response>(request, ApiTarget.Events, null).ConfigureAwait(false);
            return (EventResponse)result;
        }

        /// <exception cref="HttpException"><see cref="HttpStatusCode"/> is not <c>2XX Success</c>.</exception>
        public async Task<EventResponse> PutAsync()
        {
            var client = new PelotonClient();
            var parameter = "?token=" + EventToken;
            var request = (event_request)this;
            var result = await client.PutAsync<event_response>(request, ApiTarget.Events, parameter).ConfigureAwait(false);
            return (EventResponse)result;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
        internal class event_request : request_base
        {
            [StringLength(32, MinimumLength = 32)]
            public string event_token { get; set; }

            [StringLength(32, MinimumLength = 32)]
            public string account_token { get; set; }

            public string name { get; set; }

            public string friendly_url_path { get; set; }

            public string description { get; set; }

            public DateTime? start_date { get; set; }

            public DateTime? end_date { get; set; }

            public state state { get; set; }

            public ICollection<EventItem.event_item> items { get; set; }

            public string terms_and_conditions_content { get; set; }

            public string refund_policy_content { get; set; }

            public event_request(RequestBase requestBase) : base(requestBase) { }

            public static explicit operator event_request(EventRequest eventsRequest)
            {
                return new event_request(eventsRequest)
                {
                    event_token = eventsRequest.EventToken,
                    account_token = eventsRequest.AccountToken,
                    application_name = eventsRequest.ApplicationName,
                    authentication_header = eventsRequest.AuthenticationHeader,
                    name = eventsRequest.Name,
                    description = eventsRequest.Description,
                    friendly_url_path = eventsRequest.FriendlyUrlPath,
                    start_date = eventsRequest.StartDate,
                    end_date = eventsRequest.EndDate,
                    state = (state)eventsRequest.State,
                    items = eventsRequest.Items?.Select(ei => (EventItem.event_item)ei).ToList(),
                    terms_and_conditions_content = eventsRequest.TermsAndConditionsContent,
                    refund_policy_content = eventsRequest.RefundPolicyContent,
                    language_code = Enum.GetName(typeof(LanguageCode), eventsRequest.LanguageCode)
                };
            }
        }
    }
}
