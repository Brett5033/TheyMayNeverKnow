using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class GridTester : MonoBehaviour
{

    //New Variables
    /// <summary> The tile list of the map, used to hold all permanent tiles for pathfinding and interactions </summary>
    public List<MapTile> map_Tiles;
    /// <summary> How far a civilization may expand from (0,0) </summary>
    public float Expansion_Distance;
    /// <summary> How far apart objects must spawn from each other </summary>
    public float min_Distance;
    /// <summary> Random amount of starter objects that can spawn at generation </summary>
    public int starter_Objects;
    /// <summary> How many tiles spawn at generation </summary>
    public int starter_Tiles;

    //New Methods

    // Old Variables
    //public MapGrid mapGrid;
    //public MapTile[,] mapObjects;
    //public MapTile[] activeTiles;
    public MapTile mapFillerPrefab;
    public Population population;
    public MeshHandler Land;
    public GameObject Background;
    public AstarPath aPath;
    public LayerMask whatIsTiles;
    
    //public int mapWidth;
    //public int mapHeight;
    //public int gridNodeWidth;
    //public int gridNodeHeight;
    //public float distanceScalar = 3f;
    //public float distancePower = 1.5f;
    //public float updateThreshold = .2f;
    //public int updateChance = 175;
    public float neighborBonus = 0.1f;

    public float numTries = 0f;
    public float totalChance = 0f;
    public float startUpDelay;

    public Vector2 AltarPos;
    
    RandomFromDistribution.ConfidenceLevel_e confLevel = RandomFromDistribution.ConfidenceLevel_e._999;

    // Start is called before the first frame update
    void Start()
    {
        ControlFactors.SetDefaults();
        PopulateMap(true);
        //Background.transform.position = new Vector3(-2 * Expansion_Distance, 2 * Expansion_Distance, 0);
        //Background.GetComponent<SpriteRenderer>().size = new Vector3(2 * Expansion_Distance, 2 * Expansion_Distance, 0);
        //Debug.Log("Map Generated");


        Land = transform.Find("MeshHandler").GetComponent<MeshHandler>();
        //mapGrid = new MapGrid(mapWidth, mapHeight, gridNodeWidth, gridNodeHeight);
        population = new Population(this);
        //PopulateGrid();
        aPath.transform.position = new Vector3(aPath.transform.position.x - (Expansion_Distance), aPath.transform.position.y - (Expansion_Distance), 0);
        GridGraph g = aPath.data.gridGraph;
        
        g.SetDimensions((int)(Expansion_Distance * 2), (int)(Expansion_Distance * 2), 1f);
        g.nodeSize = .5f;
        g.collision.heightMask = LayerMask.GetMask("TileGrid");
        //aPath.batchGraphUpdates = true;
        

        StartCoroutine(StartGrid(.5f));
        //g.drawGizmos = true;
    }
    /*
    private void FixedUpdate()
    {
        if (!startUpDone)
        {
            if(startUpDelay <= 0)
            {
                startUpDone = true;

            }
            else
            {
                startUpDelay -= Time.deltaTime;
            }
        }
       
    }*/

    public void PopulateMap(bool usePerlin)
    {
        Vector3 newPos;
        bool canPlace = false;

        MapTile m = Instantiate(mapFillerPrefab, Vector3.zero, Quaternion.identity);
        map_Tiles.Add(m);
        m.transform.parent = transform;
        m.altar = true;
        AltarPos = m.transform.position;
        // We want to place #count godly cubes
        for (int i = 0; i < starter_Objects; i++)
        {

            do
            {
                //Pick a random new pos ...
                float flippedX = 1;
                float flippedY = 1;
                if (Random.value > .5)
                    flippedX = -1f;
                if (Random.value < .5f)
                    flippedY = -1f;
                newPos = new Vector3((int)(Random.value * Expansion_Distance * flippedX), (int)(Random.value * Expansion_Distance * flippedY),0);

                if (usePerlin)
                {
                    // We have to ask mister perlin if he thinks this point is ok (And check distances)
                    canPlace = !(PerlinCheck(newPos) && CanBePlaced(newPos));
                }
                else
                    // It's enough to check distances
                    canPlace = !CanBePlaced(newPos);

            } while (canPlace); // This loop will run endlessly, if you try to stuff too many things in a too small ares. (Even on a modern quadcore!)
            MapTile newTile = Instantiate(mapFillerPrefab, newPos, Quaternion.identity);
            map_Tiles.Add(newTile);
            newTile.transform.parent = transform;
        }
    }

    private bool CanBePlaced(Vector3 newPos)
    {
        // Loop through all positions where we already want to place something
        if (!(Vector3.Distance(Vector3.zero, newPos) < Expansion_Distance))
            return false;
        for (int i = 0; i < map_Tiles.Count; i++) {
            if (Vector3.Distance(map_Tiles[i].transform.position, newPos) < min_Distance) // ... and check if the new point maybe is to close
                return false;
        }
        return true;
    }

    private bool PerlinCheck(Vector3 newPos)
    {
        // Basically how fast perlin changes his mind when you ask him for nearby Points
        float frequency = 8;

        // Lets ask him what he thinks of the current position
        float confidence = Mathf.PerlinNoise(newPos.x / Expansion_Distance * frequency, newPos.y / Expansion_Distance * frequency);

        if (Random.value <= confidence)
            return true;
        else
            return false;
    }

    IEnumerator StartGrid(float time) // Used to place first tiles and scan the graph
    {
        yield return new WaitForSeconds(time);
        //Debug.Log("Starting active tile placement");
        //Debug.Log("Spawning starter Tiles");
        GameObject[] startTiles = getNormalDistributionFromMap(Vector3.zero, Expansion_Distance / 10, whatIsTiles, 3);
        //Debug.Log(startTiles.Length);
        for (int i = 0; i < startTiles.Length; i++)
        {
            MapTile t = startTiles[i].gameObject.GetComponent<MapTile>();
            t.ChangeTileStage(1);
        }
        //Debug.Log("Done Spawning starter Tiles");
        //Debug.Log("Scanning");
        aPath.Scan();
    }
    
    /*
    public void PopulateGrid()
    {
        mapObjects = new MapTile[mapGrid.width, mapGrid.height];
        for(int w = 0; w < mapGrid.width; w++)
        {
            for(int h = 0; h < mapGrid.height; h++)
            {
                mapObjects[w, h] = Instantiate(mapFillerPrefab, mapGrid.Map[w, h], Quaternion.identity);
                //mapObjects[w, h].transform.localScale = ControlFactors.isometricScale;
                mapObjects[w, h].transform.parent = transform;
                mapObjects[w, h].GridPositon = new Vector2(w, h);
                if(w == Mathf.Floor(mapGrid.width/2f) && h == Mathf.Floor(mapGrid.height/2f)) //Determines Altar position at middle of map
                {
                    mapObjects[w, h].altar = true;
                    AltarPos = mapObjects[w, h].GetComponent<Transform>().position;
                    AltarPos.y -= 1.25f;
                }
            }
        }
    }
    */
    public GameObject[] getNormalDistributionFromMap(Vector3 centerPos, float radius, LayerMask whatIsObject, int numObjects)
    {
        Collider2D[] hitBoxes = Physics2D.OverlapCircleAll(centerPos, radius, whatIsObject);
        //Debug.Log("HitBoxes length: " + hitBoxes.Length);
        GameObject[] g = new GameObject[numObjects];
        int[] usedIndexes = new int[numObjects];
        for(int i = 0; i < g.Length; i++)
        {
            if (i >= hitBoxes.Length)
                break;
            g[i] = hitBoxes[(int)(Random.value*hitBoxes.Length - 1)].gameObject;
        }
        return g;
    }
    
    public bool ChanceForTileUpdate(MapTile tile)
    {
        Collider2D[] neighbors = Physics2D.OverlapCircleAll(tile.transform.position, 5f, 8);
        float chanceForUpdate = Random.value * 10f;
        if (neighbors.Length > 2)
            chanceForUpdate += 2;
        if(chanceForUpdate >= 8)
        {
            calcDevelopmentScore();
            return true;
        }
        return false;
    }

    private void calcDevelopmentScore()
    {
        if (confLevel == RandomFromDistribution.ConfidenceLevel_e._80 && ControlFactors.DEVELOPMENT_SCORE > 15000) { confLevel = RandomFromDistribution.ConfidenceLevel_e._60; Debug.Log("60"); }
        else if (confLevel == RandomFromDistribution.ConfidenceLevel_e._90 && ControlFactors.DEVELOPMENT_SCORE > 10000) { confLevel = RandomFromDistribution.ConfidenceLevel_e._80; Debug.Log("80"); }
        else if (confLevel == RandomFromDistribution.ConfidenceLevel_e._95 && ControlFactors.DEVELOPMENT_SCORE > 5000) { confLevel = RandomFromDistribution.ConfidenceLevel_e._90; Debug.Log("90"); }
        else if (confLevel == RandomFromDistribution.ConfidenceLevel_e._98 && ControlFactors.DEVELOPMENT_SCORE > 2000) { confLevel = RandomFromDistribution.ConfidenceLevel_e._95; Debug.Log("95"); }
        else if (confLevel == RandomFromDistribution.ConfidenceLevel_e._99 && ControlFactors.DEVELOPMENT_SCORE > 1000) { confLevel = RandomFromDistribution.ConfidenceLevel_e._98; Debug.Log("98"); }
        else if (confLevel == RandomFromDistribution.ConfidenceLevel_e._999 && ControlFactors.DEVELOPMENT_SCORE > 500) { confLevel = RandomFromDistribution.ConfidenceLevel_e._99; Debug.Log("99"); }
        
    }

    public MapTile targetRandomMapTilePosition(Man man,Vector2 homePos, int homeLevel) // Used to get a path target for Humans
    {
        //Craetes a normal distributed range based off current level
        GameObject[] target = getNormalDistributionFromMap(homePos, 10 * homeLevel, whatIsTiles, 1);
        return target[0].GetComponent<MapTile>();
    }
}
