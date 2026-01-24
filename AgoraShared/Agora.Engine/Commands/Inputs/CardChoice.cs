using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Inputs;
using Agora.Engine.Commands._base;
using Agora.Engine.Entities;

namespace Agora.Engine.Commands.Inputs;

public class CardChoice : BaseGameInput<string, Card>
{
    public List<Card> Options { get; }

    public CardChoice(Player player, List<Card?> options) : base(player)
    {
        Options = options;
    }

    protected override GameActionResult Execute(string selectedCardName)
    {
        if (string.IsNullOrEmpty(selectedCardName))
            throw new Exception("Invalid card Name received.");

        var selectedCard = Options.FirstOrDefault(c => c.Name == selectedCardName);
        
        if (selectedCard == null)
            throw new Exception("Player tried to select a card not in the options!");

        Result = selectedCard; 
        
        return new GameActionResult();
    }

    protected override CommandDto CreateDto()
    {
        return new ChoiceInputDto()
        {
            Label = "Choose a card!!!!",
            Choices =
            [
                new TextChoiceDto() { Text = Options[0].Name },
                new TextChoiceDto() { Text = Options[1].Name },
            ]
        };
    }
}