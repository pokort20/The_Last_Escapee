using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform buttonTransform;

    void OnDisable()
    {
        OnPointerExit(null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GameManager.instance.isPaused || GameManager.instance.isPlayerDead)
        {
            Time.timeScale = 1.0f;
            AudioManager.instance.playAudio("item_use");
            Time.timeScale = 0.0f;
        }
        else
        {
            AudioManager.instance.playAudio("item_use");
        }
        buttonTransform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //throw new System.NotImplementedException();
    }
}
