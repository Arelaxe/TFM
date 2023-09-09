using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Drag : Selectable, ISubmitHandler
{
    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private GameObject submitTarget;

    [SerializeField]
    private GameObject upperLimit;

    [SerializeField]
    private GameObject lowerLimit;

    public void DragHandler(BaseEventData data){

        PointerEventData pointerData = (PointerEventData)data;

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            pointerData.position,
            canvas.worldCamera,
            out position);
        transform.position = LimitPosition(canvas.transform.TransformPoint(position));
    }

    public void setCanvas(Canvas canvas){
        this.canvas = canvas;
    }

    protected Vector3 LimitPosition(Vector3 newPosition)
    {
        Vector3 upperLimitPos = upperLimit.transform.position;
        Vector3 lowerLimitPos = lowerLimit.transform.position;

        float newX = newPosition.x;
        if (newPosition.x < lowerLimitPos.x)
        {
            newX = lowerLimitPos.x;
        }
        else if (newPosition.x > upperLimitPos.x)
        {
            newX = upperLimitPos.x;
        }

        float newY = newPosition.y;
        if (newPosition.y < lowerLimitPos.y)
        {
            newY = lowerLimitPos.y;
        }
        else if (newPosition.y > upperLimitPos.y)
        {
            newY = upperLimitPos.y;
        }

        return new(newX, newY, newPosition.z);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (submitTarget)
        {
            ElectricInput electricInput = submitTarget.GetComponent<ElectricInput>();
            if (electricInput)
            {
                electricInput.TrySetFuse(gameObject);
            }
        }
    }
}
