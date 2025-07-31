using Microsoft.Extensions.Logging;
using Test.Client.Commands;

namespace Test.Client.Menues;

internal class ProductMenu : BaseMenu
{
    private readonly ServerHttpClient _httpClient;
    private readonly ILogger<ProductMenu> _logger;

    public ProductMenu(IContext context, ServerHttpClient httpClient, ILogger<ProductMenu> logger)
        : base(context)
    {
        _httpClient = httpClient;
        _logger = logger;
        _commands.Add(1, new AddProductCommand());
        _commands.Add(2, new AddPriceCommand());
        _commands.Add(3, new UpdateProductPriceCommand());
        _commands.Add(4, new DeleteProductCommand());
        _commands.Add(5, new SearchProductByNameCommand());
        _commands.Add(6, new SearchProductByPriceCommand());
        _commands.Add(7, new ExitCommand());
    }

    public async override Task ShowMenuAsync()
    {
        Console.Clear();
        Console.WriteLine("1. Add a new product.");
        Console.WriteLine("2. Add a price for a product.");
        Console.WriteLine("3. Update a product price.");
        Console.WriteLine("4. Delete a product.");
        Console.WriteLine("5. Search products by name.");
        Console.WriteLine("6. Search products by price.");
        Console.WriteLine("7. Exit the application..");
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
