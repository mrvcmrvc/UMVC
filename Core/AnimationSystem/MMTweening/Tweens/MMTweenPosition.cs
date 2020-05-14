using UnityEngine;

public class MMTweenPosition : MMUITweener
{
    public Vector3 From, To;
    public bool UseRigidbody;
    public bool UseTransform;

    public Vector3 Value { get; private set; }

    RectTransform myRectTransform;
    Transform myTransform;
    Rigidbody myRigidbody;
    //Rigidbody2D myRigidbody2D;

    protected override void Wake()
    {
        if (UseRigidbody)
            CheckForRigidbodyAndCollider();
        else if(!UseTransform)
            myRectTransform = gameObject.GetComponent<RectTransform>();
        else
            myTransform = gameObject.GetComponent<Transform>();

        base.Wake();
    }

    void CheckForRigidbodyAndCollider()
    {
        //TODO: Şimdilik sadece 3D rigidbody check edilip ekleniyor, ilerde projenin 2D veya 3D olmasına bağlı olarak check işlemi yapılmalı
        if (gameObject.GetComponent<Rigidbody>() == null)
        {
            myRigidbody = gameObject.AddComponent<Rigidbody>();
            myRigidbody.isKinematic = true;
        }
        else
            myRigidbody = gameObject.GetComponent<Rigidbody>();

        if (gameObject.GetComponent<Collider>() == null)
            gameObject.AddComponent<BoxCollider>();
    }

    protected override void SetValue(float clampedValue)
    {
        if(Ease == MMTweeningEaseEnum.Shake/* || Ease == MMTweeningEaseEnum.Punch*/)
        {
            Vector2 delta = ShakePunchDirection * ShakePunchAmount * clampedValue;

            if (myRectTransform != null)
                Value = myRectTransform.anchoredPosition + delta;
            else if (myTransform != null)
                Value = myTransform.localPosition + (Vector3)delta;
        }
        else
        {
            Vector3 diff = To - From;
            Vector3 delta = diff * clampedValue;

            Value = From + delta;
        }
    }

    protected override void PlayAnim()
    {
        if (UseRigidbody)
        {
            if (myRigidbody != null)
                myRigidbody.position = Value;
            //else if (myRigidbody2D != null)
            //    myRigidbody2D.position = Value;
        }
        else if (myRectTransform != null)
            myRectTransform.anchoredPosition = Value;
        else if (myTransform != null)
            myTransform.localPosition = Value;
    }

    protected override void Finish()
    {
    }

    protected override void Kill()
    {
    }

    #region ContextMenu
    [ContextMenu("Set FROM")]
    void SetFrom()
    {
        RectTransform rt = GetComponent<RectTransform>();

        if(rt != null)
            From = rt.anchoredPosition;
        else
        {
            Transform t = GetComponent<Transform>();

            From = t.localPosition;
        }
    }

    [ContextMenu("Set TO")]
    void SetTo()
    {
        RectTransform rt = GetComponent<RectTransform>();

        if (rt != null)
            To = rt.anchoredPosition;
        else
        {
            Transform t = GetComponent<Transform>();

            To = t.localPosition;
        }
    }

    [ContextMenu("Assume FROM")]
    void AssumeFrom()
    {
        if (GetComponent<RectTransform>() != null)
            GetComponent<RectTransform>().anchoredPosition = From;
        else if (GetComponent<Transform>() != null)
            GetComponent<Transform>().localPosition = From;
    }

    [ContextMenu("Assume TO")]
    void AssumeTo()
    {
        if (GetComponent<RectTransform>() != null)
            GetComponent<RectTransform>().anchoredPosition = To;
        else if (GetComponent<Transform>() != null)
            GetComponent<Transform>().localPosition = To;
    }

    public override void InitValueToFROM()
    {
        Value = From;

        base.InitValueToFROM();
    }

    public override void InitValueToTO()
    {
        Value = To;

        base.InitValueToTO();
    }
    #endregion
}
