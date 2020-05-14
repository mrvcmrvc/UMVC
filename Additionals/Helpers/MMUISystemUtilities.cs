using UnityEngine;

public static class MMUISystemUtilities
{
    public static Vector2 WorldToCanvasOverlayPoint(this Camera targetCamera, Vector3 worldPos)
    {
        Vector2 screenPos = targetCamera.WorldToScreenPoint(worldPos);

        return screenPos;
    }
}