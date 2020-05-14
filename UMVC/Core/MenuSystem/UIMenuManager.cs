using System;
using System.Collections.Generic;
using System.Linq;
using UMVC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UMVC
{
    public class UIMenuManager : MonoBehaviour
    {
        #region Events
        public Action<UIMenu> OnUIMenuInitCompleted;
        #endregion

        private static UIMenuManager _instance;
        public static UIMenuManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<UIMenuManager>();

                return _instance;
            }
        }

        [SerializeField] private bool _toggleDebug;

        public List<UIMenu> UIMenuList { get; private set; }
        public List<UIMenu> ActiveUIMenuColl { get; private set; }

        private List<UIMenu> _closeMenuColl = new List<UIMenu>();
        private bool _isDeactivationFinished;
        private UIMenu _nextOpeningMenu;

        private void Awake()
        {
            ActiveUIMenuColl = new List<UIMenu>();

            _isDeactivationFinished = true;

            UIMenu.OnPostDeactivation += UIMenuClosed;
            UIMenu.OnUIMenuInitCompleted += OnNewUIInitCompleted;
            UIMenu.OnUIMenuDestroyed += OnUIMenuDestroyed;

            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDestroy()
        {
            UIMenu.OnPostDeactivation -= UIMenuClosed;
            UIMenu.OnUIMenuInitCompleted -= OnNewUIInitCompleted;
            UIMenu.OnUIMenuDestroyed -= OnUIMenuDestroyed;

            SceneManager.sceneUnloaded -= OnSceneUnloaded;

            _instance = null;
        }

        public void OnBackPressed()
        {
            if (ActiveUIMenuColl.Count > 0)
                CloseUIMenu(ActiveUIMenuColl[ActiveUIMenuColl.Count - 1]);
        }

        private void OnSceneUnloaded(Scene unloadedScene)
        {
            if (_toggleDebug)
                Debug.Log("Scene Unloaded, Removing All Registered Windows!");

            ActiveUIMenuColl.Clear();

            _isDeactivationFinished = true;
        }

        public void OpenUIMenu(UIMenu menuRequestedToActivate)
        {
            if (ActiveUIMenuColl.Contains(menuRequestedToActivate))
                return;

            if (ActiveUIMenuColl.Count > 0)
            {
                if (menuRequestedToActivate.DisableMenusUnderneath)
                {
                    foreach (var underneathMenu in ActiveUIMenuColl)
                    {
                        if (underneathMenu != menuRequestedToActivate)
                            _closeMenuColl.Add(underneathMenu);

                        if (underneathMenu.DisableMenusUnderneath)
                            break;
                    }
                }

                menuRequestedToActivate.transform.SetAsLastSibling();
            }

            _nextOpeningMenu = menuRequestedToActivate;

            if (!_isDeactivationFinished)
                return;

            StartCloseMenu(null);
        }

        private void UIMenuClosed(UIMenu closedMenu)
        {
            StartCloseMenu(null);
        }

        private void StartCloseMenu(Action callback)
        {
            if (_closeMenuColl.Count == 0)
            {
                callback?.Invoke();

                if (_nextOpeningMenu == null)
                {
                    foreach (var menu in ActiveUIMenuColl)
                    {
                        if (!menu.IsPreActivationFinished)
                            menu.Activate();

                        if (menu.DisableMenusUnderneath)
                            break;
                    }
                }
                else
                {
                    if (!_nextOpeningMenu.IsPreActivationFinished)
                    {
                        ActiveUIMenuColl.Add(_nextOpeningMenu);

                        _nextOpeningMenu.Activate();
                    }

                    _nextOpeningMenu = null;
                }

                _isDeactivationFinished = true;

                return;
            }

            _isDeactivationFinished = false;

            var instance = _closeMenuColl[0];
            _closeMenuColl.RemoveAt(0);
            if (!instance.IsPreDeactivationFinished)
                instance.Deactivate();
        }

        public void CloseUIMenu(UIMenu menuRequestedToDeactivate)
        {
            if (_toggleDebug)
                Debug.Log("Trying to Close Window: " + menuRequestedToDeactivate.GetType());

            if (ActiveUIMenuColl.Count == 0)
            {
                Debug.LogWarningFormat(menuRequestedToDeactivate, "{0} cannot be closed because menu list is empty", menuRequestedToDeactivate.GetType());
                return;
            }

            CloseMenu(menuRequestedToDeactivate);
        }

        public void CloseMenu(UIMenu instance, Action callback = null)
        {
            ActiveUIMenuColl.Remove(instance);

            if (_toggleDebug)
                Debug.Log("Close Top Menu called, closing window: " + instance.GetType());

            if (_closeMenuColl.Contains(instance))
            {
                Debug.LogWarningFormat("Close Menu list already contains window: " + instance.GetType());
                return;
            }

            _closeMenuColl.Add(instance);

            if (_closeMenuColl.Count >= 2)
                return;

            StartCloseMenu(callback);
        }

        public void CloseTopMenu(Action callback = null)
        {
            var instance = ActiveUIMenuColl[ActiveUIMenuColl.Count - 1];

            ActiveUIMenuColl.RemoveAt(ActiveUIMenuColl.Count - 1);

            if (_toggleDebug)
                Debug.Log("Close Top Menu called, closing window: " + instance.GetType());

            if (_closeMenuColl.Contains(instance))
            {
                Debug.LogWarningFormat("Close Menu list already contains window: " + instance.GetType());
                return;
            }

            _closeMenuColl.Add(instance);

            if (_closeMenuColl.Count >= 2)
                return;

            StartCloseMenu(callback);
        }

        public void CloseAllUIMenus(Action callback)
        {
            if (ActiveUIMenuColl.Count == 0)
                callback?.Invoke();

            for (int i = ActiveUIMenuColl.Count - 1; i >= 0; i--)
                CloseTopMenu(callback);
        }

        public void CloseAllUIMenusExcept(Type menuType, Action callback)
        {
            if (ActiveUIMenuColl.Count == 0)
                callback?.Invoke();

            for (int i = ActiveUIMenuColl.Count - 1; i >= 0; i--)
            {
                if (ActiveUIMenuColl[ActiveUIMenuColl.Count - 1].GetType().Equals(menuType))
                    break;

                CloseTopMenu(callback);
            }
        }

        public T GetOpenMenu<T>()
            where T : UIMenu
        {
            return (T)ActiveUIMenuColl.FirstOrDefault(val => val is T);
        }

        public T GetUIMenu<T>()
            where T : UIMenu
        {
            T target = (T)UIMenuList.FirstOrDefault(val => val is T);

            if (target == null)
                target = FindObjectOfType<T>();

            return target;
        }

        public bool IsUIActive<T>()
            where T : UIMenu
        {
            return GetOpenMenu<T>() != null;
        }

        public bool IsAnyUIActive()
        {
            return ActiveUIMenuColl != null && ActiveUIMenuColl.Count > 0;
        }

        public bool IsAnyUIActive(params Type[] menuTypeCollection)
        {
            foreach (Type type in menuTypeCollection)
            {
                UIMenu targetUI = ActiveUIMenuColl.FirstOrDefault(val => val.GetType().Equals(type));

                if (targetUI != null)
                    return true;
            }

            return false;
        }

        public bool IsAnyUIActive(out List<UIMenu> activeUIMenuCollection, params Type[] menuTypeCollection)
        {
            activeUIMenuCollection = new List<UIMenu>();

            foreach (Type type in menuTypeCollection)
            {
                UIMenu targetUI = ActiveUIMenuColl.FirstOrDefault(val => val.GetType().Equals(type));

                if (targetUI != null)
                    activeUIMenuCollection.Add(targetUI);
            }

            return activeUIMenuCollection.Count > 0;
        }

        private void OnNewUIInitCompleted(UIMenu uiMenu)
        {
            if (UIMenuList == null)
                UIMenuList = new List<UIMenu>();

            UIMenuList.Add(uiMenu);

            OnUIMenuInitCompleted?.Invoke(uiMenu);
        }

        private void OnUIMenuDestroyed(UIMenu uiMenu)
        {
            if (UIMenuList == null)
                return;

            UIMenuList.Remove(uiMenu);
        }
    }
}