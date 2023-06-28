using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAudioHandler : MonoBehaviour, IPointerEnterHandler
{
    #region Variables
    [SerializeField] private AudioClip mouseOverClip;
    [SerializeField] private AudioClip mouseClickClip;

    private Button _button;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(PlayClickSound);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(PlayClickSound);
    }
    #endregion

    #region Methods
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable)
            return;

        PersistentAudioSource.Instance.PlaySound(mouseOverClip);
    }

    private void PlayClickSound()
    {
        if (!_button.interactable)
            return;

        PersistentAudioSource.Instance.PlaySound(mouseClickClip);
    }
    #endregion
}
