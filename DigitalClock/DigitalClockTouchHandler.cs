using UnityEngine;
using UnityEngine.EventSystems;


public class DigitalClockTouchHandler : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        if (Clock.CurrentMode == Clock.ClockMode.SettingAlarm)
        {
            float inputDividerY = 15f;

            switch (gameObject.name)
            {               
                case "secondsText":

                    var seconds = Mathf.Clamp(Input.mousePosition.y / inputDividerY, 0, 59);
                    DigitalClock.secondsText.text = seconds.ToString("00");
                    AnalogClock.secondHandTransform.eulerAngles = new Vector3(0, 0, -seconds * AnalogClock.HAND_OFFSET_ANGLE);

                    Alarm.selectedAlarmTime["Seconds"] = seconds.ToString("00");
                    break;

                case "minutesText":

                    var minutes = Mathf.Clamp(Input.mousePosition.y / inputDividerY, 0, 59);
                    DigitalClock.minutesText.text = minutes.ToString("00");
                    AnalogClock.minuteHandTransform.eulerAngles = new Vector3(0, 0, -minutes * AnalogClock.HAND_OFFSET_ANGLE);

                    Alarm.selectedAlarmTime["Minutes"] = minutes.ToString("00");
                    break;

                case "hoursText":

                    var hours = Mathf.Clamp(Input.mousePosition.y / inputDividerY/3f, 0, 11);
                    DigitalClock.hoursText.text = hours.ToString("00");
                    AnalogClock.hourHandTransform.eulerAngles =
                          new Vector3(0, 0, -hours * AnalogClock.HAND_OFFSET_ANGLE * AnalogClock.HOURS_HAND_OFFSET_MULTIPLIER);              

                    Alarm.selectedAlarmTime["Hours"] = hours.ToString("00");                   
                    break;
            }           
        }       
    }
}