using MyBox;
using UnityEngine;

namespace BubbleWand {
    [CreateAssetMenu]
    sealed class GameManager : ScriptableAsset {
        [SerializeField]
        SceneReference gameScene = new();

        public void StartGame() {
            gameScene.LoadScene();
        }

        [SerializeField]
        SceneReference creditsScene = new();

        public void ShowCredits() {
            creditsScene.LoadScene();
        }

        [SerializeField]
        SceneReference mainMenuScene = new();

        public void LoadMainMenu() {
            mainMenuScene.LoadScene();
        }

        public void ExitGame() {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
#endif
        }
    }
}
