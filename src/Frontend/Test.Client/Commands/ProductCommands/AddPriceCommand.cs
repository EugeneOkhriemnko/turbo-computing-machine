namespace Test.Client.Commands.ProductCommands;

internal class AddPriceCommand : ICommand
{
    public async Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Addining a price...");
            Console.Write("Enter product id: ");
            var productIdStr = Console.ReadLine();
            Console.Write("Enter price: ");
            var priceStr = Console.ReadLine();
            if (string.IsNullOrEmpty(priceStr) || string.IsNullOrEmpty(productIdStr))
            {
                ICommand.PrintMessage("Fields product id and price cannot be empty. Please try again.");
                return;
            }
            if (!int.TryParse(productIdStr, out int productId))
            {
                ICommand.PrintMessage("Incorrect product ID format.");
                return;
            }
            if (!decimal.TryParse(priceStr, out decimal price))
            {
                ICommand.PrintMessage("Incorrect price format.");
                return;
            }
            await httpClient.AddPriceAsync(productId, price);
            ICommand.PrintMessage($"Price {price} added to product {productId} successfully!");
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
