using Microsoft.Extensions.Logging;
using Test.Client.Commands;
using Test.Client.Commands.LoginCommands;

namespace Test.Client.Menues;

internal class LoginMenu : BaseMenu
{
    private readonly ServerHttpClient _httpClient;
    private readonly ILogger<LoginMenu> _logger;

    public LoginMenu(IContext context, ServerHttpClient httpClient, ILogger<LoginMenu> logger) : base(context)
    {
        _httpClient = httpClient;
        _logger = logger;
        _commands.Add(1, new RegisterCommand());
        _commands.Add(2, new LoginCommand());
        _commands.Add(3, new ExitCommand());
    }
    public async override Task ShowMenuAsync()
    {
        Console.Clear();
        Console.WriteLine("1. Register new user.");
        Console.WriteLine("2. Login.");
        Console.WriteLine("3. Exit the application.");
        Console.Write("Choose number of command: ");
        var commandStr = Console.ReadLine();
        var commandNumber = CheckCommandNumber(commandStr);
        if (commandNumber is null)
        {
            Console.WriteLine("Invalid command number. Please try again.");
            Console.ReadKey();
            return;
        }
        try
        {
            await _commands[commandNumber.Value].ExecuteAsync(_context, _httpClient);
        }
        catch (ExitException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while executing the command.");
        }
    }
}
