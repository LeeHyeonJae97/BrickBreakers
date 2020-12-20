using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AlertConfirm : MonoBehaviour
{
    public Text message;
    public RectTransform confirmButton;
    public RectTransform cancelButton;
    private UnityAction onClickConfirmButton;

    public void Alert(string message)
    {
        gameObject.SetActive(true);

        this.message.text = message;

        confirmButton.gameObject.SetActive(true);        

        confirmButton.anchoredPosition = new Vector2(0, -70);
    }

    public void Confirm(string message, UnityAction onClick)
    {
        gameObject.SetActive(true);

        this.message.text = message;

        onClickConfirmButton = onClick;

        confirmButton.gameObject.SetActive(true);
        cancelButton.gameObject.SetActive(true);

        confirmButton.anchoredPosition = new Vector2(-60, -70);
        cancelButton.anchoredPosition = new Vector2(-60, -70);
    }

    public void OnClickCancelButton()
    {
        gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    public void OnClickConfirmButton()
    {
        OnClickCancelButton();
        onClickConfirmButton?.Invoke();
    }
}
