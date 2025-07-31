namespace Test.Client.Commands.ProductCommands;

internal class DeleteProductCommand : ICommand
{
    public async Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Deleting a product...");
            Console.Write("Enter product id: ");
            var productIdStr = Console.ReadLine();
            if (string.IsNullOrEmpty(productIdStr))
            {
                ICommand.PrintMessage("Product ID cannot be empty. Please try again.");
                return;
            }
            if (!int.TryParse(productIdStr, out int productId))
            {
                ICommand.PrintMessage("Incorrect product ID format.");
                return;
            }
            await httpClient.DeleteProductAsync(productId);
            ICommand.PrintMessage($"Product {productId} deleted successfully!");
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
