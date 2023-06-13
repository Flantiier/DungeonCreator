using UnityEngine;
using _ScriptableObjects.GameManagement;

namespace _Scripts.UI.Menus
{
	public class LoadingInfos : MonoBehaviour
	{
		[SerializeField] private GameProperties properties;
        [SerializeField] private GameObject[] panels;

        private void Awake()
        {
            switch (properties.role)
            {
                case Role.Warrior:
                    panels[0].SetActive(true);
                    break;
                case Role.Archer:
                    panels[1].SetActive(true);
                    break;
                case Role.Wizard:
                    panels[2].SetActive(true);
                    break;
                case Role.Master:
                    panels[3].SetActive(true);
                    break;
            }
        }
    }
}
