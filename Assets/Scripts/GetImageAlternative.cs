using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using ZXing;

public class GetImageAlternative : MonoBehaviour
{
    [SerializeField]
    private ARCameraBackground _arCameraBackground;
    [SerializeField]
    private RenderTexture _targetRenderTexture;
    [SerializeField]
    private TextMeshProUGUI _qrCodeText;

    private Texture2D _cameraImageTexture;
    private IBarcodeReader _reader = new BarcodeReader();

    // Update is called once per frame
    void Update()
    {
        Graphics.Blit(null, _targetRenderTexture, _arCameraBackground.material);
        _cameraImageTexture = new Texture2D(_targetRenderTexture.width, _targetRenderTexture.height, TextureFormat.RGBA32, false);
        Graphics.CopyTexture(_targetRenderTexture, _cameraImageTexture);

        //Detect and decode thae barcode inside the bitmap
        var result = _reader.Decode(_cameraImageTexture.GetPixels32(), _cameraImageTexture.width, _cameraImageTexture.height);

        if (result != null)
        {
            _qrCodeText.text = result.Text;
        }
    }
}
