namespace VideoGameCharacterApi.Middleware
{
    public class HelloMiddleware
    {
        private readonly RequestDelegate _next;

        public HelloMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine();
            Console.WriteLine($"{nameof(HelloMiddleware)} invoked!");
            Console.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Hello from middleware!");
            await _next(context);
        }
    }

}
