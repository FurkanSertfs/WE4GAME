using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour,IPointerClickHandler,IPointerDownHandler
{
    public bool isPressed;

    [SerializeField]
    Image fireButton;

    private void Update()
    {
        if (isPressed)
        {
            fireButton.fillAmount += Time.deltaTime*5; 
        }
        else
        {
            fireButton.fillAmount -= Time.deltaTime * 5;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }
}
