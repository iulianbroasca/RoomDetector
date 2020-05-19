using Globals;
using Managers;
using Models;
using Newtonsoft.Json;
using UnityEngine;

namespace Utils
{
    // ReSharper disable once InconsistentNaming
    public class CallAPI : MonoBehaviour
    {
        //Script for testing

        private string result;
        public Texture2D texture;

        [ContextMenu("Upload")]
        public void CallUpload()
        {
            var image = new ImageData("roomName", texture.EncodeToJPG());
            StartCoroutine(
                ApiManager.Instance.Put(
                    Constants.UriSendImage,
                    JsonConvert.SerializeObject(image),
                    (callback) =>
                    {
                        result = callback;
                        Debug.Log(result);
                    })
            );
        }

        [ContextMenu("Upload2")]
        public void CallUpload2()
        {
            var image = texture.EncodeToJPG();
            //File.WriteAllBytes(Path.Combine(Application.dataPath,"PICTURE.JPG"), textu);
            //string text = null;
            //foreach(var x in textu)
            //{
            //    text += x.ToString();
            //}
            //Debug.Log(text);
            result = null;
            StartCoroutine(ApiManager.Instance.Put(
                Constants.UriDetection,image, 
                (callback) =>
                {
                    result = callback;
                    Print(result);
                })
            );
        }

        private static void Print(string result)
        {
            Debug.Log(result);
        }
    }
}
