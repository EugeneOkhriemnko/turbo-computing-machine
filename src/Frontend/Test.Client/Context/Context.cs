namespace Test.Client.Context;

internal class Context : IContext
{
    private static string _accessToken = string.Empty;
    public void SetAccessToken(string accessToken)
    {
        _accessToken = accessToken;
    }

    public string GetAccessToken()
    {
        return _accessToken;
    }
}