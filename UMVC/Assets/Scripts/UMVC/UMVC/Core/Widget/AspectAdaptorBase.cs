using UnityEngine;

public abstract class AspectAdaptorBase : MonoBehaviour
{
    private void Awake()
    {
        RegisterToModelEvents();
    }

    private void OnDestroy()
    {
        UnregisterFromModelEvents();
    }

    public void UpdateViewManually()
    {
        OnValueUpdated();
    }

    protected abstract void OnValueUpdated();
    protected abstract void RegisterToModelEvents();
    protected abstract void UnregisterFromModelEvents();
}
