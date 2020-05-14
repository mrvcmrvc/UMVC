using System;
using UnityEngine;

public abstract class AspectAdaptorBase<TPLD, TViewValue> : MonoBehaviour
    where TPLD : IPLDBase
{
    #region Events
    public Action<TPLD> OnValueChanged;
    #endregion

    [SerializeField] private bool _autoNotifyView;

    private TPLD _viewValue;

    private void Awake()
    {
        RegisterEvents();
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    public void SetValue(TViewValue value)
    {
        CallModelOnViewUpdate(value);

        if (_autoNotifyView)
            GeneratePLDAndNotifyView();
    }

    protected void GeneratePLDAndNotifyView()
    {
        GeneratePLD(out _viewValue);

        OnValueChanged?.Invoke(_viewValue);
    }

    protected abstract void GeneratePLD(out TPLD pld);
    protected abstract void RegisterEvents();
    protected abstract void UnregisterEvents();
    protected abstract void CallModelOnViewUpdate(TViewValue valueForModel);
}
