namespace Test.Client.Commands.ProductCommands;

internal class SearchProductByPriceCommand : ICommand
{
    public async Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Searching for products by price...");
            Console.Write("Enter min price: ");
            var minPriceStr = Console.ReadLine();
            Console.Write("Enter max price: ");
            var maxPriceStr = Console.ReadLine();
            if (string.IsNullOrEmpty(minPriceStr) || string.IsNullOrEmpty(maxPriceStr))
            {
                ICommand.PrintMessage("Min and max prices cannot be empty. Please try again.");
                return;
            }
            if (!decimal.TryParse(minPriceStr, out decimal minPrice))
            {
                ICommand.PrintMessage("Incorrect min price format.");
                return;
            }
            if (!decimal.TryParse(maxPriceStr, out decimal maxPrice))
            {
                ICommand.PrintMessage("Incorrect max price format.");
                return;
            }
            var products = (await httpClient.SearchProductByPriceAsync(minPrice, maxPrice)).ToList();
            if (products.Count == 0)
            {
                ICommand.PrintMessage($"No products found with price between '{minPrice}' - '{maxPrice}'.");
                return;
            }
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}");
                foreach (var priceDetail in product.PriceDetails)
                {
                    Console.WriteLine($"  Price ID: {priceDetail.Id}, Price: {priceDetail.Price}, Created date: {priceDetail.CreatedDate}");
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
