using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.SceneManagement;

public class WebcamTexture : MonoBehaviour {

    public int Width = 1920;
    public int Height = 1080;
    public int FPS = 30;
    // public WebCamTexture webcamTexture;
    public Texture2D texture;

    void Start()
    {

    }

    // 画像をビット列に変換するやつ
    byte[] LoadBytes(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryReader bin = new BinaryReader(fs);
        byte[] result = bin.ReadBytes((int)bin.BaseStream.Length);
        bin.Close();
        return result;
    }

    private void Update()
    {
		if (Input.GetKeyDown(KeyCode.A))
		{
			texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("C://VirtualWindowAssets//画像//pic_00.jpg"));
			GetComponent<Renderer>().material.mainTexture = texture;
            Debug.Log("A");
		}
        if (Input.GetKeyDown(KeyCode.B))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("C://Virtualwindow//pic_01.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_02.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_03.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_04.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_05.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_06.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_07.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_08.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_09.jpg"));
            GetComponent<Renderer>().material.mainTexture = texture;
        }
		if (Input.GetKeyDown(KeyCode.KeypadPlus))
		{
			texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_10.jpg"));
			GetComponent<Renderer>().material.mainTexture = texture;
		}
		if (Input.GetKeyDown(KeyCode.KeypadMinus))
		{
			texture = new Texture2D(0, 0);
			texture.LoadImage(LoadBytes("M://VirtualWindowAssets//画像//pic_11.jpg"));
			GetComponent<Renderer>().material.mainTexture = texture;
		}
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMovie");
        }
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
    private static extern short GetKeyState(int keyCode);


    [DllImport("user32.dll")]
    private static extern int GetKeyboardState(byte[] lpKeyState);


    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);


    private const byte VK_NUMLOCK = 0x90; private const uint KEYEVENTF_EXTENDEDKEY = 1; private const int KEYEVENTF_KEYUP = 0x2; private const int KEYEVENTF_KEYDOWN = 0x0;

    public bool GetNumLock()
    {
        return (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
    }

    public void SetNumLock(bool bState) {
        if (GetNumLock() != bState) {
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0);
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}
