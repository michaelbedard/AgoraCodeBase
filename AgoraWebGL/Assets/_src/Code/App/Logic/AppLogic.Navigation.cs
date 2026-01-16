using System.Threading.Tasks;

namespace _src.Code.App.Logic
{
    public partial class AppLogic
    {
        public async Task LoadEntryScene()
        {
            // load
            await _sceneService.LoadEntryScreen();
            
            // set music
            // await _audioService.PlayBackgroundMusicAsync("Assets/_src/Audio/439_Goodhaven.mp3");
        }
        
        public async Task LoadGameScene(bool showLoginScreen)
        {

        }
    }
}