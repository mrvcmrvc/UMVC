using UnityEngine;

public static class RectTransformExtensions
{
    public static void SetPivotWithCounterAdjustPosition(this RectTransform rectTrans, Vector2 newPivot)
    {
        Vector2 pivotDelta = newPivot - rectTrans.pivot;
        Vector2 posDelta = new Vector2(rectTrans.sizeDelta.x * pivotDelta.x, rectTrans.sizeDelta.y * pivotDelta.y);

        rectTrans.pivot = newPivot;
        rectTrans.anchoredPosition += posDelta;
    }

    /// <summary>
    /// Use this method if target rectTransform's size changed
    /// </summary>
    /// <param name="rectTrans"></param>
    /// <param name="newPivot"></param>
    /// <param name="initialSize"></param>
    public static void SetPivotWithCounterAdjustPosition(this RectTransform rectTrans, Vector2 newPivot, Vector2 initialSize)
    {
        Vector2 pivotDelta = newPivot - rectTrans.pivot;
        Vector2 posDelta = new Vector2(initialSize.x * pivotDelta.x, initialSize.y * pivotDelta.y);

        rectTrans.pivot = newPivot;
        rectTrans.anchoredPosition += posDelta;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvasScale">Canvas.transform.lossyScale.x</param>
    /// <returns></returns>
    public static Rect GetRect(this RectTransform rectTrans, float canvasScale)
    {
        Rect targetRect = new Rect(rectTrans.rect)
        {
            position = rectTrans.position
        };

        targetRect.size *= canvasScale;
        targetRect.position -= targetRect.size * rectTrans.pivot;

        return targetRect;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rectTrans2">Other RectTransform</param>
    /// <param name="canvasScale">Canvas.transform.lossyScale.x</param>
    /// <returns></returns>
    public static bool RectOverlaps(this RectTransform rectTrans1, RectTransform rectTrans2, float canvasScale)
    {
        Rect rect1 = rectTrans1.GetRect(canvasScale);
        Rect rect2 = rectTrans2.GetRect(canvasScale);

        return rect1.Overlaps(rect2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rectTrans2">Other RectTransform</param>
    /// <param name="canvasScale">Canvas.transform.lossyScale.x</param>
    /// <returns></returns>
    public static bool RectEnvelopes(this RectTransform rectTrans1, RectTransform rectTrans2, float canvasScale)
    {
        Vector3[] objectCorners = new Vector3[4];
        rectTrans2.GetWorldCorners(objectCorners);

        return RectContainsPoints(rectTrans1, canvasScale, objectCorners);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="canvasScale">Canvas.transform.lossyScale.x</param>
    /// <param name="corners"></param>
    /// <returns></returns>
    public static bool RectContainsPoints(this RectTransform rectTrans1, float canvasScale, params Vector3[] corners)
    {
        Rect rect1InWorld = rectTrans1.GetRect(canvasScale);

        bool envelopes = true;
        for (var i = 0; i < corners.Length; i++)
        {
            if (!rect1InWorld.Contains(corners[i]))
            {
                envelopes = false;

                break;
            }
        }

        return envelopes;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetTransform">target transform that is seeing this transform</param>
    /// <param name="canvasScale">Canvas.transform.lossyScale.x</param>
    /// <returns></returns>
    public static bool IsFullyVisibleFrom(this RectTransform rectTransform, RectTransform targetTransform)
    {
        return RectEnvelopes(targetTransform, rectTransform, targetTransform.transform.lossyScale.x);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetTransform">target transform that is seeing this transform</param>
    /// <param name="canvasScale">Canvas.transform.lossyScale.x</param>
    /// <returns></returns>
    public static bool IsVisibleFrom(this RectTransform rectTransform, RectTransform targetTransform)
    {
        return RectOverlaps(targetTransform, rectTransform, targetTransform.transform.lossyScale.x);
    }

    public static Vector3 GetWorldPositionWithoutPivot(this RectTransform rectTransform, Vector3 canvasScale)
    {
        Vector2 pivotPoint = rectTransform.pivot;
        Vector2 rectSize = rectTransform.rect.size * canvasScale;

        Vector3 offset = rectSize * (new Vector2(0.5f, 0.5f) - pivotPoint);
        offset.z = 0f;

        return rectTransform.position + offset;
    }
}
