using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Man : MonoBehaviour
{

    public float speed;
    public float restTime;
    public int level;

    public ParticleSystem celebrate;
    public ParticleSystem altarOffering;
    public ParticleSystem suprise;
    public EmotionPopup emotionPopupPrefab;
    public GameObject gravestone;

    public ManBrain brain;
    public Rigidbody2D rb;
    public Animator ani;
    public ManSprite currentModel;

    public GridTester gt;
    public ManSpriteList mSpriteList;
    public Canvas canvasUI;
    public Camera mainCamera;

    public AIPath path;
    public Seeker seeker;
    public MapTile homeTile;

    MapTile targetTile;
    Vector3 home;

    bool resting;
    
    EmotionPopup ePop;
    bool statMenuOpen = false;
    bool emotionPopupOpen = false;
    bool forceEmotionPopup = false;

    public PlayerState state = PlayerState.atHome;

    public enum PlayerState
    {
        atHome,
        atDestination,
        atAltar,
        travelToDestination,
        travelToHome,
        travelToAltar

    }

    // Start is called before the first frame update
    void Start()
    {
        gt = GameObject.FindGameObjectWithTag("GridController").GetComponent<GridTester>();
        mSpriteList = GameObject.FindGameObjectWithTag("ManSpriteList").GetComponent<ManSpriteList>();
        canvasUI = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        path = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        brain = new ManBrain(this);

        homeTile = transform.parent.GetComponent<MapTile>();
        home = homeTile.spawnPos;

        resting = false;
        seeker.StartPath(ABPath.Construct(transform.position, home, null));
        SetSeeker(false);
        path.maxSpeed = speed;

        ani.Play("Idle", 0, Random.Range(0, ani.GetCurrentAnimatorStateInfo(0).length));
    }


    // Update is called once per frame
    private void Update()
    {
        // Setting Animation
        float m = path.velocity.magnitude;
        if (path.velocity.x < 0)
            m *= -1;
        ani.SetFloat("Velocity", m);
    }
    private void FixedUpdate()
    {
        //Starts player state machine
        if (!resting)
        {
            playerStateMachine();
        }
    }

    

    private void playerStateMachine() // Controls the player states and the sequence of actions
    {
        if (path.canSearch) // In transit
        {
            if(path.reachedEndOfPath)// Has reached target position
            {
                //Debug.Log("Destination Reached");
                SetSeeker(false);
                PlanAction(state);
            }
        } else // Needs Action
        {
            //Debug.Log("Rested: choosing action");
            PlanAction(state);
        }
    }

    IEnumerator Rest(float rest)
    {
        resting = true;
        yield return new WaitForSeconds(Random.Range(rest/2,rest));
        resting = false;
    }

    IEnumerator WaitToRest(float time,bool rest)
    {
        yield return new WaitForSeconds(time);
        resting = rest;
        //Debug.Log("resting set false");
    }

    private void SetSeeker(bool canSearch)
    {
        path.canSearch = canSearch;
    }

    private void PlanAction(PlayerState setAction) // Sets the man's course of action depending on its current state
    {
        state = setAction;
        switch (state)
        {
            case PlayerState.atHome: // Was at Home, Now chooses Next activity
                {
                    float randomChoice = Random.Range(0, 10);
                    if (randomChoice < 5) // Walk to random location (5/10)
                    {
                        state = PlayerState.travelToDestination;

                        resting = false;

                        SetSeeker(true);
                        targetTile = gt.targetRandomMapTilePosition(this, home, level);
                        seeker.StartPath(ABPath.Construct(transform.position, targetTile.spawnPos));
                    }
                    else if (randomChoice == 5) // Walk to altar (1/10)
                    {
                        state = PlayerState.travelToAltar;

                        resting = false;

                        SetSeeker(true);
                        seeker.StartPath(ABPath.Construct(transform.position, gt.AltarPos));
                    }
                    else // Do Nothing
                    {
                        StartCoroutine(Rest(restTime));
                    }
                }
                break;
            case PlayerState.atDestination: //Was at Desination, goes home
                {
                    state = PlayerState.travelToHome;

                    resting = false;

                    SetSeeker(true);
                    seeker.StartPath(ABPath.Construct(transform.position, home, null));
                }
                break;
            case PlayerState.atAltar: //Was at Altar, goes home
                {
                    state = PlayerState.travelToHome;

                    resting = false;

                    SetSeeker(true);
                    seeker.StartPath(ABPath.Construct(transform.position, home, null));
                }
                break;
            case PlayerState.travelToDestination: 
                {
                    state = PlayerState.atDestination;

                    //Decides if the target tile will get an update
                    if (gt.ChanceForTileUpdate(targetTile))
                    {
                        targetTile.ChangeTileStage(1);
                        Celebrate(1);
                    }
                    StartCoroutine(Rest(restTime/2));
                }
                break;
            case PlayerState.travelToHome:
                {
                    state = PlayerState.atHome;

                    StartCoroutine(Rest(restTime));

                    Celebrate(1);
                }
                break;
            case PlayerState.travelToAltar:
                {
                    state = PlayerState.atAltar;

                    //Now decides what to do at altar
                    float randomChoice = Random.Range(1, 4);

                    switch (randomChoice)
                    {
                        case 0:
                            { // attempts to load option scene
                                if (!SceneLoader.optionSceneActive())
                                {
                                    SceneLoader.loadOptionScene();
                                    Debug.Log("Option Loaded");
                                }
                            }
                            break;
                        case 1:
                            { // Sacrifice
                                brain.changeFear(1);
                                ControlFactors.ChangeEnergy(3 * (1 + (ControlFactors.EMOTION / 100f)));
                                Celebrate(2);
                            }
                            break;
                        case 2: // Tribute
                            {
                                brain.changeFear(1);
                                ControlFactors.ChangeEnergy(3 * (1 + (ControlFactors.EMOTION / 100f)));
                                Celebrate(2);
                            }
                            break;
                        case 3: // Sacrifice/Tribute Teir 2
                            {
                                brain.changeBestTrait(3);
                                ControlFactors.ChangeEnergy(5 * (1 + (ControlFactors.EMOTION / 100f)));
                                Celebrate(2);
                            }
                            break;
                    }
                    StartCoroutine(Rest(restTime/2));
                }
                break;

        }
    }

    private void Celebrate(int choice)
    {
        ani.SetTrigger("Celebration");
        switch (choice)
        {
            case 1: // Normal
                {
                    celebrate.Stop();
                    celebrate.Play();
                }
                break;
            case 2: // Altar
                {
                    altarOffering.Stop();
                    altarOffering.Play();
                }
                break;
        }
    }

    public void setStatMenuOpen(bool openMenu)
    {
        statMenuOpen = openMenu;
        if (openMenu) // Close all other UI's
        {

        }
    }

    private void OnMouseEnter()
    {
        toggleEmotionPopup();
    }

    private void OnMouseExit()
    {
        toggleEmotionPopup();
    }

    public void toggleEmotionPopup()
    {
        if (!forceEmotionPopup)
        {
            if (!emotionPopupOpen)
            {
                ePop = Instantiate(emotionPopupPrefab, mainCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 2f, 0f)), Quaternion.identity);
                ePop.man = this;
                ePop.transform.SetParent(canvasUI.transform);
                ePop.transform.SetAsFirstSibling();
                emotionPopupOpen = true;
            }
            else
            {
                ePop.close();
                emotionPopupOpen = false;
            }
        }
    }

    public void toggleEmotionPopup(bool forceOpen)
    {
        forceEmotionPopup = forceOpen;
        if (forceOpen && !emotionPopupOpen) // Open if not already open
        {
            ePop = Instantiate(emotionPopupPrefab, mainCamera.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 2f, 0f)), Quaternion.identity);
            ePop.man = this;
            ePop.transform.SetParent(canvasUI.transform);
            ePop.transform.SetAsFirstSibling();
            emotionPopupOpen = true;
        }
        else if(!forceOpen && emotionPopupOpen) // Close if not aleady closed
        {
            ePop.close();
            emotionPopupOpen = false;
        }
    }

    public void summonDeath()
    {
        gt.population.populationList.Remove(this);
        toggleEmotionPopup(false);
        Instantiate(gravestone, transform.position, Quaternion.identity);
        homeTile.zeroTile();
    }

}
