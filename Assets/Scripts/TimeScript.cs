using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeScript : MonoBehaviour
{
    public int timeSpeed;
    public int dayStartTime;
    public int dayEndTime;
    private TextMeshProUGUI TimeClock;
    public GameObject ResultsScreen;
    public MoneyBarScript MBS;
    public float seconds;
    public int minutes;
    public int hours;
    private bool timeChange;
    private string timePeriod;
    public bool timePaused;
    public int day;
    public List<float> dailyEarnings = new List<float>();
    private TextMeshProUGUI dayText;
    public GameObject dayBanner;
    public ShareHolderMeetingScript SHMS;

    void Start()
    {
        TimeClock = GetComponent<TextMeshProUGUI>();
        dayText = transform.parent.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        timePeriod = "AM";
        hours = dayStartTime;
        TimeClock.text = "0" + hours + ":0" + minutes + " " + timePeriod;
        day = 1;
        timePaused = true;
    }

    void Update()
    {
        if (!timePaused)
        {
            seconds += Time.deltaTime * timeSpeed;
        }

        if (seconds >= 60)
        {
            minutes++;
            seconds = 0;
            timeChange = true;
        }

        if (minutes >= 60)
        {
            hours++;
            minutes = 0;
            timeChange = true;
            if (hours == 12)
            {
                if (timePeriod == "AM")
                {
                    timePeriod = "PM";
                }
                else
                {
                    timePeriod = "AM";
                }
            }

            if (hours == 13)
            {
                hours = 1;
            }

            if (hours == dayEndTime)
            {
                day++;
                timePeriod = "AM";
                hours = dayStartTime;
                timeChange = true;
                dayText.text = "Day: " + day;
                StartCoroutine("DayBanner");
            }
        }

        if (timeChange)
        {
            if (hours < 10)
            {
                if (minutes >= 10)
                {
                    TimeClock.text = "0" + hours + ":" + minutes + " " + timePeriod;
                }
                else
                {
                    TimeClock.text = "0" + hours + ":0" + minutes + " " + timePeriod;
                }
            }
            else
            {
                if (minutes >= 10)
                {
                    TimeClock.text = hours + ":" + minutes + " " + timePeriod;
                }
                else
                {
                    TimeClock.text = hours + ":0" + minutes + " " + timePeriod;
                }
            }
            timeChange = false;
        }

    }


    IEnumerator DayBanner()
    {
        dayBanner.SetActive(true);
        dayBanner.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = day.ToString();
        dayBanner.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = MBS.money / SHMS.monthlyGoal * 100 + "%";
        yield return new WaitForSeconds(5f);
        dayBanner.SetActive(false);
    }
}
