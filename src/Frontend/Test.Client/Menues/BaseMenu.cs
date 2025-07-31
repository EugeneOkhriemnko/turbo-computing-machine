using Test.Client.Commands;

namespace Test.Client.Menues;

internal abstract class BaseMenu(IContext context)
{
    protected IContext _context = context;
    protected Dictionary<int, ICommand> _commands = [];

    public abstract Task ShowMenuAsync();

    protected int? CheckCommandNumber(string? str)
    {
        if (!string.IsNullOrEmpty(str) && int.TryParse(str, out int commandNumber))
        {
            return commandNumber > 0 && commandNumber <= _commands.Count
                ? commandNumber
                : null;
        }
        return null;
    }
}
