using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ValidationException:
                    return Task.CompletedTask;

                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
