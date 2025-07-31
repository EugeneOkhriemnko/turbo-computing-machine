namespace Test.Client.Commands;

internal class ExitCommand : ICommand
{
    public Task ExecuteAsync(IContext context, ServerHttpClient httpClient)
    {
        Console.WriteLine("The app has closed.");
        throw new ExitException();
    }
}
