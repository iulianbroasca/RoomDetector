using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManagerUserScene : MonoBehaviour
    {
        #region Singleton

        private static UIManagerUserScene instance;

        public static UIManagerUserScene Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                instance = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManagerUserScene>();
                return instance;
            }
        }

        public void Singleton()
        {
            if (Instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }

        #endregion

        [Header("Top text"), SerializeField]  
        private Text topText;

        private void Awake()
        {
            Singleton();
        }

        public void SetTopText(string value)
        {
            if(string.IsNullOrEmpty(value))
                return;
            topText.text = value;
        }
    }
}
