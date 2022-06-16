using System.Collections;
using UnityEngine;
using System;
using System.Threading;

public class TimeUpdateService : MonoBehaviour
{
    private const float TIME_UPDATE_INTERVAL = 3600f;

    public delegate void TimeUpdateEvent();
    public static event TimeUpdateEvent HourPassed;

    public enum TimeFormat { AM, PM }


    public static DateTime TryGetTimeFromInternet(string source, out bool sourcehasTime)
    {
        var www = new WWW(source);

        while (!www.isDone && www.error == null)
            Thread.Sleep(1);

        var str = www.responseHeaders["Date"];
        DateTime dateTime;

        if (!DateTime.TryParse(str, out dateTime))
        {
            sourcehasTime = false;
            return DateTime.MinValue;
        }

        sourcehasTime = true;
        return dateTime.ToLocalTime();
    }

    private static IEnumerator CheckApplicationLaunchTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(TIME_UPDATE_INTERVAL);
            HourPassed();
        }       
    }

    private void Awake() => StartCoroutine(CheckApplicationLaunchTime());
}
