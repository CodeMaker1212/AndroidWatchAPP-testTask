using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TimeFormat = TimeUpdateService.TimeFormat;

public class Clock : MonoBehaviour
{
    protected const float MAX_SECONDS = 60f;
    protected const float MAX_MINUTES = 60f;
    protected const float MAX_HOURS = 24f;

    public enum ClockMode { DisplayingCurrentTime, SettingAlarm}
    public static ClockMode CurrentMode { get; set; }
    public static TimeFormat CurrentFormat { get; private set; }
    
    protected static Dictionary<string, float> currentTime;
    private static Text _timeSourceText;

    protected static void GetActuallyTime()
    {
        currentTime = new Dictionary<string, float>();

        if (Application.internetReachability == NetworkReachability.NotReachable)
            GetSystemTime();
        else
        {          
            bool hasTimeFromYandex;
            var yandexTime = TimeUpdateService.TryGetTimeFromInternet("https://yandex.ru", out hasTimeFromYandex);          

            if (hasTimeFromYandex == true)
            {
                _timeSourceText.text = "Yandex.ru time";

                currentTime.Add("Hours", float.Parse(yandexTime.ToString("hh")));
                currentTime.Add("Minutes", float.Parse(yandexTime.ToString("mm")));
                currentTime.Add("Seconds", float.Parse(yandexTime.ToString("ss")));

                CurrentFormat = (yandexTime.ToString("tt") == "оо" || yandexTime.ToString("tt") == "PM") ? TimeFormat.PM : TimeFormat.AM;
            }
            else 
            {
                bool hasTimeFromGoogle;
                var googleTime = TimeUpdateService.TryGetTimeFromInternet("https://google.ru", out hasTimeFromGoogle);

                if (hasTimeFromGoogle == true)
                {
                    _timeSourceText.text = "Google.ru time";

                    currentTime.Add("Hours", float.Parse(googleTime.ToString("hh")));
                    currentTime.Add("Minutes", float.Parse(googleTime.ToString("mm")));
                    currentTime.Add("Seconds", float.Parse(googleTime.ToString("ss")));

                    CurrentFormat = (googleTime.ToString("tt") == "оо" || googleTime.ToString("tt") == "PM") ? TimeFormat.PM : TimeFormat.AM;
                }
                else 
                    GetSystemTime();
            }          
        }                   
    }
    private static void GetSystemTime()
    {
        _timeSourceText.text = "System time. Check internet connection!";

        currentTime.Add("Hours", float.Parse(DateTime.Now.ToString("hh")));
        currentTime.Add("Minutes", float.Parse(DateTime.Now.ToString("mm")));
        currentTime.Add("Seconds", float.Parse(DateTime.Now.ToString("ss")));

        CurrentFormat = (DateTime.Now.ToString("tt") == "оо" || DateTime.Now.ToString("tt") == "PM") ? TimeFormat.PM : TimeFormat.AM;
    }
    
    protected virtual void StartTimeDisplay() { }
    protected virtual IEnumerator ShowCurrentTime() { yield return null; }

    private void Awake()
    {
        _timeSourceText = GameObject.Find("timeSourceText").GetComponent<Text>();
    }
}
