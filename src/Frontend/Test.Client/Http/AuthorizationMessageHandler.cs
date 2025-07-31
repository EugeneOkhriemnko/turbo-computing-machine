namespace Test.Client.Http;

internal class AuthorizationMessageHandler(IContext context) : DelegatingHandler
{
    private readonly IContext _context = context;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = _context.GetAccessToken();
        if (!string.IsNullOrEmpty(token))
        {
            var bearerToken = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
        }
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
