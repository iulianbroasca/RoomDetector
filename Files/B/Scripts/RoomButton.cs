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

        public void SetRoomName(string value)
        {
            roomName = value;
            text.text = value;
        }

        private string GetRoomName()
        {
            return roomName;
        }

        private void SetCurrentRoomInApp()
        {
            AdminSceneManager.instance.SetCurrentRoom(GetRoomName());
            UIManagerAdminScene.Instance.SetTextOnTopBar(Constants.CurrentRoomText + GetRoomName());
        }
    }
}
