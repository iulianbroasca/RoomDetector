using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.Networking;

namespace Managers
{
    public class UserSceneManager : MonoBehaviour
    {
        private string isContentReady;
        private void Awake()
        {
            StartCoroutine(ApiManager.Instance.Get(Constants.UriPrepareContentServer, SetIsContentReady));
        }

        private IEnumerator Start()
        {
            // Wait until the content is loaded on server
            yield return new WaitUntil(()=>isContentReady == "Success");

            // Wait a second before sending the first frame
            yield return new WaitForSeconds(1);

            while (true)
            {
                var www = UnityWebRequest.Put(Constants.UriDetection, CameraManager.Instance.GetJpgTexture());
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    UIManagerUserScene.Instance.SetTopText(www.downloadHandler.text);
                }
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void SetIsContentReady(string value)
        {
            isContentReady = value;
        }
    }
}
