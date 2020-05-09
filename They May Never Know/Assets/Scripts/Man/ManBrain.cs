using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManBrain
{
    public float maxEmotion = 100f;
    public float Love;
    public float Fear;

    public ManBrain()
    {
        Love = maxEmotion/2;
        Fear = maxEmotion/2;
    }

    public void changeLove(float change)
    {
        Love += change;
        Fear -= change;
        adjustEmotion();
    }

    public void changeFear(float change)
    {
        Fear += change;
        Love -= change;
        adjustEmotion();
    }

    public void changeBestTrait(float change)
    {
        if(Love > Fear)
        {
            changeLove(change);
        } else if(Fear > Love)
        {
            changeFear(change);
        } else
        {
            changeLove(change/2f);
            changeFear(change/2f);
        }
    }

    private void adjustEmotion()
    {
        if(Love > maxEmotion)
        {
            Love = maxEmotion;
            Fear = 0;
        }
        if(Fear > maxEmotion)
        {
            Fear = maxEmotion;
            Love = 0;
        }
    }
}
