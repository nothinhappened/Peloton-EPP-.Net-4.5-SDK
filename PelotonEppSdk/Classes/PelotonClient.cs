﻿using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using PelotonEppSdk.Enums;
using PelotonEppSdk.Models;

namespace PelotonEppSdk.Classes
{
    internal enum RequestType
    {
        Get,
        Post,
        Put,
        Delete
    }

    internal class PelotonClient
    {
        /// <exception cref="HttpException">When status code is not <c>2XX Success</c>.</exception>
        /// <exception cref="NotImplementedException">When RequestType is not yet supported.</exception>
        private async Task<T> MakeBasicHttpRequest<T>(RequestType type, request_base content, ApiTarget target, string parameter)
        {
            var factory = new UriFactory();
            var serializer = new JavaScriptSerializer();
            var serializedContent = serializer.Serialize(content);
            var stringContent = new StringContent(serializedContent, Encoding.Default, "application/json");
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = content.authentication_header;
                client.BaseAddress = content.base_uri;
                var targetUriPart = factory.GetTargetUriPart(target);
                var targetPath = targetUriPart + parameter;

                HttpResponseMessage httpResponseMessage = null;
                switch (type)
                {
                    case RequestType.Get:
                        throw new NotImplementedException();
                    case RequestType.Post:
                        httpResponseMessage = await client.PostAsync(targetPath, stringContent).ConfigureAwait(false);
                        break;
                    case RequestType.Put:
                        httpResponseMessage = await client.PutAsync(targetPath, stringContent).ConfigureAwait(false);
                        break;
                    case RequestType.Delete:
                        httpResponseMessage = await client.DeleteAsync(targetPath).ConfigureAwait(false);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                string stringResult = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrEmpty(stringResult))
                {
                    return serializer.Deserialize<T>(stringResult);
                }

                // handle server errors
                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new HttpException((int)httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                    case HttpStatusCode.Unauthorized:
                        throw new HttpException((int)httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                    default:
                        throw new HttpException((int)httpResponseMessage.StatusCode, httpResponseMessage.ReasonPhrase);
                }
            }
        }

        /// <exception cref="HttpException">When status code is not <c>2XX Success</c>.</exception>
        public async Task<T> PostAsync<T>(request_base content, ApiTarget target, string parameter = null)
        {
            return await MakeBasicHttpRequest<T>(RequestType.Post, content, target, parameter).ConfigureAwait(false);
        }

        /// <exception cref="HttpException">When status code is not <c>2XX Success</c>.</exception>
        public async Task<T> PutAsync<T>(request_base content, ApiTarget target, string parameter = null)
        {
            return await MakeBasicHttpRequest<T>(RequestType.Put, content, target, parameter).ConfigureAwait(false);
        }

        /// <exception cref="HttpException">When status code is not <c>2XX Success</c>.</exception>
        public async Task<T> DeleteAsync<T>(request_base content, ApiTarget target, string parameter = null)
        {
            return await MakeBasicHttpRequest<T>(RequestType.Delete, content, target, parameter).ConfigureAwait(false);
        }
    }
}
