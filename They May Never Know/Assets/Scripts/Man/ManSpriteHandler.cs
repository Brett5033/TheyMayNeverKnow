using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManSpriteHandler : MonoBehaviour
{
    public GameObject _Head_Forward;
    public GameObject _Head_Side;
    public GameObject _Body;
    public GameObject _Arm_Left;
    public GameObject _Arm_Right;
    public GameObject _Leg_Left;
    public GameObject _Leg_Right;


    ManSpriteHandler(Profession p)
    {
        convertToNewProfession(p);
    }

    public void convertToNewProfession(Profession p)
    {
        _Head_Forward.GetComponent<SpriteRenderer>().sprite = p._Head_Forward;
        _Head_Side.GetComponent<SpriteRenderer>().sprite = p._Head_Side;
        _Body.GetComponent<SpriteRenderer>().sprite = p._Body;
        _Arm_Left.GetComponent<SpriteRenderer>().sprite = p._Arm;
        _Arm_Right.GetComponent<SpriteRenderer>().sprite = p._Arm;
        _Leg_Left.GetComponent<SpriteRenderer>().sprite = p._Leg;
        _Leg_Right.GetComponent<SpriteRenderer>().sprite = p._Leg;
    }
}
