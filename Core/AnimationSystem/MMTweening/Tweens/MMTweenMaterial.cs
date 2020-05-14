using UnityEngine;

public class MMTweenMaterial : MMUITweener
{
    public Color From, To;

    public Color Value { get; private set; }

    private MeshRenderer _renderer;
    private MaterialPropertyBlock _propBlock;

    protected override void Wake()
    {
        InitRenderer();

        base.Wake();
    }

    private void InitRenderer()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<MeshRenderer>();
    }

    protected override void SetValue(float clampedValue)
    {
        float r = CalculateR(clampedValue);
        float g = CalculateG(clampedValue);
        float b = CalculateB(clampedValue);
        float a = CalculateA(clampedValue);

        Value = From + new Color(r, g, b, a);
    }

    float CalculateR(float clampedValue)
    {
        float diff = To.r - From.r;
        return diff * clampedValue;
    }

    float CalculateG(float clampedValue)
    {
        float diff = To.g - From.g;
        return diff * clampedValue;
    }

    float CalculateB(float clampedValue)
    {
        float diff = To.b - From.b;
        return diff * clampedValue;
    }

    float CalculateA(float clampedValue)
    {
        float diff = To.a - From.a;
        return diff * clampedValue;
    }

    protected override void PlayAnim()
    {
        if (_renderer == null)
            return;

        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetColor("_Color", Value);

        _renderer.SetPropertyBlock(_propBlock);
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
        InitRenderer();

        _renderer.GetPropertyBlock(_propBlock);

        From = _propBlock.GetColor("_Color");
    }

    [ContextMenu("Set TO")]
    void SetTo()
    {
        InitRenderer();

        _renderer.GetPropertyBlock(_propBlock);

        To = _propBlock.GetColor("_Color");
    }

    [ContextMenu("Assume FROM")]
    void AssumeFrom()
    {
        InitRenderer();

        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetColor("_Color", From);

        _renderer.SetPropertyBlock(_propBlock);
    }

    [ContextMenu("Assume TO")]
    void AssumeTo()
    {
        InitRenderer();

        _renderer.GetPropertyBlock(_propBlock);

        _propBlock.SetColor("_Color", To);

        _renderer.SetPropertyBlock(_propBlock);
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
