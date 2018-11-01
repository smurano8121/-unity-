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

public class WeatherInfo : MonoBehaviour
{
    public int Width = 1920;
    public int Height = 1080;
    public int FPS = 30;
    public Texture2D texture;
    //[SerializeField]
    public VideoPlayer videoPlayer;
    public AudioSource audioPlayer;
    public ushort audioTrackNum = 0;
    public ushort audioTrackCount = 1;
    // Use this for initialization
    public string beforeweather; //天気が変化したか比較するための変化前の天気
    public string judgeclouds;
    public string month; //現在の月を取得
    public string hour; //現在の時間を取得
    public string minute; //現在の分を取得
    public string date; //現在の時刻
    public float min_10; //10分測る
    public float min_1; //1分測る
    public string[] array0; //日の出JSONデータ格納
    public string[] array1; //日の入りJSONデータ格納
    public string[] arraysunset; //日の出時刻
                                 //public int[] arraysunsettime = new int[4]; //日の出時刻
    public int arraysunsettime;
    public string[] arraysunrise; //日の入り時刻
                                  //日の入り時刻
    public int sunrisetime;
    public int sunriseminute;
    public int sunrizenum;
    //日の出時刻
    public int sunsettime;
    public int sunsetminute;
    public int sunsetnum;
    //夕暮れ時刻
    public int darktime;
    public int darkminute;
    public int darknum;
    //現在の時刻
    public int nowhour;
    public int nowminute;
    public int nownum;
    public string nowweather; //現在の天気
    public string nowseason; //現在の季節
    public string nowtime; //現在の朝昼夕晩
    public string nowinfo; //現在の動画条件
    public string pastinfo; //現在の動画条件保持
    public int time; //動画が切り替わった回数
    public string weathertxt;
    public string weathertxt2;
    public int accweather;

    public static DateTime UnixTimeToDateTime(double unixTime)
    {
        DateTime dt =
            new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        dt = dt.AddSeconds(unixTime);
        return dt;
    }

    IEnumerator Start()
    {

        min_10 = 0;
        min_1 = 0;
        StartCoroutine("Weather"); //天気情報を取得
        enabled = false;
        yield return new WaitForSeconds(1); //天気情報を取得するまで処理を止める
        enabled = true;
        //動画と音が再生できるように用意
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioPlayer = gameObject.AddComponent<AudioSource>();
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.source = VideoSource.Url;
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.controlledAudioTrackCount = audioTrackCount;
        videoPlayer.EnableAudioTrack(audioTrackNum, true);
        videoPlayer.SetTargetAudioSource(audioTrackNum, audioPlayer);
        videoPlayer.Prepare();
        //天気情報の読み込み　weather_0["main"]に天気情報が格納
        //openweathermapから天気情報取得
        FileInfo fi = new FileInfo("Assets/Resources/sample.json");
        try
        {
            // 一行毎読み込み
            using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                weathertxt = sr.ReadToEnd();
            }
        }
        catch (Exception e) { }
        //accweatherから天気情報
        FileInfo fi2 = new FileInfo("Assets/Resources/weather.txt");
        try
        {
            // 一行毎読み込み
            using (StreamReader sr = new StreamReader(fi2.OpenRead(), Encoding.UTF8))
            {
                weathertxt2 = sr.ReadToEnd();
            }
        }
        catch (Exception e) { }
        accweather = weathertxt2.Length;
        var json = weathertxt; //(1階層目)
        var jsonData = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>; //(2階層目)
        var weather = (IList)jsonData["weather"]; //(3階層目)
        var weather_0 = (IDictionary)weather[0];
        var sys = jsonData["sys"] as Dictionary<string, object>;
        DateTime dt0 = UnixTimeToDateTime((long)sys["sunrise"] + 32400);
        DateTime dt1 = UnixTimeToDateTime((long)sys["sunset"] + 32400);
        string str0 = dt0.ToString();
        array0 = str0.Split(' ');
        arraysunrise = array0[1].Split(':');
        string str1 = dt1.ToString();
        array1 = str1.Split(' ');
        arraysunset = array1[1].Split(':');
        month = System.DateTime.Now.Month.ToString(); //現在の月を取得
        sunrisetime = int.Parse(arraysunrise[0]);
        sunriseminute = int.Parse(arraysunrise[1]);
        sunrizenum = sunrisetime * 60 + sunriseminute;
        sunsettime = int.Parse(arraysunset[0]) + 12;
        sunsetminute = int.Parse(arraysunset[1]);
        sunsetnum = sunsettime * 60 + sunsetminute;
        beforeweather = "Weather"; //初期の天気
        Debug.Log(str0);
        Debug.Log(str1);
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
    void Update()
    {
        if (min_1 >= 60f || time == 0)
        {
            //天気情報の読み込み　weather_0["main"]に天気情報が格納
            FileInfo fi = new FileInfo("Assets/Resources/sample.json");
            try
            {
                // 一行毎読み込み
                using (StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
                {
                    weathertxt = sr.ReadToEnd();
                    Debug.Log(weathertxt);
                }
            }
            catch (Exception e) { }

            var json = weathertxt; //(1階層目)
            var jsonData = MiniJSON.Json.Deserialize(json) as Dictionary<string, object>; //(2階層目)
            var weather = (IList)jsonData["weather"]; //(3階層目)
            var weather_0 = (IDictionary)weather[0];
            hour = System.DateTime.Now.Hour.ToString(); //現在の時間を取得
            minute = System.DateTime.Now.Minute.ToString(); //現在の分を取得
            date = System.DateTime.Now.ToString(); //現在の時刻
            nowhour = int.Parse(hour);
            nowminute = int.Parse(minute);
            Debug.Log(nowhour);
            Debug.Log(nowminute);
            FileInfo fi2 = new FileInfo("Assets/Resources/weather.txt");
            try
            {
                // 一行毎読み込み
                using (StreamReader sr = new StreamReader(fi2.OpenRead(), Encoding.UTF8))
                {
                    weathertxt2 = sr.ReadToEnd();
                }
            }
            catch (Exception e) { }
            Debug.Log(weathertxt2);
            accweather = weathertxt2.Length;
            min_1 = 0;

            nownum = nowhour * 60 + nowminute;

            if (month == "3" || month == "4" || month == "5") { nowseason = "spring"; }
            else if (month == "6" || month == "7" || month == "8") { nowseason = "summer"; }
            else if (month == "9" || month == "10" || month == "11") { nowseason = "autumm"; }
            else { nowseason = "winter"; }

            if (month == "1" || month == "2" || month == "10" || month == "11" || month == "12") { darktime = 16; darkminute = 30; }
            else if (month == "3" || month == "4" || month == "9") { darktime = 17; darkminute = 30; }
            else { darktime = 18; darkminute = 30; }

            darknum = darktime * 60 + darkminute;

            if (accweather != 6) { nowweather = "heavyrain"; }
            else if (accweather == 6 && (string)weather_0["main"] == "Clear") { nowweather = "clear"; }
            else if (accweather == 6 && (string)weather_0["main"] == "Clouds" && ((string)weather_0["description"] == "scattered clouds" || (string)weather_0["description"] == "few clouds")) { nowweather = "clear"; }
            else if ((string)weather_0["main"] == "Clouds") { nowweather = "clouds"; }
            else if ((string)weather_0["main"] == "Rain" && ((string)weather_0["description"] == "light rain" || (string)weather_0["description"] == "moderate rain" || (string)weather_0["description"] == "light intensity shower rain" || (string)weather_0["description"] == "shower rain")) { nowweather = "lightrain"; }
            else if ((string)weather_0["main"] == "Rain" || (string)weather_0["main"] == "Thunderstorm") { nowweather = "heavyrain"; }
            else { nowweather = "snow"; }

            Debug.Log(nowweather);

            if (sunrizenum - 8 <= nownum && nownum < sunrizenum) { nowtime = "morning"; }
            else if (sunrizenum <= nownum && nownum < 12 * 60) { nowtime = "morning1"; }
            else if (12 * 60 <= nownum && nownum < darknum) { nowtime = "day"; }
            else if (darknum <= nownum && nownum < sunsetnum) { nowtime = "evening"; }
            else if (sunsetnum <= nownum && nownum < sunsetnum + 2) { nowtime = "evening2"; }
            else if (sunsetnum + 2 <= nownum && nownum < sunsetnum + 4) { nowtime = "evening3"; }
            else if (sunsetnum + 4 <= nownum && nownum < sunsetnum + 8) { nowtime = "evening4"; }
            else if (sunsetnum + 8 <= nownum && nownum < sunsetnum + 13) { nowtime = "evening5"; }
            else if (sunsetnum + 13 <= nownum && nownum < sunsetnum + 18) { nowtime = "evening6"; }
            else { nowtime = "night"; }
            StreamWriter sw = new StreamWriter("Assets/Resources/Logdate.txt", true); //true=追記 false=上書き
            sw.WriteLine("{0}時{1}分の天気,{2}",nowhour,nowminute,nowweather);
            sw.Flush();
            sw.Close();
            time++;
        }
        // nowinfo = "file://M:/VirtualWindowAssets/動画/" + nowseason + "/" + nowseason + "_" + nowweather + "_" + nowtime + ".mp4";
        //Debug.Log(nowinfo);

        /* if (nowinfo != pastinfo || time == 0)
         {
             Debug.Log(nownum);
             Debug.Log(nowinfo);
             Debug.Log((string)weather_0["main"]);//天気
             Destroy(videoPlayer);
             videoPlayer = gameObject.AddComponent<VideoPlayer>();
             videoPlayer.url = nowinfo;
             GetComponent<Renderer>().material.mainTexture = videoPlayer.texture;
             videoPlayer.Play(); videoPlayer.isLooping = true;
             pastinfo = nowinfo;
             time += 1;
         }*/

        if (min_10 >= 600f) //10分おきに天気情報を読み取る
        {
            StartCoroutine("Weather");
            min_10 = 0;
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainCamera");
        }

        min_10 += Time.deltaTime;
        min_1 += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainCamera");
        }
    }

    //天気情報をOpenWeatherのAPIから10分おきに取得
    IEnumerator Weather()
    {
        Debug.Log("refresh");
        var url = "http://api.openweathermap.org/data/2.5/weather?lat=34.8505121&lon=135.778444&APPID=b8194c16b5b19a1f5fe0b7d4632413a5";
        var wwwForm = new WWWForm();
        var name = "Tanabe";
        wwwForm.AddField("name", name);
        WWW req = new WWW(url, wwwForm);
        yield return req;
        if (req.error != null || req.text == "")
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.text);
            var sw = new StreamWriter("Assets/Resources/sample.json", false);
            sw.WriteLine(req.text);
            sw.Flush();
            sw.Close();
        }
        req.Dispose();
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
