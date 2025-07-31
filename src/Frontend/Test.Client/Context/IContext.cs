namespace Test.Client.Context;

internal interface IContext
{
    void SetAccessToken(string accessToken);
    string GetAccessToken();
}
