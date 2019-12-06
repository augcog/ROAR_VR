using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using OpenCvSharp;
using System.Threading;
using System.Threading.Tasks;

public class GstreamerRear : MonoBehaviour
{
    // Start is called before the first frame update
    private Texture2D texture;
    private Texture2D textureleft;
    private Texture2D textureright;
    public Material materialleft;
    public Material materialright;
    private Gstreamer gst;
    public int width,height;
    void Start()
    {
        //texture = new Texture2D(width , height);
        //width = gst.width;
        //height = gst.height;
        texture = new Texture2D(width, height, TextureFormat.RGB24, false, false);
        textureleft = new Texture2D(height, height);
        textureright = new Texture2D(height, height);
        gst = GameObject.Find("Utility").GetComponent<Gstreamer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float t1= Time.time;
        /*Color32[] c = new Color32[width * height];

        //Get the pixel data from parallel loop
        Parallel.For(0, height, i => {
            for (var j = 0; j < width; j++)
            {
                byte vec = gst.frame1[j + i * width];
                var color32 = new Color32
                {
                    r = vec,
                    g = vec,
                    b = vec,
                    a = 0
                };
                c[j + i * width] = color32;
            }
        });

        //Create Texture from the result
        texture = new Texture2D(width, height, TextureFormat.RGBA32, true, true);
        texture.SetPixels32(c);
        texture.Apply();*/
        texture.LoadRawTextureData(gst.frame1);
        texture.Apply();
        //texture.LoadImage(gst.frame1);
        Color[] c1 = texture.GetPixels(0, 0, height, height);
        textureleft.SetPixels(c1);
        textureleft.Apply();
        materialleft.mainTexture = textureleft;
        materialleft.color = Color.white;

        c1 = texture.GetPixels(width-height, 0, height, height);
        textureright.SetPixels(c1);
        textureright.Apply();
        materialright.mainTexture = textureright;
        materialright.color = Color.white;
        //Debug.Log((Time.time - t1)*1000);
    }
}
