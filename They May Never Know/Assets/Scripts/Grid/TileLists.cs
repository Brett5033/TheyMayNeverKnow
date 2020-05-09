using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLists : MonoBehaviour
{
    public GameObject Grass;
    public GameObject Altar;
    public GameObject[] starter;
    public GameObject[] stage1;
    public GameObject[] stage2;
    public GameObject[] stage3;
    public GameObject[] stage4;
    public GameObject[] stage5;
    

    public GameObject getRandomGameobject(int a)
    {
        //Debug.Log("finding tile at: " + a);
        switch(a)
        {
            case 1: return stage1[Random.Range(0, stage1.Length)];
            case 2: return stage2[Random.Range(0, stage2.Length)];
            case 3: return stage3[Random.Range(0, stage3.Length)];
            case 4: return stage4[Random.Range(0, stage4.Length)];
            case 5: return stage5[Random.Range(0, stage5.Length)];
            default:
                Debug.Log("Array Not Found: " + a);
                return null;
           
        }
    }

    public GameObject getStartGameobject()
    {
        return starter[Random.Range(0, starter.Length)];
    }

    public GameObject getAltar()
    {
        return Altar;
    }
    
}
