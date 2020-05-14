using System;
using System.Collections;
using UnityEngine;

public abstract partial class UIMenu : MonoBehaviour
{
    #region Events
    public static Action<UIMenu> OnUIMenuInitCompleted;
    public static Action<UIMenu> OnUIMenuDestroyed;
    #endregion

    [SerializeField] private bool _activateInAwake;
    [SerializeField] private bool _disableMenusUnderneath;
    [SerializeField] private UIAnimSequence _uiMenuAnimation;

    public bool DisableMenusUnderneath { get { return _disableMenusUnderneath; } }
    public bool IsPreActivationFinished { get; protected set; }
    public bool IsPostActivationFinished { get; protected set; }
    public bool IsPreDeactivationFinished { get; protected set; }
    public bool IsPostDeactivationFinished { get; protected set; }

    public static Action<UIMenu> OnPreActivation, OnPostActivation, OnPreDeactivation, OnPostDeactivation;

    private IEnumerator _deactivateRoutine, _activateRoutine;

    protected virtual IEnumerator PreDeactivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostDeactivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PreActivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual IEnumerator PostActivateAdditional()
    {
        yield return new WaitForEndOfFrame();
    }

    protected virtual void Awake()
    {
        Init();

        if (_activateInAwake)
            Activate();
    }

    protected virtual void OnDestroy()
    {
        OnUIMenuDestroyed?.Invoke(this);
    }

    protected void Init()
    {
        gameObject.SetActive(false);

        IsPreDeactivationFinished = true;
        IsPostDeactivationFinished = true;

        IsPreActivationFinished = false;
        IsPostActivationFinished = false;

        OnUIMenuInitCompleted?.Invoke(this);
    }

    #region Activation / Deactivation
    public virtual void Activate()
    {
        gameObject.SetActive(true);

        if (_deactivateRoutine != null)
            StopCoroutine(_deactivateRoutine);

        if (_activateRoutine != null)
            StopCoroutine(_activateRoutine);

        _activateRoutine = ActivateRoutine();
        StartCoroutine(_activateRoutine);
    }

    IEnumerator ActivateRoutine()
    {
        IsPreDeactivationFinished = false;
        IsPostDeactivationFinished = false;

        yield return StartCoroutine(PreActivateAdditional());
        IsPreActivationFinished = true;

        OnPreActivation?.Invoke(this);

        if (_uiMenuAnimation != null)
        {
            _uiMenuAnimation.ResetSequence();

            _uiMenuAnimation.PlayIntroSequence(OnMenuIntroAnimFinished);
        }
        else
            OnMenuIntroAnimFinished();
    }

    private void OnMenuIntroAnimFinished()
    {
        StartCoroutine(PostActivateAdditional());

        IsPostActivationFinished = true;

        OnPostActivation?.Invoke(this);
    }

    public virtual void Deactivate()
    {
        if (_activateRoutine != null)
            StopCoroutine(_activateRoutine);

        if (_deactivateRoutine != null)
            StopCoroutine(_deactivateRoutine);

        _deactivateRoutine = DeactivateRoutine();
        StartCoroutine(_deactivateRoutine);
    }

    IEnumerator DeactivateRoutine()
    {
        IsPreActivationFinished = false;
        IsPostActivationFinished = false;

        yield return StartCoroutine(PreDeactivateAdditional());
        IsPreDeactivationFinished = true;

        OnPreDeactivation?.Invoke(this);

        if (_uiMenuAnimation != null)
            _uiMenuAnimation.PlayOutroSequence(OnMenuOutroAnimFinished);
        else
            OnMenuOutroAnimFinished();
    }

    private void OnMenuOutroAnimFinished()
    {
        if (_uiMenuAnimation != null)
            _uiMenuAnimation.ResetSequence();

        StartCoroutine(PostDeactivateAdditional());
        IsPostDeactivationFinished = true;

        gameObject.SetActive(false);

        OnPostDeactivation?.Invoke(this);
    }
    #endregion
}
