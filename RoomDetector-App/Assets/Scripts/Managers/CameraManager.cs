using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Managers
{
    /// <summary>
    /// This component tests getting the latest camera image
    /// and converting it to RGBA format. If successful,
    /// it displays the image on the screen as a RawImage
    /// and also displays information about the image.
    ///
    /// This is useful for computer vision applications where
    /// you need to access the raw pixels from camera image
    /// on the CPU.
    ///
    /// This is different from the ARCameraBackground component, which
    /// efficiently displays the camera image on the screen. If you
    /// just want to blit the camera texture to the screen, use
    /// the ARCameraBackground, or use Graphics.Blit to create
    /// a GPU-friendly RenderTexture.
    ///
    /// In this example, we get the camera image data on the CPU,
    /// convert it to an RGBA format, then display it on the screen
    /// as a RawImage texture to demonstrate it is working.
    /// This is done as an example; do not use this technique simply
    /// to render the camera image on screen.
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        #region Singleton

        public static CameraManager Instance;

        public void Singleton()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        #endregion

        [SerializeField]
        [Tooltip("The ARCameraManager which will produce frame events.")]
        private ARCameraManager arCameraManager;

        /// <summary>
        /// Get or set the <c>ARCameraManager</c>.
        /// </summary>
        public ARCameraManager ArCameraManager
        {
            get => arCameraManager;
            set => arCameraManager = value;
        }

        [SerializeField] private Text imageInfo;

        private Texture2D texture;

        /// <summary>
        /// The UI Text used to display information about the image on screen.
        /// </summary>
        public Text ImageInfo
        {
            get => imageInfo;
            set => imageInfo = value;
        }

        public byte[] GetJpgTexture()
        {
            return texture.EncodeToJPG();
        }

        private void Awake()
        {
            Singleton();
        }

        private void OnEnable()
        {
            if (arCameraManager != null)
            {
                arCameraManager.frameReceived += OnArCameraFrameReceived;
            }
        }

        private void OnDisable()
        {
            if (arCameraManager != null)
            {
                arCameraManager.frameReceived -= OnArCameraFrameReceived;
            }
        }

        private unsafe void OnArCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
        {
            // Attempt to get the latest camera image. If this method succeeds,
            // it acquires a native resource that must be disposed (see below).
            if (!ArCameraManager.TryGetLatestImage(out var image))
            {
                return;
            }

            // Display some information about the camera image
            imageInfo.text = $"Image info:\n\twidth: {image.width}\n\theight: {image.height}\n\tplaneCount: {image.planeCount}\n\ttimestamp: {image.timestamp}\n\tformat: {image.format}";

            // Once we have a valid XRCameraImage, we can access the individual image "planes"
            // (the separate channels in the image). XRCameraImage.GetPlane provides
            // low-overhead access to this data. This could then be passed to a
            // computer vision algorithm. Here, we will convert the camera image
            // to an RGBA texture and draw it on the screen.

            // Choose an RGBA format.
            // See XRCameraImage.FormatSupported for a complete list of supported formats.
            var format = TextureFormat.RGBA32;

            if (texture == null || texture.width != image.width || texture.height != image.height)
            {
                texture = new Texture2D(image.width, image.height, format, false);
            }

            // Convert the image to format, flipping the image across the Y axis.
            // We can also get a sub rectangle, but we'll get the full image here.
            var conversionParams = new XRCameraImageConversionParams(image, format, CameraImageTransformation.MirrorY);

            // Texture2D allows us write directly to the raw texture data
            // This allows us to do the conversion in-place without making any copies.
            var rawTextureData = texture.GetRawTextureData<byte>();
            try
            {
                image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
            finally
            {
                // We must dispose of the XRCameraImage after we're finished
                // with it to avoid leaking native resources.
                image.Dispose();
            }

            // Apply the updated texture data to our texture
            texture.Apply();
        }
    }
}