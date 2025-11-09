using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

// We inherit from Button, but also tell it we are implementing these interfaces
public class ButtonBase : Button, IPointerDownHandler, IPointerUpHandler
{

    public bool isOtherTarget = false;
    private Transform target;

    // FIX: Added "protected new" to specify this is a new Awake,
    // not overriding the one in the 'Selectable' parent class.
    protected new void Awake()
    {
        base.Awake(); // It's also good practice to call the base function
        if (isOtherTarget)
        {
            target = transform.GetChild(0);
        }
        else
        {
            target = transform;
        }
    }

    // FIX: Added "public override" because we ARE overriding
    // the parent's (Selectable) OnPointerDown function.
    public override void OnPointerDown(PointerEventData data)
    {
        base.OnPointerDown(data); // Call the base function
        target.transform.DOKill();
        target.transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    // FIX: Added "public override" here as well.
    public override void OnPointerUp(PointerEventData data)
    {
        base.OnPointerUp(data); // Call the base function
        target.transform.DOKill();
        target.transform.DOScale(1f, 0.4f).SetEase(Ease.OutElastic).SetUpdate(true);
    }
}