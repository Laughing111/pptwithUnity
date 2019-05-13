using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.Common;

public class QRManager : MonoBehaviour {

    public delegate void QRScanFinished(string str);
    public event QRScanFinished e_QRScanFinished;
    bool decoding = false;
    string dataText = null;
    private Color32[] orginalc;
    private byte[] targetbyte;
    private int W, H, WxH;
    int z = 0;
    private Texture2D texTemp;
    private BarcodeReader codeReader;
    private static QRManager qRManager;
    private QRManager() { }
    public static QRManager Ins
    {
        get
        {
            if (qRManager == null)
            {
                GameObject temp = GameObject.Find("QRManager");
                if (temp == null)
                {
                    temp = new GameObject("QRManager");
                    temp.AddComponent<QRManager>();
                }
                qRManager = temp.GetComponent<QRManager>();
            }
            return qRManager;
        }
    }

    IEnumerator Decode(WebCamTexture te)
    {
        while (true)
        {
            if (decoding && te != null)
            {
                orginalc = te.GetPixels32();
                z = 0;
                // convert the image color data
                for (int y = H - 1; y >= 0; y--)
                {
                    for (int x = 0; x < W; x++)
                    {

                        targetbyte[z++] = (byte)(((int)orginalc[y * W + x].r) << 16 | ((int)orginalc[y * W + x].g) << 8 | ((int)orginalc[y * W + x].b));
                    }
                }
                //RGBLuminanceSource luminancesource = new RGBLuminanceSource(targetbyte, W, H, RGBLuminanceSource.BitmapFormat.Gray8);
                PlanarYUVLuminanceSource planarYUV = new PlanarYUVLuminanceSource(targetbyte, W, H, 0, 0, W, H, false);
                var bitmap = new BinaryBitmap(new HybridBinarizer(planarYUV));
                Result data;
                var reader = new MultiFormatReader();
                data = reader.decode(bitmap);
                if (data != null)
                {
                    {
                        decoding = false;
                        dataText = data.Text;
                        Debug.Log(dataText + e_QRScanFinished.ToString());
                        e_QRScanFinished(dataText);
                    }
                }
                else
                {

                    yield return new WaitForEndOfFrame();
                    Color[] color = te.GetPixels();
                    float res = 0;
                    for (int i = 0; i < color.Length; i++)
                    {
                        res += (color[i].r + color[i].g + color[i].b) / 3;
                    }
                    res /= color.Length;
                    for (int i = 0; i < color.Length; i++)
                    {
                        if ((color[i].r + color[i].g + color[i].b) / 3 < res)
                        {
                            color[i] = Color.black;
                        }
                        else
                        {
                            color[i] = Color.white;
                        }
                    }
                    texTemp.SetPixels(color);
                    texTemp.Apply();
                    data = DecodeByStaticPic(texTemp);
                    if (data != null)
                    {
                        {
                            decoding = false;
                            dataText = data.Text;
                            Debug.Log(dataText);
                            e_QRScanFinished(dataText);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

    }

    /// <summary>
    /// QRmanager 解析二维码，扫码成功后停止扫码
    /// </summary>
    /// <param name="te">摄像机材质</param>
    /// <param name="finishedMethod">扫码成功后的方法</param>
    public void StartDecode(WebCamTexture te, QRScanFinished finishedMethod)
    {
        decoding = true;
        codeReader = new BarcodeReader();
        W = te.width;
        H = te.height;
        WxH = W * H;
        targetbyte = new byte[WxH];
        texTemp = new Texture2D(W, H);
        e_QRScanFinished += finishedMethod;
        StartCoroutine(Decode(te));
    }


    /// <summary>
    /// 恢复扫码
    /// </summary>
    public void ReStartDecode()
    {
        decoding = true;
    }

    public void StopDecode()
    {
        decoding = false;
        StopAllCoroutines();
    }

    public Result DecodeByStaticPic(Texture2D tex)
    {
        Result data = codeReader.Decode(tex.GetPixels32(), tex.width, tex.height);
        return data;
    }

    /// <summary>
    /// 生成二维码
    /// </summary>
    /// <param name="textForEncoding">需要写入的字符串</param>
    /// <param name="width">二维码宽度</param>
    /// <param name="height">二维码高度</param>
    /// <returns></returns>
    public static Texture2D Encode(string textForEncoding, int width, int height)
    {
        BitMatrix matrix = new MultiFormatWriter().encode(textForEncoding, BarcodeFormat.QR_CODE, width, height);
        //注意千万不能使用 writer，因为这个组件除了256 大小能正常输出以外，其他都不支持。
        Texture2D te = new Texture2D(width, height);
        // 下面这里按照二维码的算法，逐个生成二维码的图片， 
        // 两个for循环是图片横列扫描的结果 
        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (matrix[x, y])
                {
                    te.SetPixel(y, x, Color.black);// 0xff000000;
                }
                else
                {
                    te.SetPixel(y, x, Color.white);// 0xffffffff;
                }
            }
        }
        te.Apply();
        return te;
    }

}
