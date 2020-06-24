using System.Collections.Generic;
using Globals;
using Models;
using UnityEngine;
using Newtonsoft.Json;

namespace Managers
{
    public class AdminSceneManager : MonoBehaviour
    {
        private string currentRoom;
        private List<string> rooms;

        #region Singleton

        public static AdminSceneManager instance;

        public void Singleton()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        #endregion

        private void Awake()
        {
            Singleton();
            GetRooms();
        }

        public void GetRooms()
        {
            StartCoroutine
            (
                ApiManager.Instance.Get
                (
                    Constants.UriGetRooms,
                    (callback) =>
                    {
                        rooms = JsonConvert.DeserializeObject<List<string>>(callback);
                        UIManagerAdminScene.Instance.InstantiateRoomList(rooms);
                    }
                )
            );
        }

        public void SetCurrentRoom(string value)
        {
            currentRoom = value;
        }

        public void SendImage()
        {
            if (currentRoom == null)
            {
                UIManagerAdminScene.Instance.SetTextOnTopBar(Constants.MandatorySelectRoom);
                return;
            }

            UIManagerAdminScene.Instance.EnableLoader();
            UIManagerAdminScene.Instance.SetActiveSendImageButton(false);

            var image = new ImageData(currentRoom, CameraManager.instance.GetJpgTexture());
            StartCoroutine
            (
                    ApiManager.Instance.Put
                    (
                            Constants.UriSendImage,
                            JsonConvert.SerializeObject(image),
                            (callback) =>
                            {
                                UIManagerAdminScene.Instance.EnablePopup(callback == Constants.MessageSuccessServer);
                            }
                    )
            );
        }
    }
}
