namespace Test.Client.Commands;

internal interface ICommand
{
    Task ExecuteAsync(IContext context, ServerHttpClient httpClient);

    static void PrintMessage(string msg)
    {
        Console.WriteLine(msg);
        Console.ReadKey();
    }
}
