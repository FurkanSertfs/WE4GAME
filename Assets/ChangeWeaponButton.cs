using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ChangeWeaponButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public bool isPressed;

    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }
}
