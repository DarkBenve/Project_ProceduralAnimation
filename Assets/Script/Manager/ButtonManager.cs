using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Manager
{
    public class ButtonManager
    {
        public ButtonManager(Button restartButton)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}