namespace Test.Client.Commands.ProductCommands;

internal class AddProductCommand : ICommand
{
    public async Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Creating a product...");
            Console.Write("Enter name: ");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                ICommand.PrintMessage("Name of a product cannot be empty. Please try again.");
                return;
            }
            var product = await httpClient.AddProductAsync(name);
            ICommand.PrintMessage($"Product created successfully! ID: {product.Id}, Name: {product.Name}");
        }
        catch (HttpResponseException ex)
        {
            ICommand.PrintMessage($"Error: {ex.Message}");
            throw;
        }
        catch (Exception)
        {
            ICommand.PrintMessage("There's been a mistake. Please try again later.");
            throw;
        }
    }
}
