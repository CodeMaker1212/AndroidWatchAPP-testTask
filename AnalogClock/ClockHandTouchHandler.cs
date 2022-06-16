using UnityEngine;
using UnityEngine.EventSystems;

sealed class ClockHandTouchHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private const float TOUCH_POINT_OFFSET = 90f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Clock.CurrentMode == Clock.ClockMode.SettingAlarm)
        {
            switch (gameObject.name)
            {
                case "secondHand":
                    AnalogClock.secondHandTransform.eulerAngles = new Vector3(0, -180, AnalogClock.secondHandTransform.eulerAngles.z);
                    break;

                case "minuteHand":
                    AnalogClock.minuteHandTransform.eulerAngles = new Vector3(0, -180, AnalogClock.minuteHandTransform.eulerAngles.z);
                    break;

                case "hourHand":
                    AnalogClock.hourHandTransform.eulerAngles = new Vector3(0, -180, AnalogClock.hourHandTransform.eulerAngles.z);
                    break;
            }
        }       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Clock.CurrentMode == Clock.ClockMode.SettingAlarm)
        {
            Vector3 direction;
            float angle;

            switch (gameObject.name)
            {
                case "secondHand":

                    direction = Input.mousePosition - AnalogClock.secondHandTransform.position;
                    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    AnalogClock.secondHandTransform.eulerAngles = new Vector3(0, -180, -angle + TOUCH_POINT_OFFSET);

                    var timeInSeconds = Mathf.Clamp(AnalogClock.secondHandTransform.eulerAngles.z / AnalogClock.HAND_OFFSET_ANGLE, 0, 59);

                    DigitalClock.secondsText.text = timeInSeconds.ToString("00");
                    Alarm.selectedAlarmTime["Seconds"] = DigitalClock.secondsText.text;
                    break;

                case "minuteHand":

                    direction = Input.mousePosition - AnalogClock.minuteHandTransform.position;
                    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    AnalogClock.minuteHandTransform.eulerAngles = new Vector3(0, -180, -angle + TOUCH_POINT_OFFSET);

                    var timeInMinutes = Mathf.Clamp(AnalogClock.minuteHandTransform.eulerAngles.z / AnalogClock.HAND_OFFSET_ANGLE, 0, 59);

                    DigitalClock.minutesText.text = timeInMinutes.ToString("00");
                    Alarm.selectedAlarmTime["Minutes"] = DigitalClock.minutesText.text;
                    break;

                case "hourHand":

                    direction = Input.mousePosition - AnalogClock.hourHandTransform.position;
                    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    AnalogClock.hourHandTransform.eulerAngles = new Vector3(0, -180, -angle + TOUCH_POINT_OFFSET);

                    var timeInHours = Mathf.Clamp(AnalogClock.hourHandTransform.eulerAngles.z
                                                / AnalogClock.HAND_OFFSET_ANGLE
                                                / AnalogClock.HOURS_HAND_OFFSET_MULTIPLIER, 0, 11);

                    DigitalClock.hoursText.text = timeInHours.ToString("00");
                    Alarm.selectedAlarmTime["Hours"] = DigitalClock.hoursText.text;                   
                    break;
            }
            Alarm.selectedAlarmTime["Format"] = DigitalClock.timeFormatText.text;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Clock.CurrentMode == Clock.ClockMode.SettingAlarm)
        {
            switch (gameObject.name)
            {
                case "secondHand":
                    AnalogClock.secondHandTransform.eulerAngles = new Vector3(0, 0, -AnalogClock.secondHandTransform.eulerAngles.z);
                    break;

                case "minuteHand":
                    AnalogClock.minuteHandTransform.eulerAngles = new Vector3(0, 0, -AnalogClock.minuteHandTransform.eulerAngles.z);
                    break;

                case "hourHand":
                    AnalogClock.hourHandTransform.eulerAngles = new Vector3(0, 0, -AnalogClock.hourHandTransform.eulerAngles.z);
                    break;
            }
        }       
    }
}
