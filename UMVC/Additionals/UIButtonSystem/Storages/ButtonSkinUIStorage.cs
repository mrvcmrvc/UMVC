using UnityEngine;

namespace UMVC
{
    public class ButtonSkinUIStorage : MonoBehaviour
    {
        private static ButtonSkinUIStorage _instance;
        public static ButtonSkinUIStorage Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<ButtonSkinUIStorage>();

                return _instance;
            }
        }

        public ButtonSkinUIInfoSO SkinInfoSO;

        public ButtonSkinInfo GetButtonSkinInfo(Sprite sprite)
        {
            return SkinInfoSO.ButtonSkinInfoList.Find(i => i.ButtonIdleSkin == sprite);
        }
    }
}
