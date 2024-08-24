using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ButtonManager
    {
        public ButtonManager(Button restartButton, Button startGame)
        {
            if (restartButton != null)
                restartButton.onClick.AddListener(RestartGame);
            if (startGame != null)
                startGame.onClick.AddListener(StartGame);
        }
        
        private void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void StartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}