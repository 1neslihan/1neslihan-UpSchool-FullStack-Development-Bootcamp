﻿using Application.Common.Models.Errors;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace WebApi.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger=logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            
            ApiErrorDto apiErrorDto = new ApiErrorDto();
            switch(context.Exception)
            {
                case ValidationException:
                    var validationException=context.Exception as ValidationException;

                    var propertyNames = validationException.Errors
                        .Select(x => x.PropertyName)
                        .Distinct();
                    //["email","username","password"]
                    foreach(var propertyName in propertyNames)
                    {
                        var propertyFailures = validationException.Errors
                            .Where(e => e.PropertyName== propertyName)
                            .Select(x => x.ErrorMessage)
                            .ToList();

                        //password is required,
                        //password  must have atleast 5 characters
                        //password must have at least 1 special character

                        apiErrorDto.Errors.Add(new ErrorDto(propertyName, propertyFailures));
                    }

                    apiErrorDto.Message="One or more validation errors were occurred.";
                    //context.HttpContext.Response.StatusCode =(int)StatusCodes.Status400BadRequest;
                    context.Result=new BadRequestObjectResult(apiErrorDto);
                    break;


                default:
                    _logger.LogError(context.Exception, context.Exception.Message);
                    apiErrorDto.Message="An unexpected error was occurred.";
                    //context.HttpContext.Response.StatusCode=(int)StatusCodes.Status500InternalServerError;
                    context.Result=new ObjectResult(apiErrorDto)
                    {
                        StatusCode=(int)StatusCodes.Status500InternalServerError
                    };
                    break;
                    
            }
            //context.HttpContext.Response.ContentType="application/json";
            //var apiErrorDtoJson = JsonSerializer.Serialize(apiErrorDto);
            //await context.HttpContext.Response.WriteAsync(apiErrorDtoJson);
            return Task.CompletedTask;
        }
    }
}
