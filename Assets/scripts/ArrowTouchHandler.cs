using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTouchHandler : MonoBehaviour
{
    public TileControllerHoldToTop tileHold;
    public bool isArrowHeld = false;
    private Vector3 originalScale;

    void Start() {
        originalScale = transform.localScale;
    }

    void OnMouseDown()
    {
        if (tileHold != null && !isArrowHeld)
        {
            isArrowHeld = true;
            tileHold.OnStarPointerDown();
        }
    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            tileHold.OnArrowPointerUp();
        }
    }


}
