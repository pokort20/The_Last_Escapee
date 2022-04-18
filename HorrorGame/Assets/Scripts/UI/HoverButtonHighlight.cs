using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform buttonTransform;

    void OnDisable()
    {
        OnPointerExit(null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //throw new System.NotImplementedException();
    }
}
