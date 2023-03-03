using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class VariableDisplayer : MonoBehaviour
{

    [SerializeField] protected TextMeshProUGUI textMeshPro;
    [SerializeField] protected FloatVariable variable;
    [SerializeField] protected StringRound rounding;

    private void Update()
    {
        UpdateText();
    }

    protected virtual void UpdateText()
    {
        if (!textMeshPro)
            return;

        if (!variable)
            return;

        textMeshPro.text = rounding.RoundValueInString(variable.value);
    }
}

#region RoundString class
[System.Serializable, HideLabel]
public class StringRound
{
    public enum RoundType { F0, F1, F2, F3 }
    [SerializeField] private RoundType roundType;

    public string RoundValueInString(float value)
    {
        switch (roundType)
        {
            case RoundType.F1:
                return value.ToString("F1");
            case RoundType.F2:
                return value.ToString("F2");
            case RoundType.F3:
                return value.ToString("F3");
            default:
                return value.ToString("F0");
        }
    }
}
#endregion
