using UnityEngine;

public abstract class ViewBase<T1, TPLD, TViewValue> : MonoBehaviour
    where T1 : AspectAdaptorBase<TPLD, TViewValue>
    where TPLD : IPLDBase
{
    [SerializeField] private T1 _adaptor;

    private void Awake()
    {
        InitAdapter();

        RegisterEvents();
    }

    private void OnDestroy()
    {
        _adaptor.OnValueChanged -= OnAdapterValueChanged;

        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        RegisterEventsCustomActions();
    }

    private void UnregisterEvents()
    {
        UnregisterEventsCustomActions();
    }

    private void InitAdapter()
    {
        _adaptor.OnValueChanged += OnAdapterValueChanged;
    }

    private void OnAdapterValueChanged(TPLD pld)
    {
        ParsePLD(pld);
    }

    protected void CallAdaptor(TViewValue value)
    {
        _adaptor.SetValue(value);
    }

    protected abstract void ParsePLD(TPLD pld);
    protected virtual void RegisterEventsCustomActions() { }
    protected virtual void UnregisterEventsCustomActions() { }
}
