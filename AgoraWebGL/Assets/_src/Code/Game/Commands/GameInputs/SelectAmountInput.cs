using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.UI.Scenes.Game.GameInputs;
using Agora.Core.Dtos.Game.Commands.Inputs;
using Agora.Core.Payloads.Hubs;

namespace _src.Code.Game.Commands.GameInputs
{
    public class SelectAmountInput : BaseInput<SelectAmountDto>
    {
        private readonly IVisualElementService _visualElementService;
        private readonly IGameHubProxy _gameHubProxy;

        private SelectAmountPanel _panel;

        public SelectAmountInput()
        {
            _visualElementService = ServiceLocator.GetService<IVisualElementService>();
            _gameHubProxy = ServiceLocator.GetService<IGameHubProxy>();
        }

        // ask
        protected override async void AskCore(SelectAmountDto input)
        {
            // set label
            _panel = await _visualElementService.GetOrCreate<SelectAmountPanel>();
            _panel.Title.Label.text = input.Label;
            _panel.TextInput.Value = input.Min.ToString();

            _panel.Show(false);

            _panel.SecondaryButton.Clicked += () =>
            {
                _gameHubProxy.ExecuteInput(new ExecuteInputPayload()
                {
                    InputId = input.Id,
                    Answer = _panel.TextInput.Value
                });
                
                _panel.Hide();
            };
        }

        public override void Cancel()
        {
            _panel.Hide();
        }
    }
}