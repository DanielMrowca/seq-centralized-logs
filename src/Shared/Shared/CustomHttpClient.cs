using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Prodigy.HTTP;
using Prodigy.Models;

namespace Shared
{
    public class CustomHttpClient : ProdigyHttpClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomHttpClient(
            HttpClient httpClient,
            HttpClientOptions options,
            IEnumerable<JsonConverter> jsonConverters,
            AppSettings appSettings, IHttpContextAccessor httpContextAccessor) : base(httpClient, options, jsonConverters, appSettings)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task BeforeSendRequestAsync(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add(Extensions.CorrelationHeaderKey, GetCorrelationId());
            return base.BeforeSendRequestAsync(httpClient);
        }

        private string GetCorrelationId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
                return string.Empty;

            httpContext.Request.Headers.TryGetValue(Extensions.CorrelationHeaderKey, out var output);
            var correlationId = output.ToString();
            if (!string.IsNullOrWhiteSpace(correlationId)) return correlationId;

            httpContext.Response.Headers.TryGetValue(Extensions.CorrelationHeaderKey, out var output2);
            correlationId = output2.ToString();
            return correlationId;
        }
    }
}
