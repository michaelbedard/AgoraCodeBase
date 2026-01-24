using Agora.Core.Dtos;
using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Payloads.Hubs;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;
using Agora.Engine.Entities.@base;

namespace Agora.Engine;

public abstract partial class BaseGame : IGame
{
    // --- State ---
    protected List<Player> Players { get; private set; } = new();
    private readonly Dictionary<Player, List<IGameAction>> _allowedActionsByPlayer = new();
    private readonly Dictionary<Player, IGameInput> _inputActionByPlayers = new();
    private readonly Dictionary<Player, List<CommandDto>> _animationsByPlayer = new();
    private readonly List<string> _playersTakingTurn = new();
    public int ActionIdCount = 0;
    
    // The "Waiter" stops the code until an external event (PerformAction) triggers it
    private readonly Dictionary<Player, TaskCompletionSource<bool>> _inputWaiters = new();
    private TaskCompletionSource<BaseGameAction> _actionWaiter;
    
    // --- Abstract Fields and Methods ---
    protected abstract string Title { get; }
    protected abstract string RulesAddress { get; }
    protected abstract string MusicAddress { get; }
    protected abstract string EnvironmentAddress { get; }
    protected abstract List<IGameModule> Setup();
    protected abstract Dictionary<string, string> GetDescriptions(Player player);
    protected abstract Task GameLogicAsync();
    
    // ------------------------------------------------------------
    // Initialization & Loop
    // ------------------------------------------------------------
    public LoadGamePayload LoadGame(List<UserDto> players)
    {
        Players = players.Select(u => new Player(u)).ToList();
        ResetAllowedActions();
        ResetAnimations();

        List<IGameModule> serverModules = Setup();
        List<GameModuleDto> clientModules = AssignIdsAndConvert(serverModules);
        
        return new LoadGamePayload()
        {
            Title = Title,
            RulesAddress = RulesAddress,
            MusicAddress = MusicAddress,
            EnvironmentAddress = EnvironmentAddress,
            GameModules = clientModules,
            Players = players,
        };
    }

    public void StartGame()
    {
        _ = RunGameLoop();
    }

    private async Task RunGameLoop()
    {
        try 
        {
            await GameLogicAsync();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n==========================================");
            Console.WriteLine("!!! CRITICAL GAME LOGIC FAILURE !!!");
            Console.WriteLine("==========================================");
        
            PrintExceptionDetails(ex);

            Console.WriteLine("==========================================");
            Console.ResetColor();
        }
    }
    
    private List<GameModuleDto> AssignIdsAndConvert(IEnumerable<IGameModule> modules)
    {
        var nameCounts = new Dictionary<string, int>();

        void AssignIdRecursive(IGameModule module)
        {
            if (string.IsNullOrEmpty(module.Name) || !string.IsNullOrEmpty(module.Id)) 
                throw new ArgumentException($"Module is missing Name or has predefined ID: {module.Name}");

            nameCounts.TryAdd(module.Name, 0);

            int count = nameCounts[module.Name];
            module.Id = $"{module.Name}__{count}";
            nameCounts[module.Name]++;

            // Recurse: Check if this module has children
            if (module is Deck deck)
            {
                foreach (var card in deck.Cards)
                {
                    AssignIdRecursive(card);
                }
            }
            else if (module is ZoneDto zone)
            {
            }
        }

        var result = new List<GameModuleDto>();
        foreach (var module in modules)
        {
            AssignIdRecursive(module);
            result.Add(module.ToDto());
        }
        
        return result;
    }
}