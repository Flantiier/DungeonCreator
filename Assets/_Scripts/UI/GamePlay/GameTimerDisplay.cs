using System;
using UnityEngine;

namespace _Scripts.UI
{
    public class GameTimerDisplay : VariableDisplayer
    {
        protected override void UpdateText()
        {
            if (!textMeshPro || !variable)
                return;

            //Timer over 1 minut
            if (variable.value > 60)
            {
                float seconds = variable.value % 60;
                float minuts = Mathf.Floor(variable.value / 60);
                textMeshPro.text = minuts.ToString("00") + ":" +  seconds.ToString("00");
                return;
            }
            //Timer under 1 minut
            textMeshPro.text = rounding.RoundValueInString(variable.value);
        }
    }
}
