using Agora.Core.Dtos.Game.GameModules;
using Agora.Engine.Entities.@base;

namespace Agora.Engine.Entities;

public class Deck : DeckDto, IGameModule
{
    public new List<Card> Cards { get; set; } = new List<Card>();

    public GameModuleDto ToDto()
    {
        return new DeckDto()
        {
            Id = Id,
            Name = Name,
            Type = Type,
            Color = Color,
            TopImage = TopImage,
            Cards = Cards.Select(card => card.ToDto() as CardDto).ToList(),
        };
    }
}