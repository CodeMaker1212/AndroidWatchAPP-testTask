using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TimeFormat = TimeUpdateService.TimeFormat;

 sealed class Alarm : Clock
{
    public delegate void AlarmSettingEvent();
    public static event AlarmSettingEvent AlarmIsSet;

    private GameObject _setAlarmButton;
    private GameObject _confirmButton;
    private GameObject _arrows;
    private GameObject _resetAlarmButton;

    private Animator _alarmAnimator;
    private AudioSource _alarmAudio;
   
    private Image _switchTimeFormatButtonImage;

    private Text _alarmClockTimeText;

    public static TimeFormat CurrentAlarmFormat { get; private set; }
    private bool _isAlarmEnabled = false;

    public static Dictionary<string, string> selectedAlarmTime;


    public void EnableSettingAlarm()
    {
        CurrentMode = ClockMode.SettingAlarm;

        _setAlarmButton.SetActive(false);
        _arrows.SetActive(true);
        _confirmButton.SetActive(true);

        selectedAlarmTime = new Dictionary<string, string>();
        selectedAlarmTime.Add("Hours", DigitalClock.currentTimeOnDisplay["Hours"].ToString("00"));
        selectedAlarmTime.Add("Minutes", DigitalClock.currentTimeOnDisplay["Minutes"].ToString("00"));
        selectedAlarmTime.Add("Seconds", DigitalClock.currentTimeOnDisplay["Seconds"].ToString("00"));
        selectedAlarmTime.Add("Format", "");

        _switchTimeFormatButtonImage.enabled = true;
    }
  
    public void ConfirmAlarmSettings()
    {
        CurrentMode = ClockMode.DisplayingCurrentTime;

         AlarmIsSet();

        _arrows.SetActive(false);
        _confirmButton.SetActive(false);
        _setAlarmButton.SetActive(true);

        selectedAlarmTime["Format"] =CurrentAlarmFormat.ToString();
        _alarmClockTimeText.text = selectedAlarmTime["Format"] + " " 
            + selectedAlarmTime["Hours"] + ":" + selectedAlarmTime["Minutes"] + ":" + selectedAlarmTime["Seconds"];

        _resetAlarmButton.SetActive(true);
        _switchTimeFormatButtonImage.enabled = false;

        _isAlarmEnabled = true;
        StartCoroutine(CheckAlarmTime());
    }

    private IEnumerator CheckAlarmTime()
    {
        while (_isAlarmEnabled == true)
        {
            if (DigitalClock.currentTimeOnDisplay["Hours"] == float.Parse(selectedAlarmTime["Hours"])
                && DigitalClock.currentTimeOnDisplay["Minutes"] == float.Parse(selectedAlarmTime["Minutes"])
                && DigitalClock.currentTimeOnDisplay["Seconds"] == float.Parse(selectedAlarmTime["Seconds"])
                && CurrentFormat.ToString() == selectedAlarmTime["Format"])
                Ring();

            yield return new WaitForSeconds(1f);
        }      
    }
    public void ResetAlarm()
    {
        _alarmAnimator.SetFloat("Ringing", -1f);
        _alarmAudio.Stop();
        _isAlarmEnabled=false;
        _resetAlarmButton.SetActive(false);
    }

    public void ChangeTimeFormat()
    {
        if(CurrentMode == ClockMode.SettingAlarm)
        {
            switch (DigitalClock.timeFormatText.text)
            {
                case "AM":
                     DigitalClock.timeFormatText.text = "PM";
                    CurrentAlarmFormat = TimeFormat.PM;
                    break;
                case "PM":
                    DigitalClock.timeFormatText.text = "AM";
                    CurrentAlarmFormat = TimeFormat.AM;
                    break;
            }
        }        
    }

    private void Ring()
    {
        _alarmAnimator.SetFloat("Ringing", 1f);      
        _alarmAudio.Play();
    }
   
   
    private void Start()
    {
        _setAlarmButton = GameObject.Find("alarmButton");
        _confirmButton = GameObject.Find("confirmAlarmSettingsButton");
        _arrows = GameObject.Find("backgroundArrows");
        _switchTimeFormatButtonImage = GameObject.Find("switchTimeFormatButton").GetComponent<Image>();
        _resetAlarmButton = GameObject.Find("alarmTimeButton");

        _alarmClockTimeText = GameObject.Find("alarmClockTimeText").GetComponent<Text>();

        _alarmAnimator = _resetAlarmButton.GetComponent<Animator>();
        _alarmAudio = GetComponent<AudioSource>();
        _alarmAudio.playOnAwake = false;

        _arrows.SetActive(false);
        _confirmButton.SetActive(false);
        _resetAlarmButton.SetActive(false);

        _switchTimeFormatButtonImage.enabled = false;
        CurrentAlarmFormat = CurrentFormat;
    }
}
