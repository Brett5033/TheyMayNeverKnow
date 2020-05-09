using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public SpriteRenderer sr;
    public Color c;
    public TileLists tl;
    public GridTester gt;
    public GameObject tileObject;
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
        six
    }


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tl = GameObject.FindGameObjectWithTag("TileLists").GetComponent<TileLists>();
        gt = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridTester>();
        tileObject = new GameObject();
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
        GameObject oldObject = tileObject;
        if(stage!= Stage.zero) // Tile is an active object
        {
            tileObject = Instantiate(tl.getRandomGameobject((int)stage),transform.position,ControlFactors.isometric);
            //gameObject.layer = 8;
        }
        else
        {
            if(altar && starting) // Sets altar at start
            {
                tileObject = Instantiate(tl.getAltar(), transform.position, ControlFactors.isometric);
                starting = false;
                /*if (tileObject.layer != 8)
                {
                    gameObject.layer = 0;
                }*/
            }
            else if (starting) // Sets everyother tile at start
            {
                tileObject = Instantiate(tl.getStartGameobject(), transform.position, ControlFactors.isometric);
                starting = false;
                /*if (tileObject.layer != 8)
                {
                    gameObject.layer = 0;
                }*/
            }
            else if (!altar)
            {
                tileObject = Instantiate(tl.Grass, transform.position, ControlFactors.isometric);
                /*if (tileObject.layer != 8)
                {
                    gameObject.layer = 0;
                    gt.aPath.Scan();
                }*/
                
            }
            
        }
        Destroy(oldObject);
        tileObject.transform.parent = gameObject.transform;
        gt.aPath.UpdateGraphs(new Bounds(transform.position, new Vector3(10f, 10f, 1f)), 3f);
        //tileObject.transform.localScale = ControlFactors.isometricScale;
    }
    /*
    public bool RecievedUpdate() // Called for tiles automatic updates
    {
        bool updated = false;
        if (!altar)
        {
            if (stage < stageMax)
            {
                stage++;
                SetTile();
                if (population.Count < 1)
                {
                    changePopulation(true);
                }
                updated = true;
            }
        }
        return updated;
       
    }*/

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
                        if (population.Count > 0)
                        {
                            changePopulation(false);
                        }
                    }
                }
            }
            UpdatePopulationLevel();
            SetTile();
        }
        return hasChanged;
    }

    public void changePopulation(bool adding)
    {
        if (adding)
        {
            Man m = Instantiate(manPrefab, spawnPos, Quaternion.identity);
            m.transform.parent = transform;
            m.homeLevel = 1;
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
            population[i].homeLevel = (int)stage;
        }
    }

}
