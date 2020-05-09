using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population
{   
    public List<Man> populationList;
    public float statUpdateInterval = 1f;

    float statUpdateTimer = 0;
    GridTester gt;

    public Population(GridTester gt)
    {
        populationList = new List<Man>();
        this.gt = gt;
    }

    public void forceUpdatePopulation()
    {
        populationList.Clear();
        foreach(MapTile tile in gt.map_Tiles)
        {
            if (tile.isActiveTile())
            {
                populationList.AddRange(tile.population);
            }
        }
            
    }

    public void updateLoveHateStat()
    {
        if(statUpdateTimer >= statUpdateInterval)
        {
            if(populationList.Count > 0)
            {
                statUpdateTimer = 0;
                float love = 0;
                float fear = 0;
                foreach(Man m in populationList)
                {
                    love += m.brain.Love;
                    fear += m.brain.Fear;
                }
                ControlFactors.EMOTION = populationList[0].brain.maxEmotion * populationList.Count;
                ControlFactors.LOVE = love;
                ControlFactors.FEAR = fear;
            }
            
        } else
        {
            statUpdateTimer += Time.deltaTime;
        }
        
    }

}
