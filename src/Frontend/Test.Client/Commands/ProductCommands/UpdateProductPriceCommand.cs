namespace Test.Client.Commands.ProductCommands;

class UpdateProductPriceCommand : ICommand
{
    public async Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Updating a price...");
            Console.Write("Enter price id: ");
            var priceIdStr = Console.ReadLine();
            Console.Write("Enter a new price: ");
            var newPriceStr = Console.ReadLine();
            if (string.IsNullOrEmpty(newPriceStr) || string.IsNullOrEmpty(priceIdStr))
            {
                ICommand.PrintMessage("Fields price id and price cannot be empty. Please try again.");
                return;
            }
            if (!int.TryParse(priceIdStr, out int priceId))
            {
                ICommand.PrintMessage("Incorrect price ID format.");
                return;
            }
            if (!decimal.TryParse(newPriceStr, out decimal newPrice))
            {
                ICommand.PrintMessage("Incorrect new price format.");
                return;
            }
            await httpClient.UpdatePriceAsync(priceId, newPrice);
            ICommand.PrintMessage($"Price {priceId} updated to new price {newPrice} successfully!");
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
