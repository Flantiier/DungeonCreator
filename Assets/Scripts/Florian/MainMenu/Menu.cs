using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("Menu References")]
    [Tooltip("The menuState of this menu")]
    [SerializeField] private MM_UIManager.MenuState menuState;
    [Tooltip("Referncing the panel to display the menu")]
    [SerializeField] private GameObject menuPanel;

    private void Awake()
    {
        menuPanel.SetActive(false);   
    }

    //Subscribe the changing menu method to the menuChanged from the menuManger
    private void OnEnable()
    {
        MM_UIManager.onMenuChanged += ChangeMenu;
    }

    private void OnDisable()
    {
        MM_UIManager.onMenuChanged -= ChangeMenu;
    }

    //Checking if the new menuState is the same as this local menuState variable (If yes, display)
    private void ChangeMenu(MM_UIManager.MenuState state)
    {
        if (state != menuState)
            menuPanel.SetActive(false);
        else
            menuPanel.SetActive(true);
    }
}
