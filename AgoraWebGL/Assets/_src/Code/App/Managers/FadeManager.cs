using System.Threading.Tasks;
using UnityEngine;

namespace _src.Code.App.Managers
{
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager Instance { get; private set; }
        
        [SerializeField] private CanvasGroup fadeCanvasGroup;
        [SerializeField] private float fadeDuration = 1f;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public async Task FadeIn()
        {
            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.gameObject.SetActive(true);

            while (fadeCanvasGroup.alpha < 1)
            {
                fadeCanvasGroup.alpha += Time.deltaTime / fadeDuration;
                await Task.Yield();
            }

            fadeCanvasGroup.alpha = 1;
        }

        public async Task FadeOut()
        {
            while (fadeCanvasGroup.alpha > 0)
            {
                fadeCanvasGroup.alpha -= Time.deltaTime / fadeDuration;
                await Task.Yield();
            }

            fadeCanvasGroup.alpha = 0;
            fadeCanvasGroup.gameObject.SetActive(false);
        }
    }
}