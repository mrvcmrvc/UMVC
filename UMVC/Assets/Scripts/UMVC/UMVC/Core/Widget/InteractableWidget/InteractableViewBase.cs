using UnityEngine;

namespace UMVC
{
    public abstract class InteractableViewBase<T1, TPLD, TViewValue> : MonoBehaviour
    where T1 : InteractableAspectAdaptorBase<TPLD, TViewValue>
    where TPLD : IPLDBase
    {
        [SerializeField] private T1 _adaptor;

        private void Awake()
        {
            _adaptor.OnValueChanged += OnAdapterValueChanged;

            RegisterEvents();

            AwakeCustomActions();
        }

        private void OnDestroy()
        {
            _adaptor.OnValueChanged -= OnAdapterValueChanged;

            UnregisterEvents();

            OnDestroyCustomActions();
        }

        private void RegisterEvents()
        {
            RegisterEventsCustomActions();
        }

        private void UnregisterEvents()
        {
            UnregisterEventsCustomActions();
        }

        private void OnAdapterValueChanged(TPLD pld)
        {
            ParsePLD(pld);
        }

        protected void UpdateAdaptor(TViewValue value)
        {
            _adaptor.SetValue(value);
        }

        protected abstract void ParsePLD(TPLD pld);
        protected virtual void RegisterEventsCustomActions() { }
        protected virtual void UnregisterEventsCustomActions() { }
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}