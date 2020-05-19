using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Managers
{
    public class ApiManager : MonoBehaviour
    {
        #region Singleton

        private static ApiManager instance;

        public static ApiManager Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                instance = GameObject.FindGameObjectWithTag("ApiManager").GetComponent<ApiManager>();
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

        private void Awake()
        {
            Singleton();
        }

        public IEnumerator Get(string uri, System.Action<string> callback)
        {
            var www = UnityWebRequest.Get(uri);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                callback(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }

        public IEnumerator Post(string uri, List<IMultipartFormSection> formData)
        {
            var www = UnityWebRequest.Post(uri, formData);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }

        public IEnumerator Post(string uri, string data)
        {
            var www = UnityWebRequest.Post(uri, data);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }

        public IEnumerator Post(string uri, string data, System.Action<string> callback)
        {
            var www = UnityWebRequest.Post(uri, data);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                callback(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }

        public IEnumerator PutPost(string uri, byte[] data, System.Action<string> callback)
        {
            var www = UnityWebRequest.Put(uri, data);
            www.method = UnityWebRequest.kHttpVerbPOST;
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Accept", "application/json");
            www.useHttpContinue = false;

            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                callback(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }

        public IEnumerator Put(string uri, byte[] data, System.Action<string> callback)
        {
            var www = UnityWebRequest.Put(uri, data);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                callback(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }

        public IEnumerator Put(string uri, string data, System.Action<string> callback)
        {
            var www = UnityWebRequest.Put(uri, data);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                callback(www.error);
            }
            else
            {
                callback(www.downloadHandler.text);
            }
        }
    }
}