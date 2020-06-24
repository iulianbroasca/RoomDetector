using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManagerAdminScene : MonoBehaviour
    {
        [Header("Prefab button"), Header("Rooms")] 
        [SerializeField] private GameObject lineButton;
        [Header("Content for button list")]
        [SerializeField] private GameObject contentRooms;
        [Header("Text top bar")]
        [SerializeField] private Text topBarText;
        
        [Header("Room menu game object")]
        [SerializeField] private GameObject roomMenu;
        [SerializeField] private GameObject cameraButton;
        [SerializeField] private GameObject closeButton;

        [Header("Popup")]
        [SerializeField] private GameObject popup;
        [SerializeField] private GameObject successLine;
        [SerializeField] private GameObject errorLine;
        [SerializeField] private GameObject loader;
        [SerializeField] private GameObject okButton;

        [SerializeField] private GameObject sendImageButton;

        #region Singleton

        private static UIManagerAdminScene instance;

        public static UIManagerAdminScene Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                instance = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManagerAdminScene>();
                return instance;
            }
        }

        public void Singleton()
        {
            if (Instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this);
            }
        }

        #endregion

        private void Awake()
        {
            Singleton();
        }

        /// <summary>
        /// This method is assigned on Send image button
        /// </summary>
        public void SendImage()
        {
            AdminSceneManager.instance.SendImage();
        }

        public void InstantiateRoomList(List<string> rooms)
        {
            foreach (var room in rooms)
            {
                var roomGo = Instantiate(lineButton, contentRooms.transform);
                roomGo.GetComponent<RoomButton>().SetRoomName(room);
            }
        }

        /// <summary>
        /// This method is assigned on Add room button
        /// </summary>
        /// <param name="room"></param>
        public void InstantiateRoom(Text room)
        {
            if (string.IsNullOrEmpty(room.text))
                return;
            var roomGo = Instantiate(lineButton, contentRooms.transform);
            roomGo.GetComponent<RoomButton>().SetRoomName(room.text);
        }

        public void SetTextOnTopBar(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                topBarText.text = value;
            }
        }

        /// <summary>
        /// This method is assigned on close/camera button
        /// </summary>
        public void SetActiveRoomMenu()
        {
            roomMenu.SetActive(!roomMenu.activeSelf);

            cameraButton.SetActive(roomMenu.activeSelf);
            closeButton.SetActive(!roomMenu.activeSelf);
        }

        public void EnablePopup(bool value)
        {
            popup.SetActive(true);
            okButton.SetActive(true);
            loader.SetActive(false);

            successLine.SetActive(value);
            errorLine.SetActive(!value);
        }

        public void EnableLoader()
        {
            popup.SetActive(true);
            loader.SetActive(true);
            okButton.SetActive(false);
            successLine.SetActive(false);
            errorLine.SetActive(false);
        }

        /// <summary>
        /// This method is used on Button from popup (Popup->Elements->ButtonLine->Button)
        /// </summary>
        /// <param name="value"></param>
        public void SetActiveSendImageButton(bool value)
        {
            sendImageButton.SetActive(value);
        }

        /// <summary>
        /// This method is used on Button from popup (Popup->Elements->ButtonLine->Button)
        /// </summary>
        /// <param name="value"></param>
        public void SetActivePopup(bool value)
        {
            popup.SetActive(value);
        }
    }
}
