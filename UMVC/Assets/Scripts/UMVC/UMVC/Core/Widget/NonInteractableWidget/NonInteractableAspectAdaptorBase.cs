using System;
using UnityEngine;

namespace UMVC
{
    public abstract class NonInteractableAspectAdaptorBase<TPLD> : MonoBehaviour
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

        protected void OnValueUpdated()
        {
            GeneratePLD(out _viewPLD);

            OnValueChanged?.Invoke(_viewPLD);
        }

        protected abstract void GeneratePLD(out TPLD pld);
        protected abstract void RegisterToModelEvents();
        protected abstract void UnregisterFromModelEvents();
    }
}
