using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace projectX.CustomFilter
{
    //  
    public class MyExcptionHandler:ActionFilterAttribute
    {
        public override void OnActionExecuted (ActionExecutedContext context)
        {
            //this method will be executed after the action method is executed
            if (context.Exception != null) 
            { 
                context.ExceptionHandled = true;
                context.Result = new ContentResult () { Content = "exception handled , message: " + context.Exception.Message, ContentType = "text/plain" };
                // can return view result 
                //context.Result = new ViewResult () { ViewName = "Error" };
            }
            base.OnActionExecuted (context);
        }
    }
}
