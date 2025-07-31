using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace Test.Client.Http;

internal class ServerHttpClient(HttpClient client)
{
    private readonly HttpClient _client = client;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task RegisterUserAsync(string email, string password)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { Email = email, Password = password }),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync("register", content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("User registration was successful!");
                return;
            }
            var responseBody = await response.Content.ReadAsStringAsync();

            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.Conflict => new HttpResponseException("A user with this email already exists."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}. Body: {responseBody}"),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<string> LoginUserAsync(string email, string password)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { Email = email, Password = password }),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync("login", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<LoginResponseModel>(responseBody, _options);
                if (result is null)
                    throw new Exception("Failed to deserialize login response.");
                return result.AccessToken;
            }
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}. Body: {responseBody}"),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ProductModel> AddProductAsync(string name)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { Name = name }),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync("api/products", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ProductModel>(responseBody, _options);
                if (result is null)
                    throw new Exception("Failed to deserialize adding a product response.");
                return result;
            }
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}. Body: {responseBody}"),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PriceDetailModel> AddPriceAsync(int productId, decimal price)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { Price = price }),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PostAsync($"api/products/{productId}/prices", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<PriceDetailModel>(responseBody, _options);
                if (result is null)
                    throw new Exception("Failed to deserialize adding a price response.");
                return result;
            }
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}. Body: {responseBody}"),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PriceDetailModel> UpdatePriceAsync(int priceId, decimal newPrice)
    {
        try
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { Price = newPrice }),
                Encoding.UTF8,
                "application/json");
            var response = await _client.PutAsync($"api/products/price/{priceId}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<PriceDetailModel>(responseBody, _options);
                if (result is null)
                    throw new Exception("Failed to deserialize updating a price response.");
                return result;
            }
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.NotFound => new HttpResponseException($"Price with ID {priceId} not found."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}. Body: {responseBody}"),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteProductAsync(int productId)
    {
        try
        {
            var response = await _client.DeleteAsync($"api/products/{productId}");
            if (response.IsSuccessStatusCode)
                return;
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.NotFound => new HttpResponseException($"Product with ID {productId} not found."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}."),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<ProductModel>> SearchProductByNameAsync(string name)
    {
        try
        {
            var response = await _client.GetAsync($"api/products/search/{Uri.EscapeDataString(name)}");
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<IEnumerable<ProductModel>>(responseBody, _options);
                if (result is null)
                    throw new Exception("Failed to deserialize searching products by name response.");
                return result;
            }
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}. Body: {responseBody}"),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<ProductModel>> SearchProductByPriceAsync(decimal min, decimal max)
    {
        try
        {
            var response = await _client.GetAsync($"api/products/search?min={min}&max={max}");
            var responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<IEnumerable<ProductModel>>(responseBody, _options);
                if (result is null)
                    throw new Exception("Failed to deserialize searching products by price response.");
                return result;
            }
            throw response.StatusCode switch
            {
                HttpStatusCode.BadRequest => new HttpResponseException($"Bad request. Check that the data has been entered correctly."),
                HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden => new HttpResponseException("You are not authorized to register."),
                HttpStatusCode.InternalServerError => new HttpResponseException("Server error. Please try again later."),
                _ => new Exception($"Unexpected HTTP status: {(int)response.StatusCode} - {response.StatusCode}. Body: {responseBody}"),
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
