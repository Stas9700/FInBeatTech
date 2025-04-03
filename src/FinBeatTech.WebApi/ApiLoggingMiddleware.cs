using FinBeatTech.Database;
using Microsoft.EntityFrameworkCore;

namespace FinBeatTech.WebApi;

public class ApiLoggingMiddleware(RequestDelegate next, IDbContextFactory<FinBeatDbContext> contextFactory)
{
    public async Task Invoke(HttpContext context)
    {
        // Чтение тела запроса
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        // Сохранение оригинального потока ответа
        var originalBodyStream = context.Response.Body;
            
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await next(context);

            // Чтение тела ответа
            responseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
            responseBody.Seek(0, SeekOrigin.Begin);

            // Логирование
            var log = new ApiLog
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                Query = context.Request.QueryString.ToString(),
                RequestBody = requestBody,
                ResponseBody = responseBodyText,
                Timestamp = DateTime.UtcNow,
                StatusCode = context.Response.StatusCode
            };

            using (var dbContext = contextFactory.CreateDbContext())
            {
                dbContext.ApiLogs.Add(log);
                await dbContext.SaveChangesAsync();
            }

            // Возвращаем ответ клиенту
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}