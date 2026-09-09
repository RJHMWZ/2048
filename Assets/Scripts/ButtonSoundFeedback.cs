using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Plays button feedback before the button action is invoked.
/// </summary>
[RequireComponent(typeof(Button))]
public sealed class ButtonSoundFeedback :
    MonoBehaviour,
    IPointerDownHandler,
    ISubmitHandler
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        PlayIfInteractable();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        PlayIfInteractable();
    }

    private void PlayIfInteractable()
    {
        if (button == null || !button.IsInteractable())
            return;

        GameAudioManager.Instance.PlayButtonClick();
    }
}
