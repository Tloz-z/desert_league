using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Image imageBackGround;
    private Image imageController;

    private void Awake()
    {
        imageBackGround = GetComponent<Image>();
        imageController = transform.GetChild(0).GetComponent<Image>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPosition = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imageBackGround.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            touchPosition.x = (touchPosition.x / imageBackGround.rectTransform.sizeDelta.x);
            touchPosition.y = (touchPosition.y / imageBackGround.rectTransform.sizeDelta.y);

            touchPosition = new Vector2(touchPosition.x * 2 - 1, touchPosition.y * 2 - 1);

            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            imageController.rectTransform.anchoredPosition = new Vector2(
                touchPosition.x * imageBackGround.rectTransform.sizeDelta.x / 2,
                touchPosition.y * imageBackGround.rectTransform.sizeDelta.y / 2);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        imageController.rectTransform.anchoredPosition = Vector2.zero;
    }
}
