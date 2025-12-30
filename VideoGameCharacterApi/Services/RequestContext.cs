
namespace VideoGameCharacterApi.Services
{
    public class RequestContext : IRequestContext
    {
        public string Method { get; }

        public string Path { get; }

        public DateTime StartTimeUtc { get; }

        public string CorrelationId { get; }

        public bool IsHealthOrPing { get; }

        // Build the context from HttpContext only once per request
        public RequestContext(IHttpContextAccessor accessor)
        {

            var http = accessor.HttpContext ?? throw new InvalidOperationException("No HttpContext.");

            Method = http.Request.Method;
            Path = http.Request.Path.Value ?? string.Empty;
            StartTimeUtc = DateTime.UtcNow;
            CorrelationId = http.TraceIdentifier ?? Guid.NewGuid().ToString("N"); // reuse server id or make one

            // Minimal “skip” rule commonly used in pipelines
            IsHealthOrPing = Path.Contains("test-connection", StringComparison.OrdinalIgnoreCase);

        }

    }
}
