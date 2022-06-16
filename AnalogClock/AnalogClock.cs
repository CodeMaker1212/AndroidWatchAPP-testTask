using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogClock : Clock
{
    public enum ClockHand { Hour, Minute, Second }

    public const float HAND_OFFSET_ANGLE = 6f;
    public const float HOUR_HAND_OFFSET_ANGLE = 29f;
    public const float HOURS_HAND_OFFSET_MULTIPLIER = 5f;

    public static Transform hourHandTransform;
    public static Transform minuteHandTransform;
    public static Transform secondHandTransform;

    private Dictionary<string, float> _currentHandsRotation;

    private Coroutine _currentCoroutine;

    protected override void StartTimeDisplay()
    {
        if (_currentCoroutine == null)
        {
            GetActuallyTime();
            SetHandsStartPosition(out _currentHandsRotation);
            _currentCoroutine = StartCoroutine(ShowCurrentTime());
        }
        else
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            StartTimeDisplay();
        }       
    }

    private void SetHandsStartPosition(out Dictionary<string, float> currentHandsRotation)
    {
        currentHandsRotation = new Dictionary<string, float>();

        secondHandTransform.eulerAngles = new Vector3(0, 0, -currentTime["Seconds"] * HAND_OFFSET_ANGLE);
        currentHandsRotation.Add("SecondHand", secondHandTransform.eulerAngles.z);

        minuteHandTransform.eulerAngles = new Vector3(0, 0, -currentTime["Minutes"] * HAND_OFFSET_ANGLE);
        currentHandsRotation.Add("MinuteHand", minuteHandTransform.eulerAngles.z);

        hourHandTransform.eulerAngles = new Vector3(0, 0, (-currentTime["Hours"] * HOURS_HAND_OFFSET_MULTIPLIER) * HAND_OFFSET_ANGLE);
        currentHandsRotation.Add("HourHand", hourHandTransform.eulerAngles.z);
    }

    private void MoveHand(ClockHand hand)
    {
        switch (hand)
        {
            case ClockHand.Hour:

                hourHandTransform.eulerAngles = new Vector3(0, 0, (_currentHandsRotation["HourHand"] - HOUR_HAND_OFFSET_ANGLE));
                _currentHandsRotation["HourHand"] = hourHandTransform.eulerAngles.z;
                break;
            case ClockHand.Minute:

                minuteHandTransform.eulerAngles = new Vector3(0, 0, _currentHandsRotation["MinuteHand"] - HAND_OFFSET_ANGLE);
                _currentHandsRotation["MinuteHand"] = minuteHandTransform.eulerAngles.z;
                break;
            case ClockHand.Second:

                secondHandTransform.eulerAngles = new Vector3(0, 0, _currentHandsRotation["SecondHand"] - HAND_OFFSET_ANGLE);
                _currentHandsRotation["SecondHand"] = secondHandTransform.eulerAngles.z;
                break;
        }       
    }

    protected override IEnumerator ShowCurrentTime()
    {
        bool canMoveHourHand = true;

        while (CurrentMode == ClockMode.DisplayingCurrentTime)
        {
            yield return new WaitForSeconds(1f);

            MoveHand(ClockHand.Second); 
            
            if(secondHandTransform.eulerAngles.z > 0 && secondHandTransform.eulerAngles.z < 1f)
            {
                MoveHand(ClockHand.Minute);
                canMoveHourHand = true;
            }
                         
            if (minuteHandTransform.eulerAngles.z > 0 && minuteHandTransform.eulerAngles.z < 1f && canMoveHourHand == true)
            {
                MoveHand(ClockHand.Hour);
                canMoveHourHand = false;
            }
        }       
    }
    
   
   

    private void Start()
    {
        hourHandTransform = GameObject.Find("hourHand").GetComponent<Transform>();
        minuteHandTransform = GameObject.Find("minuteHand").GetComponent<Transform>();
        secondHandTransform = GameObject.Find("secondHand").GetComponent<Transform>();

        StartTimeDisplay();

        Alarm.AlarmIsSet += StartTimeDisplay;
        TimeUpdateService.HourPassed += StartTimeDisplay;
    }
}
