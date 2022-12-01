using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.UI.Interfaces
{
    public class OptionsMenu : MonoBehaviour
    {
        #region Variables
        [Header("Panels references")]
        [SerializeField] private GameObject[] panels;
        #endregion

        #region Builts_In
        private void Awake()
        {
            EnableAllPanels(false);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            EnableAllPanels(false);
            gameObject.SetActive(false);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable or disable all the panels in the panels array
        /// </summary>
        /// <param name="state"> new state </param>
        private void EnableAllPanels(bool state)
        {
            if (panels.Length <= 0)
                return;

                foreach (GameObject panel in panels)
            {
                if (!panel)
                    continue;

                panel.SetActive(state);
            }
        }

        /// <summary>
        /// Enable a panel by referencing a index
        /// </summary>
        public void EnablePanelByIndex(int index)
        {
            EnableAllPanels(false);

            if (panels.Length <= 0 || panels.Length < index)
                return;

            Debug.Log("coucou");
            panels[index].SetActive(true);
        }

        /// <summary>
        /// Disable a panel by referencing a index
        /// </summary>
        public void DisablePanelByIndex(int index)
        {
            if (panels.Length <= 0 || panels.Length >= index)
                return;

            panels[index].SetActive(false);
        }
        #endregion
    }
}
