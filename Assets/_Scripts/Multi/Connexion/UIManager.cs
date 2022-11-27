using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public void DMDeck()
        {
            SceneManager.LoadSceneAsync("Menu_Deck");
        }

        public void Parameters()
        {
            SceneManager.LoadSceneAsync("Menu_Parameters");
        }

        public void Credit()
        {
            SceneManager.LoadSceneAsync("Menu_Credit");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}