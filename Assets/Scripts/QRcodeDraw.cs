using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class QRcodeDraw : MonoBehaviour
{
    public static Texture2D encoded;
    //指定字符串    
    public RawImage QRImage;
    public GameObject tips_Obj;
    public static bool updated = false;
    float getTime;

    public static QRcodeDraw instance;

    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (updated)
            return;
        getTime += Time.deltaTime;
        if (getTime >= 5)
        {
            getTime = 0;
            //#if VER_LYY
            LeYaoYao.SendCmd_GetQrCode();
            //#endif
        }
    }
    public void Update_QrCode(string url)
    {
        updated = true;
        ShowCode(url);
    }

    //定义方法生成二维码
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }

    public void ShowCode(string textForEncoding)
    {
        encoded = new Texture2D(256, 256);
        if (textForEncoding.Length > 0)
        {
            //二维码写入图片
            var color32 = Encode(textForEncoding, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            //重新赋值一张图，计算大小,避免白色边框过大
            Texture2D encoded1;
            encoded1 = new Texture2D(192, 192);//创建目标图片大小
            encoded1.SetPixels(encoded.GetPixels(32, 32, 192, 192));
            encoded1.Apply();
            QRImage.texture = encoded1;
        }
    }
}

