using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public TileLists tileList;
    public ProfessionList profList;
    public GridTester gt;
    public MapObject mapObject;
    public Man manPrefab;
    public List<Man> population;
    public Vector3 spawnPos;
    public bool altar = false;

    readonly Stage stageMax = Stage.three;
    Stage stage = Stage.zero;

    
    bool starting = true;

    enum Stage{ // Determines the development stage of the map tile
        zero,
        one,
        two,
        three,
        four,
        five,
        six,
        NumberOfTypes
    }


    // Start is called before the first frame update
    void Start()
    {
        gt = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridTester>();
        tileList = gt.GetComponent<TileLists>();
        profList = gt.GetComponent<ProfessionList>();
        population = new List<Man>();
        spawnPos = transform.position;
        spawnPos.y -= 1.25f;
        SetTile();
        //tileObject.transform.localScale = ControlFactors.isometricScale;

        //Testing Purposes: Fills in Grid:
        //stage = Stage.one;
        //SetTile();

    }

    private void SetTile()
    {
        MapObject oldObject = mapObject;
        if(stage!= Stage.zero) // Tile is an active object
        {
            mapObject = Instantiate(tileList.getRandomGameobject((int)stage),transform.position,ControlFactors.isometric);
            //gameObject.layer = 8;
        }
        else
        {
            if(altar && starting) // Sets altar at start
            {
                mapObject = Instantiate(tileList.getAltar(), transform.position, ControlFactors.isometric);
                starting = false;
            }
            else if (starting) // Sets everyother tile at start
            {
                mapObject = Instantiate(tileList.getStartMapObject(), transform.position, ControlFactors.isometric);
                starting = false;
            }
            else if (!altar) // When any tile gets destroyed
            {
                mapObject = Instantiate(tileList.Grass, transform.position, ControlFactors.isometric);
            }
            
        }
        if (oldObject != null)
        {
            oldObject.gameObject.SetActive(false);
            Destroy(oldObject,.01f);
        }
        
        mapObject.transform.parent = gameObject.transform;
        gt.aPath.UpdateGraphs(new Bounds(transform.position, new Vector3(10f, 10f, 1f)), 3f);
        //tileObject.transform.localScale = ControlFactors.isometricScale;
    }

    public bool ChangeTileStage(float change) // Changes a tile by change, used for spells that affect tiles
    {
        bool hasChanged = false;
        //Debug.Log("Changing Tile");
        if (!altar)
        {
            for (int c = 0; c < Mathf.Abs(change); c++)
            {
                if (change > 0)
                {
                    stage++;
                    if (stage > stageMax)
                    {
                        stage = stageMax;
                    }
                    else
                    {
                        hasChanged = true;
                        ControlFactors.DEVELOPMENT_SCORE++;
                        if (population.Count < 1)
                        {
                            changePopulation(true);
                        }
                    }
                }
                else
                {
                    stage--;
                    if (stage < Stage.zero)
                    {
                        stage = Stage.zero;
                    }
                    else
                    {
                        hasChanged = true;
                        ControlFactors.DEVELOPMENT_SCORE--;
                        if (population.Count > 0)
                        {
                            changePopulation(false);
                        }
                    }
                }
            }
            SetTile();
            UpdatePopulationLevel();
            setManProfession();
            
        }
        return hasChanged;
    }

    public void zeroTile()
    {
        ChangeTileStage(-((int)stage));
    }

    public void changePopulation(bool adding)
    {
        if (adding)
        {
            Man m = Instantiate(manPrefab, spawnPos, Quaternion.identity);
            m.transform.parent = transform;
            m.level = 1;
            gt.population.populationList.Add(m);
            population.Add(m);
        } else
        {
            gt.population.populationList.Remove(population[population.Count - 1]);
            Destroy(population[population.Count - 1].gameObject);
            population.RemoveAt(population.Count - 1);
        }
    }

    public bool isActiveTile()
    {
        if (!altar)
        {
            return (stage > Stage.zero);
        }
        else
        {
            return false;
        }
       
    }

    public float getStageNum()
    {
        return (float)stage;
    }

    public void UpdatePopulationLevel()
    {
        for (int i = 0; i < population.Count; i++)
        {
            population[i].level = (int)stage;
        }
    }

    public void setManProfession()
    {
        if(stage > 0)
        {
            Profession p = profList.getProfession((int)stage, mapObject.humanProfessionTag);
            foreach(Man m in population)
            {
                m.changeProfession(p);
            }
            
        }
    }

}
