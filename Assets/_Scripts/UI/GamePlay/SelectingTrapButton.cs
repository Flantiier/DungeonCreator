using UnityEngine;
using TMPro;
using _Scripts.Characters.DungeonMaster;
using _ScriptablesObjects.Traps;

namespace _Scripts.TrapSystem.UI
{
    public class SelectingTrapButton : MonoBehaviour
    {/*
        [Header("Button infos")]
        [SerializeField] private TrapSO trapToSelect;
        [SerializeField] private DamagingTrapSO trapDamageableToSelect;

        public void Awake()
        {
            TextMeshProUGUI textComp = GetComponentInChildren<TextMeshProUGUI>();

            if (!trapToSelect && !trapDamageableToSelect)
            {
                return;
            }
            else if(trapToSelect)
            {
                if (textComp)
                    textComp.text = trapToSelect.trapName;
            }
            else
            {
                if (textComp)
                    textComp.text = trapDamageableToSelect.trapName;
            }
        }

        public void SelectTrap()
        {
            if(trapToSelect != null)
            {
                DMController.SelectingTrap(trapToSelect);
            }
            else if(trapDamageableToSelect != null)
            {
                DMController.SelectingTrap(trapDamageableToSelect);
            }
        }
*/
    }
}
