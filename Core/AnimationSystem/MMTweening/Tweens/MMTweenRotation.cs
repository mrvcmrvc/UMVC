using UnityEngine;

public class MMTweenRotation : MMUITweener
{
    public Vector3 From, To;
    
    public Vector3 Value { get; private set; }

    Transform myTransform;

    protected override void Wake()
    {
        myTransform = gameObject.GetComponent<Transform>();

        base.Wake();
    }

    protected override void SetValue(float clampedValue)
    {
        if (Ease == MMTweeningEaseEnum.Shake/* || Ease == MMTweeningEaseEnum.Punch*/)
        {
            Vector3 delta = ShakePunchDirection * ShakePunchAmount * clampedValue;

            Value = myTransform.localEulerAngles + delta;
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
        if (myTransform == null)
            return;

        myTransform.localRotation = Quaternion.Euler(Value);
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
        From = GetComponent<Transform>().localEulerAngles;
    }

    [ContextMenu("Set TO")]
    void SetTo()
    {
        To = GetComponent<Transform>().localEulerAngles;
    }

    [ContextMenu("Assume FROM")]
    void AssumeFrom()
    {
        GetComponent<Transform>().localEulerAngles = From;
    }

    [ContextMenu("Assume TO")]
    void AssumeTo()
    {
        GetComponent<Transform>().localEulerAngles = To;
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
