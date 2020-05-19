namespace Models
{
    public class ImageData
    {
        public string roomName;
        public byte[] image;

        public ImageData(string roomName, byte[] image)
        {
            this.roomName = roomName;
            this.image = image;
        }
    }
}
