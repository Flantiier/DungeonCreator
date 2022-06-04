using UnityEngine;

public class MM_UIManager : MonoBehaviour
{
    [Header("Differents Menus")]
    [Tooltip("Referencing the loading menu here")]
    [SerializeField] private GameObject loadingMenu;
    [Tooltip("Referencing the rooms menu here")]
    [SerializeField] private GameObject roomsMenu;

    public void LoadingGame()
    {
        roomsMenu.SetActive(false);
        loadingMenu.SetActive(true);
    }

    public void GameLoaded()
    {
        loadingMenu.SetActive(false);
        roomsMenu.SetActive(true);
    }
}
