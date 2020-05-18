using System;

namespace UMVC
{
    public abstract class InteractableAspectAdaptorBase<TPLD, TViewValue> : AspectAdaptorBase
    where TPLD : IPLDBase
    {
        #region Events
        public Action<TPLD> OnValueChanged;
        #endregion

        private TPLD _viewPLD;

        public void SetValue(TViewValue value)
        {
            OnViewUpdateRequested(value);
        }

        protected sealed override void OnValueUpdated()
        {
            GeneratePLD(out _viewPLD);

            OnValueChanged?.Invoke(_viewPLD);
        }

        protected abstract void GeneratePLD(out TPLD pld);
        protected abstract void OnViewUpdateRequested(TViewValue valueForModel);
    }
}
