using Globals;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RoomButton : MonoBehaviour
    {
        public Text text;
        private string roomName;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(SetCurrentRoomInApp);
        }

        public string GetRoomName()
        {
            return roomName;
        }

        public void SetRoomName(string value)
        {
            roomName = value;
            text.text = value;
        }

        public void SetCurrentRoomInApp()
        {
            AdminSceneManager.instance.SetCurrentRoom(GetRoomName());
            UIManagerAdminScene.Instance.SetTextOnTopBar(Constants.CurrentRoomText + GetRoomName());
        }
    }
}
