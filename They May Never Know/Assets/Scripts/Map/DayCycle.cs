using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public Light2D globalLight;
    public GridTester gt;

    //public List<Light2D> nightLights = new List<Light2D>();
    public float dayLength = 120f;
    public float lightUpdatesInterval =.5f;
    float lightUpdateTimer = 0;

    public TimeOfDay time;

    private void Start()
    {
        gt = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridTester>();
        ControlFactors.DAY_LENGTH = dayLength;
        ControlFactors.CURRENT_TIME = dayLength / 7;
        setTime();
    }

    public enum TimeOfDay
    {
        MorningMidnight,
        MorningDark,
        Sunrise,
        Day,
        Sunset,
        NightDark,
        NightMidnight
    }

    void FixedUpdate()
    {
        ControlFactors.CURRENT_TIME += Time.deltaTime;
        //Debug.Log(ControlFactors.CURRENT_TIME + " v. " + ControlFactors.DAY_LENGTH);
        if(ControlFactors.CURRENT_TIME >= ControlFactors.DAY_LENGTH)
        {
            //Debug.Log("Midnight");
            ControlFactors.CURRENT_TIME = 0;
        }
        if(lightUpdateTimer >= lightUpdatesInterval)
        {
            lightUpdateTimer = 0;
            setTime();
            setDynamicGlobalLight();
        } else
        {
            lightUpdateTimer += Time.deltaTime;
        }
    }

    private void setTime()
    {
        TimeOfDay oldTime = time;
        if(ControlFactors.CURRENT_TIME < ControlFactors.DAY_LENGTH / 12f) 
        {
            time = TimeOfDay.MorningMidnight;
        }
        else if (ControlFactors.CURRENT_TIME < (ControlFactors.DAY_LENGTH / 12f) * 2)
        {
            time = TimeOfDay.MorningDark;
        }
        else if (ControlFactors.CURRENT_TIME < (ControlFactors.DAY_LENGTH / 12f) * 3)
        {
            time = TimeOfDay.Sunrise;
        }
        else if (ControlFactors.CURRENT_TIME < (ControlFactors.DAY_LENGTH / 12f) * 9)
        {
            time = TimeOfDay.Day;
        }
        else if (ControlFactors.CURRENT_TIME < (ControlFactors.DAY_LENGTH / 12f) * 10)
        {
            time = TimeOfDay.Sunset;
        }
        else if (ControlFactors.CURRENT_TIME < (ControlFactors.DAY_LENGTH / 12f) * 11)
        {
            time = TimeOfDay.NightDark;
        }
        else
        {
            time = TimeOfDay.NightMidnight;
        }
        
        if(oldTime != time) // Checks if the Global needs to be updated;
        {
            if (time == TimeOfDay.Sunrise)
                startNewDay();
            else if (time == TimeOfDay.Sunset)
                endDay();
        }   
    }

    private void setDynamicGlobalLight()
    {
        if((int)time < 3) // Midnight to Day
        {
            globalLight.intensity = ControlFactors.CURRENT_TIME * (1f / (ControlFactors.DAY_LENGTH / 4f));

        } else if((int)time == 3) // Day
        {
            globalLight.intensity = 1f;
        }
        else // Day to Midnight
        {
            globalLight.intensity = (ControlFactors.DAY_LENGTH - ControlFactors.CURRENT_TIME) * (1f / (ControlFactors.DAY_LENGTH / 4f));
        }
    }

    private void startNewDay()
    {
        Debug.Log("New Day Started:");
        gt.population.startDayRequests();
    }

    private void endDay()
    {
        gt.population.clearRequests();
    }

    private void setGlobalLight() // 
    {
        switch (time)
        {
            case TimeOfDay.MorningMidnight:
            case TimeOfDay.NightMidnight:
                {
                    globalLight.intensity = .03f;
                }break;
            case TimeOfDay.MorningDark:
                {
                    globalLight.intensity = .15f;
                }
                break;
            case TimeOfDay.NightDark:
                {
                    globalLight.intensity = .15f;
                    //changeNightLights(true);
                }
                break;
            case TimeOfDay.Sunrise:
                {
                    globalLight.intensity = .6f;

                    //changeNightLights(false);
                }
                break;
            case TimeOfDay.Sunset:
                {
                    globalLight.intensity = .6f;
                }
                break;
            case TimeOfDay.Day:
                {
                    globalLight.intensity = 1f;
                }
                break;
        }

    }

    /*public void changeNightLights(bool on)
    {
        Debug.Log(nightLights.Count);
        foreach(Light2D l in nightLights)
        {
            l.GetComponent<Light2D>().enabled = on;
        }
    }*/
}
