using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManagerMainScene : MonoBehaviour
    {
        public void LoadScene(int number)
        {
            SceneManager.LoadScene(number);
        }
    }
}
