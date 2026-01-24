using Agora.Core.Dtos.Game.Commands;
using Agora.Engine.Commands._base;

namespace Agora.Engine;

public abstract partial class BaseGame
{
    private void PrintExceptionDetails(Exception ex, int indent = 0)
    {
        string prefix = new string(' ', indent * 4);
    
        Console.WriteLine($"{prefix}Type: {ex.GetType().FullName}");
        Console.WriteLine($"{prefix}Message: {ex.Message}");
        Console.WriteLine($"{prefix}Source: {ex.Source}");
    
        // Only print StackTrace for the top-level or most relevant exception to avoid clutter
        if (indent == 0 || ex.StackTrace != null) 
        {
            Console.WriteLine($"{prefix}Stack Trace:");
            Console.WriteLine(ex.StackTrace);
        }

        if (ex.InnerException != null)
        {
            Console.WriteLine($"\n{prefix}--- Caused By ---");
            PrintExceptionDetails(ex.InnerException, indent + 1);
        }

        // Handle AggregateExceptions (common in Task.WhenAll)
        if (ex is AggregateException aggEx)
        {
            foreach (var inner in aggEx.InnerExceptions)
            {
                if (inner != ex.InnerException) // Avoid duplicates
                {
                    Console.WriteLine($"\n{prefix}--- Also Caused By ---");
                    PrintExceptionDetails(inner, indent + 1);
                }
            }
        }
    }

    private void ResetAllowedActions()
    {
        _allowedActionsByPlayer.Clear(); 
        foreach (var p in Players) 
        {
            _allowedActionsByPlayer[p] = new List<IGameAction>(); 
        }
    }
    
    private void ResetAnimations()
    {
        _animationsByPlayer.Clear(); 
        foreach (var p in Players) 
        {
            _animationsByPlayer[p] = new List<CommandDto>(); 
        }
    }
}