using Test.Client.Menues;

namespace Test.Client;

internal class Application(IContext context, LoginMenu loginMenu, ProductMenu commandMenu)
{
    private readonly IContext _context = context;
    private readonly LoginMenu _loginMenu = loginMenu;
    private readonly ProductMenu _commandMenu = commandMenu;

    public async Task RunAsync()
    {
        while (true)
        {
            BaseMenu currentMenu = string.IsNullOrEmpty(_context.GetAccessToken())
                ? _loginMenu
                : _commandMenu;
            try
            {
                await currentMenu.ShowMenuAsync();
            }
            catch (ExitException)
            {
                return;
            }
        }
    }
}
