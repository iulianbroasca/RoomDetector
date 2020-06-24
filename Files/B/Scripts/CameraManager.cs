using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Singleton

        public static CameraManager instance;

        public void Singleton()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(this);
        }

        #endregion

        [SerializeField]
        private ARCameraManager arCameraManager;

        public ARCameraManager ArCameraManager
        {
            get => arCameraManager;
            set => arCameraManager = value;
        }

        private Texture2D texture;

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
            if (!ArCameraManager.TryGetLatestImage(out var image))
            {
                return;
            }

            var format = TextureFormat.RGBA32;

            if (texture == null || texture.width != image.width || texture.height != image.height)
            {
                texture = new Texture2D(image.width, image.height, format, false);
            }

            var conversionParams = new XRCameraImageConversionParams(image, format, CameraImageTransformation.MirrorY);
            var rawTextureData = texture.GetRawTextureData<byte>();

            try
            {
                image.Convert(conversionParams, new IntPtr(rawTextureData.GetUnsafePtr()), rawTextureData.Length);
            }
            finally
            {
                image.Dispose();
            }

            texture.Apply();
        }
    }
}