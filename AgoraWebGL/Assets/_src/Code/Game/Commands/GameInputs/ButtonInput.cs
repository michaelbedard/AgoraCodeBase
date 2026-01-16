using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands.Inputs;

namespace _src.Code.Game.Commands.GameInputs
{
    public class ButtonInput : BaseInput<ButtonInputDto>
    {
        private readonly IVisualElementService _visualElementService;
        private readonly IGameHubProxy _gameHubProxy;

        // private CircularButtonPanel _panel;

        public ButtonInput()
        {
            _visualElementService = ServiceLocator.GetService<IVisualElementService>();
            _gameHubProxy = ServiceLocator.GetService<IGameHubProxy>();
        }

        // ask
        protected override async void AskCore(ButtonInputDto input)
        {
            // set and show btn
            // _panel = await _visualElementService.Create<CircularButtonPanel>();
            //
            // _panel.AbsolutePosition = new Vector2(1400, 700);
            // _panel.CircularButton.IconAddress = input.ImageAddress;
            // _panel.CircularButton.Text = input.Text;
            //
            // _panel.Show(false);
            // _panel.CircularButton.Clicked += () =>
            // {
            //     _gameHubProxy.ExecuteInput(new ExecuteInputPayload()
            //     {
            //         InputId = input.Id,
            //         Answer = null
            //     });
            // };
        }

        public override void Cancel()
        {
            // if (_panel != null)
            // {
            //     _panel.Hide();
            // }
        }
    }
}