// using Agora.Core.Dtos.Game.Commands;
// using Agora.Core.Dtos.Game.Commands.Commands;
// using Agora.Engine.Entities;
//
// namespace Agora.Engine.Commands;
//
// public class MoveCard : BaseGameAction
// {
//     public Zone ZoneFrom;
//     public Zone ZoneTo;
//     public Card Card;
//     
//     public MoveCard(Player user, Zone zoneFrom, Zone zoneTo, Card card)
//     {
//         Player = user;
//         ZoneFrom = zoneFrom;
//         ZoneTo = zoneTo;
//         Card = card;
//     }
//     
//     public override GameActionResult Execute()
//     {
//         var result = new GameActionResult();
//         if (!ZoneFrom.Cards.Contains(Card)) return result;
//         
//         ZoneFrom.Cards.Remove(Card);
//         ZoneTo.Cards.Add(Card);
//
//         // result.SendToAll(new DrawCardAnimationDto()
//         // {
//         //     Key = nameof(PlayCardAnimationDto),
//         //     PlayerId = Player.Id,
//         //     DeckId = Deck.Id,
//         //     CardId = card.Id
//         // });
//
//         return result;
//     }
//
//     protected override CommandDto CreateDto()
//     {
//         return new DrawCardActionDto()
//         {
//             DeckId = Deck.Id
//         };
//     }
// }