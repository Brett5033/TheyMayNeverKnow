using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManBrain
{
    public Man self;
    public float maxEmotion = 100f;
    public float Love;
    public float Fear;
    public bool emotionStreakState; // true = love, false = fear
    public float streakIncrement = .01f; // Increment per value of streak
    public float streakCounter; // Duration of streak

    public ManBrain(Man self)
    {
        this.self = self;
        Love = maxEmotion/2;
        Fear = maxEmotion/2;
        emotionStreakState = false;
        streakCounter = 0f;
    }

    public void changeLove(float change)
    {
        change *= updateStreak(true);
        Love += change;
        Fear -= change;
        emotionStreakState = true;
        adjustEmotion();
    }

    public void changeFear(float change)
    {
        change *= updateStreak(false);
        Fear += change;
        Love -= change;
        emotionStreakState = false;
        adjustEmotion();
    }

    private float updateStreak(bool streakType)
    {
        if(streakType != emotionStreakState) // Streak broken
        {
            streakCounter = 1;
            emotionStreakState = streakType;
            return 1;
        } else
        {
            float streakFactor = (streakCounter * streakIncrement) + 1;
            streakCounter++;
            return streakFactor;
        }
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
        if(Love > maxEmotion) // Perfect
        {
            Love = maxEmotion;
            Fear = 0;
        }
        if(Fear > maxEmotion) // Death
        {
            Fear = maxEmotion;
            Love = 0;
            self.summonDeath();
        }
    }
}
