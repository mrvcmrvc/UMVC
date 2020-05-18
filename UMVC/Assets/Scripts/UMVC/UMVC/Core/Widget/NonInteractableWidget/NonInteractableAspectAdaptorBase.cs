using System;

namespace UMVC
{
    public abstract class NonInteractableAspectAdaptorBase<TPLD> : AspectAdaptorBase
    where TPLD : IPLDBase
    {
        #region Events
        public Action<TPLD> OnValueChanged;
        #endregion

        private TPLD _viewPLD;

        protected sealed override void OnValueUpdated()
        {
            GeneratePLD(out _viewPLD);

            OnValueChanged?.Invoke(_viewPLD);
        }

        protected abstract void GeneratePLD(out TPLD pld);
    }
}
