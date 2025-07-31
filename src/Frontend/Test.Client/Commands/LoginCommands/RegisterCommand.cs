namespace Test.Client.Commands.LoginCommands;

internal class RegisterCommand : ICommand
{
    public async Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Registering a new user...");
            Console.Write("Enter email: ");
            var email = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ICommand.PrintMessage("Email and password cannot be empty. Please try again.");
                return;
            }
            await httpClient.RegisterUserAsync(email, password);
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
