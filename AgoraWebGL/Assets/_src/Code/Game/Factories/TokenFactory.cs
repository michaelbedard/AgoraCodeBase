using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Game.Modules.Token;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Factories
{
    public class PlaceholderTokenFactory : PlaceholderFactory<TokenDto, IToken>
    {
    }
    
    public class TokenFactory : GameModuleFactory<Token, TokenModel, TokenDto, IToken>, ITokenFactory
    {
        public TokenFactory(DiContainer container, GameObject prefab)
            : base(container, prefab)
        {
        }

        protected override async Task<IToken> Setup(Token token, TokenDto loadData)
        {
            token = (Token)await base.Setup(token, loadData);
            
            token.CanBeClick = false;
            token.CanBeDrag = false;
            
            return token;
        }
    }
}