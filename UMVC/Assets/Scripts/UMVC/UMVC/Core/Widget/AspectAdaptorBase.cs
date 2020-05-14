using System;
using UnityEngine;

namespace UMVC
{
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
            RegisterToModelEvents();
        }

        private void OnDestroy()
        {
            UnregisterFromModelEvents();
        }

        public void SetValue(TViewValue value)
        {
            CallModelOnViewUpdate(value);

            if (_autoNotifyView)
                OnValueUpdated();
        }

        protected void OnValueUpdated()
        {
            GeneratePLD(out _viewValue);

            OnValueChanged?.Invoke(_viewValue);
        }

        protected abstract void GeneratePLD(out TPLD pld);
        protected abstract void RegisterToModelEvents();
        protected abstract void UnregisterFromModelEvents();
        protected abstract void CallModelOnViewUpdate(TViewValue valueForModel);
    }
}
