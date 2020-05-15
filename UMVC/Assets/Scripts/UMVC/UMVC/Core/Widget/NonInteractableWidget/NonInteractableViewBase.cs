using UnityEngine;

namespace UMVC
{
    public abstract class NonInteractableViewBase<T1, TPLD> : MonoBehaviour
    where T1 : NonInteractableAspectAdaptorBase<TPLD>
    where TPLD : IPLDBase
    {
        [SerializeField] private T1 _adaptor;

        private void Awake()
        {
            _adaptor.OnValueChanged += OnAdapterValueChanged;

            AwakeCustomActions();
        }

        private void OnDestroy()
        {
            _adaptor.OnValueChanged -= OnAdapterValueChanged;

            OnDestroyCustomActions();
        }

        private void OnAdapterValueChanged(TPLD pld)
        {
            ParsePLD(pld);
        }

        protected abstract void ParsePLD(TPLD pld);
        protected virtual void AwakeCustomActions() { }
        protected virtual void OnDestroyCustomActions() { }
    }
}