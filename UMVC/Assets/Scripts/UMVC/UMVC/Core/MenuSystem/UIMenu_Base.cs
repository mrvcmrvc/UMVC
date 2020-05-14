namespace UMVC
{
    public abstract class UIMenu<T> : UIMenu where T : UIMenu<T>
    {
        protected sealed override void ActivateUI()
        {
            bool isUIActive = UIMenuManager.Instance.IsUIActive<T>();
            if (isUIActive)
                return;

            UIMenuManager.Instance.OpenUIMenu(this);
        }
    }
}
