using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public class QrCodeRecenter : MonoBehaviour
{
    [SerializeField]
    private ARSession _session;
    [SerializeField]
    private ARSessionOrigin _sessionOrigin;
    [SerializeField]
    private ARCameraManager _cameraManager;
    [SerializeField]
    private List<Target> _navigationTargetObjects = new List<Target>();

    private Texture2D _cameraImageTexture;
    private IBarcodeReader _reader = new BarcodeReader();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetQrCodeRecenterTarget("Esport");
            Debug.Log("Space preseed!!");           
        }
    }

    private void OnEnable()
    {
        _cameraManager.frameReceived += OnCameraFrameReceived;
    }

    private void OnDisable()
    {
        _cameraManager.frameReceived -= OnCameraFrameReceived;

    }

    private void OnCameraFrameReceived(ARCameraFrameEventArgs eventArgs)
    {
        if (!_cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            return;
        }

        var conversionParams = new XRCpuImage.ConversionParams
        {
            // Get the entire image.
            inputRect = new RectInt(0, 0, image.width, image.height),

            // Downsample by 2.
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

            // Choose RGBA format.
            outputFormat = TextureFormat.RGBA32,

            // Flip across the vertical axis (mirror image).
            transformation = XRCpuImage.Transformation.MirrorY
        };

        // See how many bytes you need to store the final image.
        int size = image.GetConvertedDataSize(conversionParams);

        // Allocate a buffer to store the image.
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        // Extract the image data
        image.Convert(conversionParams, buffer);

        // The image was converted to RGBA32 format and written into the provided buffer
        // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
        image.Dispose();

        // At this point, you can process the image, pass it to a computer vision algorithm, etc.
        // In this example, you apply it to a texture to visualize it.

        // You've got the data; let's put it into a texture so you can visualize it.
        _cameraImageTexture = new Texture2D(
            conversionParams.outputDimensions.x,
            conversionParams.outputDimensions.y,
            conversionParams.outputFormat,
            false);

        _cameraImageTexture.LoadRawTextureData(buffer);
        _cameraImageTexture.Apply();

        // Done with your temporary data, so you can dispose it.
        buffer.Dispose();

        // Detect and decode the barcode inside the bitmap
        var result = _reader.Decode(_cameraImageTexture.GetPixels32(), _cameraImageTexture.width, _cameraImageTexture.height);

        // Do something with the result
        if (result != null)
        {
            SetQrCodeRecenterTarget(result.Text);
        }

    }

    private void SetQrCodeRecenterTarget(string _targetText)
    {
        Target _currentTarget = _navigationTargetObjects.Find(x => x.Name.ToLower().Equals(_targetText.ToLower()));
        if (_currentTarget != null)
        {
            // Reset position and rotation of ARSession
            _session.Reset();

            // Add offset for recentering
            _sessionOrigin.transform.position = _currentTarget.PositionObject.transform.position;
            _sessionOrigin.transform.rotation = _currentTarget.PositionObject.transform.rotation;
        }
    }
}
