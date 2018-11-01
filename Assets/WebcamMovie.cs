using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.IO;
using UnityEngine.Video;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Net;

public class WebcamMovie : MonoBehaviour {
    public int Width = 1920;
    public int Height = 1080;
    public int FPS = 30;
    public int volume = 10;
    public Texture2D texture;
    [SerializeField]
    public VideoPlayer videoPlayer;
    public AudioSource audioPlayer;
    public ushort audioTrackNum = 0;
    public ushort audioTrackCount = 1;
    // Use this for initialization
    void Start()
    {
        videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
        audioPlayer = gameObject.AddComponent<AudioSource>();
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.controlledAudioTrackCount = audioTrackCount;
        videoPlayer.EnableAudioTrack(audioTrackNum, true);
        videoPlayer.SetTargetAudioSource(audioTrackNum, audioPlayer);
        GetComponent<Renderer>().material.mainTexture = videoPlayer.texture;
        videoPlayer.Play();
        videoPlayer.isLooping = true;
    }

    //画像のフレームレート・解像度を決める
    byte[] LoadBytes(string path)
    {
        FileStream fs = new FileStream(path, FileMode.Open);
        BinaryReader bin = new BinaryReader(fs);
        byte[] result = bin.ReadBytes((int)bin.BaseStream.Length);
        bin.Close();
        return result;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D)){
            if(volume >= 8) { volume -= 1; }
            if (volume == 9) { audioPlayer.volume = 0.5f; }
            else if(volume == 8){ audioPlayer.volume = 0.1f; }
            else if (volume == 7) { audioPlayer.volume = 0.0f; }
            Debug.Log(volume);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (volume < 10) { volume += 1; }
            if (volume == 9) { audioPlayer.volume = 0.5f; }
            else if (volume == 8) { audioPlayer.volume = 0.1f; }
            else if (volume == 7) { audioPlayer.volume = 0.0f; }
            Debug.Log(volume);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            
            videoPlayer.url = "file:///C://疑似窓/VirtualWindowAssets//video_01.mp4";

        }
        if (Input.GetKeyDown(KeyCode.B))
        { 
            videoPlayer.url = "file:///C://疑似窓/VirtualWindowAssets//video_02.mp4";
        }
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainLive");
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

    public void SetNumLock(bool bState)
    {
        if (GetNumLock() != bState)
        {
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0);
            keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
    }
}
