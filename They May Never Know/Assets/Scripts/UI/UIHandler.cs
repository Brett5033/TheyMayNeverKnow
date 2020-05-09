using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public TMPro.TextMeshProUGUI _DevelopmentScore;
    public TMPro.TextMeshProUGUI _Love;
    public TMPro.TextMeshProUGUI _Fear;
    public TMPro.TextMeshProUGUI _PassRate;
    public TMPro.TextMeshProUGUI _EnergyCharges;
    public TMPro.TextMeshProUGUI _Time;
    public Slider _EnergySlider;
    public Slider _EmotionBar;

    public ControlHandler ch;
    public PlayerSpellCaster spellCaster;
    public Canvas uiCanvas;

    public ManMenu manMenuPrefab;
    public bool manMenuOpen;
    public LayerMask whatIsMan;

    public GameObject goodSpellQueue;
    public GameObject selectedBox;
    public GameObject[] spellButtonPrefabs;
    

    public List<GameObject> queuedSpellButtons = new List<GameObject>();

    public int queueItemsShown;
    public float queueButtonSize;

    void FixedUpdate()
    {
        //DisplayUI
        
        spellCaster = GameObject.FindGameObjectWithTag("SpellCaster").GetComponent<PlayerSpellCaster>();
        _DevelopmentScore.SetText("Development Score: " + ControlFactors.DEVELOPMENT_SCORE.ToString());
        _Love.SetText("Love: " + ((ControlFactors.LOVE / ControlFactors.EMOTION)*100f).ToString("F0") + "%");
        _Fear.SetText("Fear: " + ((ControlFactors.FEAR / ControlFactors.EMOTION)*100f).ToString("F0") + "%");
        _PassRate.SetText("Pass Rate: " + ControlFactors.PASS_RATE.ToString());
        _EnergySlider.value = ((ControlFactors.ENERGY % ControlFactors.MAX_ENERGY) / ControlFactors.MAX_ENERGY);
        _EnergyCharges.SetText(((int)ControlFactors.ENERGY / (int)ControlFactors.MAX_ENERGY).ToString("F2"));
        _EmotionBar.value = (ControlFactors.LOVE / ControlFactors.EMOTION);
        _Time.SetText("Time: " + getTimeInHours());

        // Checks if the Good Spell Queue needs to be updated
        if(spellCaster.spellQueue.Count > queuedSpellButtons.Count && queuedSpellButtons.Count < queueItemsShown)
        {
            addToQueue();
        }
    }
    

    private void Update()
    {
        //Handle Scene temp
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(SceneLoader.curScene!= SceneLoader.Scene.option)
            {
                SceneLoader.loadOptionScene();
            }
            else
            {
                SceneLoader.unloadOptionScene();
            }
        }

        if (!manMenuOpen && Input.GetMouseButtonDown(1))
        {
            Collider2D m = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1, whatIsMan);
            if (m != null)
            {
                ManMenu newMenu = Instantiate(manMenuPrefab, new Vector3(535f, 350f, -20f), Quaternion.identity);
                newMenu.transform.SetParent(uiCanvas.transform);
                newMenu.man = m.GetComponent<Man>();
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ManualCamera>().setTarget(m.GetComponent<Man>());
                manMenuOpen = true;
            }
        }
    }

    public void addToQueue()
    {
        //Debug.Log(queuedSpellButtons.Count);
        GameObject g = Instantiate(spellButtonPrefabs[spellCaster.SpellToType(spellCaster.spellQueue[queuedSpellButtons.Count])], goodSpellQueue.transform.position, Quaternion.identity);
        g.transform.SetParent(goodSpellQueue.transform);
        g.transform.position = new Vector3(g.transform.position.x-10, g.transform.position.y - (queuedSpellButtons.Count * queueButtonSize + 10), 0);
        if (queuedSpellButtons.Count != 0)
        {
            g.GetComponent<Button>().interactable = false;
        }
        queuedSpellButtons.Add(g);
        
        //g.GetComponent<Button>().onClick.AddListener{
        //    => spellCaster.setGoodSpell(spellCaster.SpellToType(spellCaster.spellQueue[queuedSpellButtons.Count-1]))
        //}
    }

    public void shiftQueue()
    {
        for(int i = 0; i < queuedSpellButtons.Count; i++)
        {
            queuedSpellButtons[i].transform.position = new Vector3(queuedSpellButtons[i].transform.parent.transform.position.x - 10, queuedSpellButtons[i].transform.parent.transform.position.y - (i * queueButtonSize + 10), 0);
            if(i == 0)
            {
                queuedSpellButtons[i].GetComponent<Button>().interactable = true;
            } else
            {
                queuedSpellButtons[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    private string getTimeInHours()
    {
        int timeInHours = (int)Mathf.Floor((ControlFactors.CURRENT_TIME / ControlFactors.DAY_LENGTH) * 24f);
        if(timeInHours < 1f) // 12 AM
        {
            return (timeInHours+12).ToString("F0") + ":00 AM";
        }else if(timeInHours < 13f) // 12 PM
        {
            return timeInHours.ToString("F0") + ":00 AM";
        }
        else // PM
        {
            return (timeInHours % 12).ToString("F0") + ":00 PM";
        }
    }
    
}
