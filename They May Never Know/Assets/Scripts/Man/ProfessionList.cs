using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfessionList : MonoBehaviour
{
    public Profession[] Stage1;
    public Profession[] Stage2;
    public Profession[] Stage3;

    public Profession getProfession(int stage, string tag)
    {
        switch (stage)
        {
            case 1:
                {
                    foreach(Profession p in Stage1)
                    {
                        if (p._Name.Equals(tag))
                        {
                            return p;
                        }
                    }
                }break;
            case 2:
                {
                    foreach (Profession p in Stage2)
                    {
                        if (p._Name.Equals(tag))
                        {
                            return p;
                        }
                    }
                }
                break;
            case 3:
                {
                    foreach (Profession p in Stage3)
                    {
                        if (p._Name.Equals(tag))
                        {
                            return p;
                        }
                    }
                }
                break;
            case 4:
                {

                }break;
            case 5:
                {

                }break;
            default:
                {
                    Debug.Log("Invalid Stage Given");
                    return null;
                }
               
        }
        return null;
    }
}
