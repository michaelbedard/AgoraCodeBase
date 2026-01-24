using Agora.Core.Dtos;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using Agora.Engine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var gameKey = GameKey.Uno;
var showDetails = true;

var players = new List<UserDto>
{
    new UserDto { Id = "p1", Username = "Alice" },
    new UserDto { Id = "p2", Username = "Bob" }
};



// A map to remember who owns which Action ID (ActionID -> Username)
Dictionary<int, string> _actionOwnerMap = new();

// Logic flow control
bool waitingForEngine = false;
bool gameRunning = true;

Console.WriteLine("=== AGORA GAME ENGINE SANDBOX ===");

// 1. Setup the Engine
var engine = new GameEngine();

// 2. Subscribe to Events
engine.OnUpdateGame += HandleGameUpdate;
engine.OnError += (msg) => {
    Console.WriteLine($"[ERROR] {msg}");
    waitingForEngine = false;
};
engine.OnEndGame += (payload) => {
    Console.WriteLine($"\n=== GAME OVER ===");
    gameRunning = false;
};

// 4. Load Game
Console.Write("Loading UNO... ");
var loadResult = engine.LoadGame(gameKey, players);

if (!loadResult.IsSuccess)
{
    Console.WriteLine($"FAILED: {loadResult.Error}");
    return;
}

string json = JsonConvert.SerializeObject(loadResult.Value, Formatting.Indented);
Console.WriteLine(json);

// 5. Start Game
Console.WriteLine("Starting Game Loop...");
waitingForEngine = true;
engine.StartGame();

// 6. The Input Loop
while (gameRunning)
{
    // We sit here while the engine is thinking or processing logic
    // We only unblock when HandleGameUpdate sets _waitingForEngine = false
    while (waitingForEngine && gameRunning) 
    {
        Thread.Sleep(100); 
    }

    if (!gameRunning) break;

    // Prompt user
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.Write("\n> Enter Action ID (or 'ID VALUE' for input): ");
    Console.ResetColor();
    
    var rawInput = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(rawInput)) continue;

    string[] parts = rawInput.Trim().Split(' ', 2); // Split into max 2 parts

    // Case A: Just an ID ("105") -> Standard Action
    if (parts.Length == 1 && int.TryParse(parts[0], out int actionId))
    {
        if (_actionOwnerMap.TryGetValue(actionId, out var playerUsername))
        {
            Console.WriteLine($"Executing Action {actionId} for {playerUsername}...");
            waitingForEngine = true;
            engine.ExecuteAction(playerUsername, actionId);
        }
        else
        {
            Console.WriteLine("Error: Action ID not found in current state.");
        }
    }
    // Case B: ID + Value ("105 red") -> Input Action
    else if (parts.Length == 2 && int.TryParse(parts[0], out int inputId))
    {
        string payloadValue = parts[1]; // e.g. "red" or "card_123"

        if (_actionOwnerMap.TryGetValue(inputId, out var playerUsername))
        {
            Console.WriteLine($"Sending Input {inputId}='{payloadValue}' for {playerUsername}...");
            waitingForEngine = true;
            // Note: We send the string value directly as the payload
            engine.ExecuteInput(playerUsername, inputId, payloadValue); 
        }
        else
        {
            Console.WriteLine("Error: Input ID not found in current state.");
        }
    }
    else
    {
        Console.WriteLine("Invalid Format. Use 'ID' for actions or 'ID VALUE' for inputs.");
    }
}

Console.WriteLine("Sandbox Session Ended. Press Enter to exit.");
Console.ReadLine();

/// <summary>
/// This triggers whenever the Engine broadcasts a new state.
/// It prints the board and unlocks the main thread to accept input.
/// </summary>
void HandleGameUpdate(Dictionary<string, UpdateGamePayload> states)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("=== GAME STATE UPDATE RECEIVED ===");
    Console.ResetColor();

    _actionOwnerMap.Clear();
    
    // Loop through every player's view
    foreach (var kvp in states)
    {
        string player = kvp.Key;
        UpdateGamePayload payload = kvp.Value;
        var isPlayerTurn = payload.PlayersTakingTurn.Contains(player);

        if (isPlayerTurn)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n----- View for {player} [YOUR TURN] -----");
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"\n----- View for {player} -----");
        }
        
        // ---------------------------------------------------------
        // 0. PENDING INPUT
        // ---------------------------------------------------------
        if (payload.Input != null)
        {
            var inp = payload.Input;
            _actionOwnerMap[inp.Id] = player;
            
            var details = GetPropertiesString(inp);

            Console.WriteLine($"   [INPUT]");
            Console.WriteLine($"     ({inp.Id}) {inp.Key} -> {details}");
        }
        
        // ---------------------------------------------------------
        // 1. Commands (Always shown so you know what you can do)
        // ---------------------------------------------------------
        Console.WriteLine("   [ACTIONS]");
        if (payload.Actions.Count > 0)
        {
            foreach (var act in payload.Actions)
            {
                // Reusing your existing helper for details
                var details = GetPropertiesString(act);
                Console.WriteLine($"     ({act.Id}) {act.Key} -> {details}");
                
                // Map this ID to this player so we can execute it later
                _actionOwnerMap[act.Id] = player;
            }
        }
        else
        {
            Console.WriteLine("     (None)");
        }

        // If the flag is false, skip the visual fluff
        if (!showDetails) continue;

        // ---------------------------------------------------------
        // 2. Descriptions (Game State Text)
        // ---------------------------------------------------------
        Console.WriteLine("\n   [DESCRIPTIONS]");
        if (payload.Descriptions.Count > 0)
        {
            foreach (var desc in payload.Descriptions)
            {
                Console.WriteLine($"     [{desc.Key}] {desc.Value}");
            }
        }
        else
        {
            Console.WriteLine("     (None)");
        }

        // ---------------------------------------------------------
        // 3. Animations (Visual Cues)
        // ---------------------------------------------------------
        Console.WriteLine("\n   [ANIMATIONS]");
        if (payload.Animations.Count > 0)
        {
            foreach (var anim in payload.Animations)
            {
                var details = GetPropertiesString(anim);
                Console.WriteLine($"     [{anim.Key}] {details}");
            }
        }
        else
        {
            Console.WriteLine("     (None)");
        }
    }

    Console.WriteLine("\n----------------------------------");
    
    // Unlock the Main Thread so the user can type
    waitingForEngine = false;
}


string GetPropertiesString(object obj)
{
    // 1. Get the actual runtime type (e.g., DrawCardActionDto)
    var type = obj.GetType();

    // 2. Get all public properties
    var properties = type.GetProperties();

    var values = new List<string>();

    foreach (var prop in properties)
    {
     // Optional: Skip properties you already printed manually
     if (prop.Name == "Id" || prop.Name == "Key") continue;

     var val = prop.GetValue(obj);

     // Handle nulls gracefully
     string valStr = val?.ToString() ?? "null";

     values.Add($"{prop.Name}: {valStr}");
    }

    return values.Count > 0 ? $"({string.Join(", ", values)})" : "";
}
