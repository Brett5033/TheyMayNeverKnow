using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLists : MonoBehaviour
{
    public MapObject Grass;
    public MapObject Altar;
    public MapObject[] starter;
    public MapObject[] stage1;
    public MapObject[] stage2;
    public MapObject[] stage3;
    public MapObject[] stage4;
    public MapObject[] stage5;
    

    public MapObject getRandomGameobject(int a)
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

    public MapObject getStartMapObject()
    {
        return starter[Random.Range(0, starter.Length)];
    }

    public MapObject getAltar()
    {
        return Altar;
    }
    
}
