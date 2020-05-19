using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class CatchDebug : MonoBehaviour
    {
        public Text textDebug;
        private void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        public void Log(string message, string stackTrace, LogType type)
        {
            var text = "";
            if (message == null) return;
            switch (type)
            {
                case LogType.Log:
                    text = "<color=\"green\">" + type.ToString() + "</color>";
                    break;
                case LogType.Warning:
                    text = "<color=\"yellow\">" + type.ToString() + "</color>";
                    break;
                case LogType.Error:
                    text = "<color=\"red\">" + type.ToString() + "</color>";
                    break;
                case LogType.Assert:
                    break;
                case LogType.Exception:
                    break;
                default:
                    text = type.ToString();
                    break;
            }

            textDebug.text = text + "-" + message + "\n" + textDebug.text;
        }
    }
}
