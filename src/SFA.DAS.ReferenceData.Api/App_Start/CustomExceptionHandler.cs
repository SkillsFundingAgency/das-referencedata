using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;


namespace SFA.DAS.ReferenceData.Api
{
    public class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            if (context.Exception is ValidationException)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                var message = ((ValidationException)context.Exception).Message;
                response.Content = new StringContent(message);
                context.Result = new ValidationErrorResult(context.Request, response);
                return;
            }

            base.Handle(context);
        }
    }
}