namespace BloggingWebApplication.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.Value != null && path.Value.StartsWith("Home/Dashboard"))
            {
                var user = context.Session.GetString("User");
                if (string.IsNullOrEmpty(user))
                {
                    context.Response.Redirect("/Home/Login");
                    await _next(context);
                }
                context.Response.Redirect("/Home/Dashboard");
                await _next(context);
            }
            await _next(context);
        }
    }
}
