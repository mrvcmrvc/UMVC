using System;
using UnityEngine;

namespace UMVC
{
    public abstract class InteractableAspectAdaptorBase<TPLD, TViewValue> : MonoBehaviour
    where TPLD : IPLDBase
    {
        #region Events
        public Action<TPLD> OnValueChanged;
        #endregion

        [SerializeField] private bool _updateValueOnEnable;

        private TPLD _viewPLD;

        private void Awake()
        {
            RegisterToModelEvents();
        }

        private void OnDestroy()
        {
            UnregisterFromModelEvents();
        }

        private void OnEnable()
        {
            if (_updateValueOnEnable)
                OnValueUpdated();
        }

        public void SetValue(TViewValue value)
        {
            OnViewUpdateRequested(value);
        }

        protected void OnValueUpdated()
        {
            GeneratePLD(out _viewPLD);

            OnValueChanged?.Invoke(_viewPLD);
        }

        protected abstract void GeneratePLD(out TPLD pld);
        protected abstract void RegisterToModelEvents();
        protected abstract void UnregisterFromModelEvents();
        protected abstract void OnViewUpdateRequested(TViewValue valueForModel);
    }
}
