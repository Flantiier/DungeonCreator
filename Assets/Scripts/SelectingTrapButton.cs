using UnityEngine;
using TMPro;
using _Scripts.DungeonMaster;
using _Scripts.TrapSystem.Datas;

namespace _Scripts.TrapSystem.UI
{
    public class SelectingTrapButton : MonoBehaviour
    {
        [Header("Button infos")]
        [SerializeField] private TrapSO trapToSelect;

        public void Awake()
        {
            if(!trapToSelect)
            {
                Debug.LogError($"Missing trap reference on button : {gameObject}");
                return;
            }

            TextMeshProUGUI textComp = GetComponentInChildren<TextMeshProUGUI>();

            if (textComp)
                textComp.text = trapToSelect.trapName;
        }

        public void SelectTrap()
        {
            DMController.SelectingTrap(trapToSelect);
        }
    }
}
