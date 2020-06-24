namespace Globals
{
    public static class Constants
    {
        /// <summary>
        /// AdminScene
        /// </summary>
        public const string UriGetRooms = Host + ":" + Port + RouteGetRooms;
        public const string UriSendImage = Host + ":" + Port + RouteSendImage;

        /// <summary>
        /// UserScene
        /// </summary>
        public const string UriPrepareContentServer = Host + ":" + Port + RoutePrepareContentServer;
        public const string UriDetection = Host + ":" + Port + RouteDetection;

        /// <summary>
        /// UI
        /// </summary>
        public const string MandatorySelectRoom = "Please select a room";
        public const string CurrentRoomText = "Current room: ";
        public const string MessageSuccessServer = "Success";

        private const string Host = "http://192.168.100.5";
        private const string Port = "5000";
        private const string RouteGetRooms = "/admin";
        private const string RoutePrepareContentServer = "/user";
        private const string RouteSendImage = "/send";
        private const string RouteDetection = "/detection";
    }

}