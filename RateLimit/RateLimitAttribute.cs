using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RateLimit
{
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private int counter;
        private ILogger _logger;

        public int MaxRequests { get; set; }

        public RateLimitAttribute(ILogger<RateLimitAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            counter++;
            if (counter > MaxRequests)
            {
                counter--;
                context.Result = new StatusCodeResult(429);
                return;
            }
            _logger.LogCritical(counter.ToString());
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            counter--;
        }
    }

    public class RateLimitAttributeFactory : Attribute, IFilterFactory
    {
        private int _maxRequests;

        public bool IsReusable => true;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<RateLimitAttribute>();
            filter.MaxRequests = _maxRequests;
            return filter;
        }

        public RateLimitAttributeFactory(int maxRequests)
        {
            _maxRequests = maxRequests;
        }
    }
}
