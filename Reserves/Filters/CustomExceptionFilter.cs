using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Reserves.Domain.Exceptions;
using Reserves.Models;
using Serilog;
using System.Net;

namespace Reserves.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            _ = new ErrorResponse();

            ErrorResponse? errorResponse;
            if (context.Exception is DataValidationException customEx)
            {
                errorResponse = new ErrorResponse
                {
                    Message = "Se ha producido un error al procesar su solicitud.",
                    Details = customEx.Message
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.Conflict
                };

                Log.Error($"Se produjo un error de validación personalizada: {customEx.Message}, ErrorCode: {customEx.ErrorCode}");
            }

            else if (context.Exception is ArgumentException invalidOpEx)
            {
                errorResponse = new ErrorResponse
                {
                    Message = "Operación no válida.",
                    Details = invalidOpEx.Message
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };


                Log.Error($"Se produjo una operación no válida: {invalidOpEx.Message}");
            }
            else
            {
                Log.Error(context.Exception, "Se ha producido una excepción no controlada.");

                errorResponse = new ErrorResponse
                {
                    Message = "Se ha producido un error inesperado.",
                    Details = context.Exception.Message
                };

                context.Result = new JsonResult(errorResponse)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
        }
    }
}
