using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Middleware
{
    public class RequestLogMiddleware  // middleware bir ara katmandır.
    {                                  //Loglama, doğrulama, yetki kontrolü, cache, exception handling gibi birçok ön işleme/son işlem burada yapılır.
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context) //bu metod her istek geldiğinde çalışır
        {
            Console.WriteLine($"İstek: {context.Request.Method} - {context.Request.Path}");
            await _next(context);
        }
    }     //Her gelen HTTP isteğini konsola yazıyor:
}
