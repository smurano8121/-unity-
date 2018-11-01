using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Text;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using MiniJSON;

public class readtext : MonoBehaviour
{
    private string guitxt = "";
    private string aatxt = "";
    int b;

    // Update is called once per frame
    void Update()
    {
        // スペースキーを押したらファイル読み込みする
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReadFile();
        }
    }
    // 読み込んだ情報をGUIとして表示
    void OnGUI()
    {
        GUI.TextArea(new Rect(5, 5, Screen.width, 50), guitxt);
    }

    // 読み込み関数
    void ReadFile()
    {
        // FileReadTest.txtファイルを読み込む
        FileInfo fi = new FileInfo("Assets/Resources/weather.txt");
        try
        {
            // 一行毎読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                guitxt = sr.ReadToEnd();
                Debug.Log(guitxt);
                b = guitxt.Length;
                Debug.Log(b);
                if (b == 6) { Debug.Log("aaa"); }
            }
        }
        catch (Exception e)
        {
            // 改行コード
            guitxt += SetDefaultText();
        }
    }

     string SetDefaultText(){
        return "C#あ\n";
    }
}
