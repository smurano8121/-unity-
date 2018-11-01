using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class IPCamera : MonoBehaviour
{

    [HideInInspector]
    public Byte[] JpegData;
    [HideInInspector]
    public string resolution = "480x360";

    private Texture2D texture;
    private Stream stream;
    private WebResponse resp;
    public MeshRenderer frame;

    public void StopStream()
    {
        stream.Close();
        resp.Close();
    }

    public void GetVideo(string ip)
    {
        texture = new Texture2D(2, 2);
        // create HTTP request
        resolution = "320x240";
        string url = "http://172.20.11.46/-wvhttp-01-/video.cgi";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Credentials = new NetworkCredential("root", "light");
        // get response
        resp = req.GetResponse();
        // get response stream
        stream = resp.GetResponseStream();
        frame.material.color = Color.white;
        StartCoroutine(GetFrame());
    }

    public IEnumerator GetFrame()
    {
        while (true)
        {
            int bytesToRead = FindLength(stream);
            if (bytesToRead == -1)
            {
                //                print("End of stream");
                yield break;
            }

            int leftToRead = bytesToRead;

            while (leftToRead > 0)
            {
                //                print (leftToRead);
                leftToRead -= stream.Read(JpegData, bytesToRead - leftToRead, leftToRead);
                yield return null;
            }

            MemoryStream ms = new MemoryStream(JpegData, 0, bytesToRead, false, true);

            texture.LoadImage(ms.GetBuffer());
            frame.material.mainTexture = texture;
            frame.material.color = Color.white;
            stream.ReadByte(); // CR after bytes
            stream.ReadByte(); // LF after bytes
        }
    }

    int FindLength(Stream stream)
    {
        int b;
        string line = "";
        int result = -1;
        bool atEOL = false;

        while ((b = stream.ReadByte()) != -1)
        {
            if (b == 10) continue; // ignore LF char
            if (b == 13)
            { // CR
                if (atEOL)
                {  // two blank lines means end of header
                    stream.ReadByte(); // eat last LF
                    return result;
                }
                if (line.StartsWith("Content-Length:"))
                {
                    result = Convert.ToInt32(line.Substring("Content-Length:".Length).Trim());
                }
                else
                {
                    line = "";
                }
                atEOL = true;
            }
            else
            {
                atEOL = false;
                line += (char)b;
            }
        }
        return -1;
    }
}

