namespace Models
{
    public class ImageData
    {

        // ReSharper disable once InconsistentNaming
        public string roomName;
        // ReSharper disable once InconsistentNaming
        public byte[] image;

        public ImageData(string roomName, byte[] image)
        {
            this.roomName = roomName;
            this.image = image;
        }
    }
}
