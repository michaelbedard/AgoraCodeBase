using Agora.Core.Dtos.Game.Commands.Actions;
using Agora.Engine.Commands.Actions;

namespace Agora.Engine.Games.Uno;

public partial class Uno
{
    protected override async Task GameLogicAsync()
    {
        Execute(new ShuffleDeck(null, _deck));
        
        // 1. Deal 7 cards to each player
        for (var i = 0; i < 7; i++)
        {
            foreach (var p in Players)
            {
                Execute(new DrawCard(p, _deck));
            }
        }

        // 2. Flip first card
        if (_deck.Cards.Count > 0)
        {
            Execute(new FlipTopCard(null, _deck, _discardPile));
            _activeColor = GetTopCard().Color;
        }

        // 3. Main Game Loop
        while (true)
        {
            var currentPlayer = Players[_currentPlayerIndex];
            
            // Check Win Condition
            if (Players.Any(p => p.Hand.Count == 0)) 
            {
                goto EndGame;
            }

            SetWhoIsTakingTurn(currentPlayer);
            var topCard = GetTopCard();

            // --- STEP A: GENERATE ALLOWED MOVES ---
            
            Allow(new DrawCard(currentPlayer, _deck));

            foreach (var card in currentPlayer.Hand.OfType<UnoCard>())
            {
                if (IsValidMove(card, topCard))
                {
                    Allow(new PlayCard(currentPlayer, card));
                    Allow(new PlayCardInsideZone(currentPlayer, card, _discardPile));
                }
            }

            // --- STEP B: WAIT FOR ACTION ---
            var actionTaken = await WaitForNextActionAsync();

            // --- STEP C: RESOLVE EFFECTS ---
            var playedCard = actionTaken switch
            {
                PlayCard p => p.Card as UnoCard,
                PlayCardInsideZone pz => pz.Card as UnoCard,
                _ => null
            };
            
            if (playedCard != null)
            {
                _activeColor = playedCard.Color;
                
                if (playedCard is SpecialUnoCard special)
                {
                    await special.ApplyEffectAsync(this);
                }
                else
                {
                    AdvanceTurn();
                }
            }
            else if (actionTaken is DrawCard)
            {
                AdvanceTurn();
            }
        }

        EndGame:
        Console.WriteLine("Game Over");
        Console.WriteLine($"WINNER: {Players.First(p => p.Hand.Count == 0).Username}");
    }

    // --- Helper Methods ---

    private UnoCard GetTopCard()
    {
        return _discardPile.Cards.Last() as UnoCard;
    }

    private bool IsValidMove(UnoCard handCard, UnoCard topCard)
    {
        if (topCard == null) return true;
        
        // 1. Wild cards can always be played
        if (handCard.Color == "black") return true;

        // 2. Color Match (Check against _activeColor for Wild resolutions)
        if (handCard.Color == _activeColor) return true;

        // 3. Value/Symbol Match
        if (handCard.Value == topCard.Value) return true;

        return false;
    }

    private void AdvanceTurn(int steps = 1)
    {
        _currentPlayerIndex += (steps * _direction);
        
        // Wrap around logic
        if (_currentPlayerIndex >= Players.Count) 
            _currentPlayerIndex %= Players.Count;
        else if (_currentPlayerIndex < 0) 
            _currentPlayerIndex = Players.Count + (_currentPlayerIndex % Players.Count);
            
        // Handle exact 0 case from negative modulo in C#
        if (_currentPlayerIndex == Players.Count) _currentPlayerIndex = 0;
    }
    
    public void ReverseTurnOrder()
    {
        _direction *= -1;
        // Logic usually implies the CURRENT player finished, so we move to the next in the new direction
        AdvanceTurn(); 
    }

    public void DrawCards(int count)
    {
        // The NEXT player draws
        // Determine next player without advancing the index permanently yet
        int nextIndex = _currentPlayerIndex + _direction;
        if (nextIndex >= Players.Count) nextIndex = 0;
        if (nextIndex < 0) nextIndex = Players.Count - 1;

        var victim = Players[nextIndex];
        
        for(int i=0; i<count; i++) 
            Execute(new DrawCard(victim, _deck));
            
        // Usually, drawing +2/+4 skips the victim's turn
        AdvanceTurn(2); 
    }
}