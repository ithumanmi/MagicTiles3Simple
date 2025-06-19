using UnityEngine;

public class StarTouchHandler : MonoBehaviour
{
    public TileControllerHoldToTop tileHold;

    void OnMouseDown()
    {
        if (tileHold != null)
        {
          
            tileHold.OnStarPointerDown();
        }
    }
} 