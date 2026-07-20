using System.Text.Json;

namespace SallaJo.MiddleWares
{
    public class GlobalExeptionMidlleWare
    {
        private readonly RequestDelegate _next;

        public GlobalExeptionMidlleWare(RequestDelegate next)
        {
            _next = next;   
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    message = "Server Error, Please Try Again"
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
