using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ControlFactors
{
    // Control Factors
    
    
    public static float EMOTION = 0f;

    //Balloon: Love and Fear both are hit-up Power/Speed

    // By doing good deeds for your city, your people will love you and praise you, allowing 	
    // you to influence them by deeds to keep them in control.
    public static float LOVE = 0f;

    // By doing harm to your city when they dont do as you wish, your people will fear you 		
    // allowing you to influence them through harm and acts of wrath.
    public static float FEAR = 0f;

    // The current score that shows how developed a city is
    public static float DEVELOPMENT_SCORE = 0f;
    
    //The energy needed to use player spells/moves
    public static float ENERGY = 0f;

    public const float MAX_ENERGY = 20f;

    //Determines if the player can use Godly spells or not
    public static bool PLAYER_CAN_CAST = true;

    public static float PASS_RATE = 0f;

    public static float DAY_LENGTH = 0f;

    public static float CURRENT_TIME = 0f;

    public static Quaternion isometric = Quaternion.identity; //Quaternion.Euler(0, 0, -45);
    public static Vector3 isometricScale = new Vector3(1f, 0.5f, 1f);

    public static void ChangeEnergy(float change)
    {
        ENERGY += change;
    }

    public static void SetDefaults()
    {
        EMOTION = 2f;
        LOVE = 1f;
        FEAR = 1f;
        DEVELOPMENT_SCORE = 0f;
    }

    public static void setPlayerCast(bool can)
    {
        PLAYER_CAN_CAST = can;
    }
}
