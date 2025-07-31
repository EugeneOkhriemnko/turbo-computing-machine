namespace Test.Client.Commands.ProductCommands;

internal class SearchProductByNameCommand : ICommand
{
    public async Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Searching for a product by name...");
            Console.Write("Enter product name: ");
            var name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                ICommand.PrintMessage("Product name cannot be empty. Please try again.");
                return;
            }
            var products = (await httpClient.SearchProductByNameAsync(name)).ToList();
            if (products.Count == 0)
            {
                ICommand.PrintMessage($"No products found with name '{name}'.");
                return;
            }
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");
                foreach (var price in product.PriceDetails)
                {
                    Console.WriteLine($"  Price ID: {price.Id}, Price: {price.Price}, Created date: {price.CreatedDate}");
                }
            }
            Console.ReadKey();
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
