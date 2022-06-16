using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitalClock : Clock
{
    public static Text secondsText;
    public static Text minutesText;
    public static Text hoursText;
    public static Text timeFormatText;

    public static Dictionary<string, float> currentTimeOnDisplay;

    private Coroutine _currentCoroutine;

    protected override void StartTimeDisplay()
    {
        if (_currentCoroutine == null)
        {           
            GetActuallyTime();
            SetStartClockFace(out currentTimeOnDisplay);
            _currentCoroutine = StartCoroutine(ShowCurrentTime());
        }
        else
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            StartTimeDisplay();
        }       
    }

    private void SetStartClockFace(out Dictionary<string, float> currentTimeOnDisplay)
    {
        secondsText.text = currentTime["Seconds"].ToString();
        minutesText.text = currentTime["Minutes"].ToString();
        hoursText.text = currentTime["Hours"].ToString();
        timeFormatText.text = CurrentFormat.ToString();


        currentTimeOnDisplay = new Dictionary<string, float>();

        currentTimeOnDisplay.Add("Hours", currentTime["Hours"]);
        currentTimeOnDisplay.Add("Minutes", currentTime["Minutes"]);
        currentTimeOnDisplay.Add("Seconds", currentTime["Seconds"]);
    }

    protected override IEnumerator ShowCurrentTime()
    {
        while (CurrentMode == ClockMode.DisplayingCurrentTime)
        {
            secondsText.text = currentTimeOnDisplay["Seconds"].ToString("00");
            minutesText.text = currentTimeOnDisplay["Minutes"].ToString("00");
            hoursText.text = currentTimeOnDisplay["Hours"].ToString("00");

            yield return new WaitForSeconds(1f);

            currentTimeOnDisplay["Seconds"]++;

            if (currentTimeOnDisplay["Seconds"] == MAX_SECONDS)
            {
                currentTimeOnDisplay["Seconds"] = 0;
                currentTimeOnDisplay["Minutes"]++;
            }

            if (currentTimeOnDisplay["Minutes"] == MAX_MINUTES)
            {
                currentTimeOnDisplay["Hours"]++;
                currentTimeOnDisplay["Minutes"] = 0;
            }

            if (currentTimeOnDisplay["Hours"] == MAX_HOURS)
                currentTimeOnDisplay["Hours"] = 0;            
        }
    }

  

    private void Start()
    {
        secondsText = GameObject.Find("secondsText").GetComponent<Text>();
        minutesText = GameObject.Find("minutesText").GetComponent<Text>();
        hoursText = GameObject.Find("hoursText").GetComponent<Text>();
        timeFormatText = GameObject.Find("timeFormatText").GetComponent<Text>();

        timeFormatText.text = CurrentFormat.ToString();

        StartTimeDisplay();

        Alarm.AlarmIsSet += StartTimeDisplay;
        TimeUpdateService.HourPassed += StartTimeDisplay;
    }   
}
