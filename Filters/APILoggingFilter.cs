using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters;

public class APILoggingFilter : IActionFilter
{
    private readonly ILogger<APILoggingFilter> _logger;

    public APILoggingFilter(ILogger<APILoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        //executa antes da Action
        _logger.LogInformation("### Executando -> OnActionExecuting");
        _logger.LogInformation("###################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        //executa depois da Action
        _logger.LogInformation("### Executando -> OnActionExecuted");
        _logger.LogInformation("###################################################");
        _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
        _logger.LogInformation($"Status Code : {context.HttpContext.Response.StatusCode}");
        _logger.LogInformation("###################################################");
    }    
}