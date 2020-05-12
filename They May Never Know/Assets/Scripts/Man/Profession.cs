using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Profession
{
    public string _Name;
    /*
    Body Part order:
    1. Head Forward
    2. Head Side
    3. Body
    4. Arm
    5. Leg
    */
    
    public Sprite _Head_Forward;
    public Sprite _Head_Side;
    public Sprite _Body;
    public Sprite _Arm;
    public Sprite _Leg;

    public string[] Requests;


    public string getRequest()
    {
        return Requests[(int)Random.Range(0, Requests.Length)];
    }
}
