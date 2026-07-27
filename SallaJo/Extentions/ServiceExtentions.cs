using System.Threading.RateLimiting;

namespace SallaJo.Extentions
{
    public static class ServiceExtentions
    {
        public static void SetRateLimiters(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("fixed-5-per-15min-ip", httpContext =>
                {
                    string key = "";
                    var endpointName = httpContext.GetEndpoint()?.DisplayName ?? "unknown";

                    if (HttpMethods.IsOptions(httpContext.Request.Method))
                    {
                        return RateLimitPartition.GetNoLimiter("preflight");
                    }

                    if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
                    {
                        key = forwardedFor.ToString().Split(',')[0].Trim() + $":{endpointName}";
                    }
                    else
                    {
                        key = (httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous") + $":{endpointName}";
                    }

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: key,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(15),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });

                });

                options.AddPolicy("fixed-5-per-12h-ip", httpContext =>
                {
                    string key = "";
                    var endpointName = httpContext.GetEndpoint()?.DisplayName ?? "unknown";

                    if (HttpMethods.IsOptions(httpContext.Request.Method))
                    {
                        return RateLimitPartition.GetNoLimiter("preflight");
                    }

                    if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
                    {
                        key = forwardedFor.ToString().Split(',')[0].Trim() + $":{endpointName}";
                    }
                    else
                    {
                        key = (httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous") + $":{endpointName}";
                    }

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: key,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromHours(12),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });

                });

                options.AddPolicy("fixed-150-per-1h-ip", httpContext =>
                {
                    string key = "";
                    var endpointName = httpContext.GetEndpoint()?.DisplayName ?? "unknown";

                    if (HttpMethods.IsOptions(httpContext.Request.Method))
                    {
                        return RateLimitPartition.GetNoLimiter("preflight");
                    }

                    if (httpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
                    {
                        key = forwardedFor.ToString().Split(',')[0].Trim() + $":{endpointName}";
                    }
                    else
                    {
                        key = (httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous") + $":{endpointName}";
                    }

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: key,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 150,
                            Window = TimeSpan.FromHours(1),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });

                });
            });
        }
    }
}
