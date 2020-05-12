using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerSpellCaster : MonoBehaviour
{

    public LayerMask whatIsTiles;
    public LayerMask squishMask;
    public Mesh mesh;
    public GridTester gt;
    public EventSystem es;
    public UIHandler uih;

    public GameObject spellUseIdentifier;

    public float circleSpellRadius;

    public BadSpellType badSpellChoice;
    public GoodSpellType goodSpellChoice;

    [SerializeField] public Spell[] _BadSpells;
    [SerializeField] public Spell[] _GoodSpells;

    public List<Spell> spellQueue;

    bool spellTypeSelected = false;
    bool spellDragging = false;


    public enum BadSpellType
    {
        Squish,
        Dread,
        Nimbus,
        NumberOfTypes
    }

    public enum GoodSpellType
    {
        Bless,
        Sunflower,
        NumberOfTypes
    }

    // Start is called before the first frame update
    void Start()
    {
        badSpellChoice = BadSpellType.Squish;
        goodSpellChoice = GoodSpellType.Bless;
        spellQueue = new List<Spell>();
        gt = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridTester>();
        uih = gt.GetComponent<UIHandler>();
        mesh = new Mesh();
        /*
        Vector3[] verticies =
                    {
                        new Vector3(-gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, 0),
                        new Vector3(-gt.gridNodeWidth,gt.mapHeight  * gt.gridNodeHeight),
                        new Vector3(gt.mapWidth * gt.gridNodeWidth - gt.gridNodeWidth, 0),
                        new Vector3(-gt.gridNodeWidth,-gt.mapHeight * gt.gridNodeHeight)
                    };
        mesh.vertices = verticies;
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };*/
        float angleStep = 360.0f / (float)gt.Land.numOfPoints;
        List<Vector3> vertexList = new List<Vector3>();
        List<int> triangleList = new List<int>();
        Quaternion quaternion = Quaternion.Euler(0.0f, 0.0f, angleStep);
        // Make first triangle.
        vertexList.Add(new Vector3(0.0f, 0.0f, 0.0f));  // 1. Circle center.
        vertexList.Add(new Vector3(0.0f, gt.Expansion_Distance, 0.0f));  // 2. First vertex on circle outline (radius = 0.5f)
        vertexList.Add(quaternion * vertexList[1]);     // 3. First vertex on circle outline rotated by angle)
                                                        // Add triangle indices.
        triangleList.Add(0);
        triangleList.Add(1);
        triangleList.Add(2);
        for (int i = 0; i < gt.Land.numOfPoints - 1; i++)
        {
            triangleList.Add(0);                      // Index of circle center.
            triangleList.Add(vertexList.Count - 1);
            triangleList.Add(vertexList.Count);
            vertexList.Add(quaternion * vertexList[vertexList.Count - 1]);
        }
        mesh.vertices = vertexList.ToArray();
        mesh.triangles = triangleList.ToArray();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            circleSpellRadius++;
        else if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            if(circleSpellRadius > 1)
            circleSpellRadius--;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ControlFactors.ENERGY = 100f;
        }
        if (spellDragging && Input.GetMouseButton(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spellUseIdentifier.transform.position = new Vector3(mouseWorld.x, mouseWorld.y, 20f);
        }
    }
    

    private void OnMouseUp()
    {
        
        if (spellDragging  && Input.GetMouseButtonUp(0) && !es.IsPointerOverGameObject())
        {
            //Debug.Log(spellTypeSelected);
            if (spellTypeSelected && spellQueue.Count > 0)
            {
                // Debug.Log("creating good spell" + goodSpellChoice);
                createGoodSpell();
            }
            else
            {
                // Debug.Log("creating bad spell" + badSpellChoice);
                createBadSpell();
            }
        }
        spellUseIdentifier.GetComponent<SpriteRenderer>().enabled = false;
        spellDragging = false;
    }

    private void createGoodSpell()
    {
        if (ControlFactors.PLAYER_CAN_CAST)
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 20;
            Spell s = Instantiate(spellQueue[0], clickPos, Quaternion.identity);
            if (s.spellUsed)
            {
                spellQueue.RemoveAt(0);
                Destroy(uih.queuedSpellButtons[0]);
                uih.queuedSpellButtons.RemoveAt(0);
                uih.shiftQueue();
                if(spellQueue.Count <= 0)
                {
                    uih.selectedBox.GetComponent<Image>().enabled = false;
                }
            }
           // Debug.Log("good spell created");    
        }
        
    }

    private void createBadSpell()
    {
        if (ControlFactors.PLAYER_CAN_CAST)
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickPos.z = 20;
            if (ControlFactors.ENERGY >= _BadSpells[(int)badSpellChoice].spellCost)
            {
                Spell s = Instantiate(_BadSpells[(int)badSpellChoice], clickPos, Quaternion.identity);
                if (s.spellUsed)
                    ControlFactors.ENERGY -= _BadSpells[(int)badSpellChoice].spellCost;
            }
        }
    }

    public void queueSpell()
    {
        int choice = Random.Range(0, (int)GoodSpellType.NumberOfTypes);
        spellQueue.Add(_GoodSpells[choice]);
        ControlFactors.ENERGY -= ControlFactors.MAX_ENERGY;
    }

    public void setBadSpell(int s)
    {
        if (ControlFactors.PLAYER_CAN_CAST)
        {
            // Debug.Log("Bad Spell Selected");
            badSpellChoice = (BadSpellType)s;
            spellTypeSelected = false;
            spellDragging = true;
            spellUseIdentifier.GetComponent<SpriteRenderer>().enabled = true;
            spellUseIdentifier.GetComponent<SpriteRenderer>().color = Color.red;
            uih.selectedBox.GetComponent<Image>().enabled = false;
        }
       
    }

    public void setGoodSpell(int s)
    {
        if (ControlFactors.PLAYER_CAN_CAST)
        {
            // Debug.Log("Good Spell Selected");
            goodSpellChoice = (GoodSpellType)s;
            spellTypeSelected = true;
            spellDragging = true;
            spellUseIdentifier.GetComponent<SpriteRenderer>().enabled = true;
            spellUseIdentifier.GetComponent<SpriteRenderer>().color = Color.blue;
            uih.selectedBox.GetComponent<Image>().enabled = true;
        }
    }
    

    public int SpellToType(Spell s)
    {
        for(int i = 0; i < (int)GoodSpellType.NumberOfTypes; i++)
        {
            if (s.GetType() == _GoodSpells[i].GetType())
                return i;
        }
        return 0;
    }
    
}
