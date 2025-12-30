using VideoGameCharacterApi.Services;

namespace VideoGameCharacterApi.Middleware
{

    public class RequestContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestContextMiddleware(RequestDelegate next) => _next = next;

        // DI will supply IRequestContext per request
        public async Task InvokeAsync(HttpContext context, IRequestContext requestContext)
        {
            // Add correlation id to response so clients can echo it in logs / bug reports
            context.Response.Headers["X-Correlation-Id"] = requestContext.CorrelationId;

            // Keep it minimal but useful: one line log per request
            Console.WriteLine(
                $"{DateTime.UtcNow:HH:mm:ss.fff} [{requestContext.CorrelationId}] {requestContext.Method} {requestContext.Path}");

            // Optional: skip heavy work for health checks (we still continue the pipeline)
            if (requestContext.IsHealthOrPing) 
            { /* no-op */
                Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Received Ping or Health Check Request");
            }

            // also print out other fields of the request context for demonstration purposes
            Console.WriteLine($"Request Start Time (UTC): {requestContext.StartTimeUtc:yyyy-MM-dd HH:mm:ss.fff}");
            Console.WriteLine($"Request Method: {requestContext.Method}");
            Console.WriteLine($"Request Path: {requestContext.Path}");
            Console.WriteLine($"Is Health or Ping Request: {requestContext.IsHealthOrPing}");

            await _next(context); // continue to next middleware / MVC
        }
    }
}
