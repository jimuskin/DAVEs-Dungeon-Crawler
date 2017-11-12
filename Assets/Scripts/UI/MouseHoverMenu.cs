using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Class is used on hover of text to show what button is currently being selected.
public class MouseHoverMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text MenuText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuText.text = ">  " + MenuText.text;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuText.text = MenuText.text.Substring(3);
    }
}