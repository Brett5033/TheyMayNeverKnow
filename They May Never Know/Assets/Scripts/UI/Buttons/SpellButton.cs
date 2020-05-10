using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpellButton : MonoBehaviour, IPointerDownHandler
{
    Button button = null;
    public bool isGood;
    public bool spellButton;
    public int type;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (!spellButton)
            button.onClick.AddListener(() => { generateGoodSpell(); });
    }

    public void Click()
    {/*
        if (isGood)
        {
            GameObject.FindGameObjectWithTag("SpellCaster").GetComponent<PlayerSpellCaster>().setGoodSpell(type);
        } else
        {
            GameObject.FindGameObjectWithTag("SpellCaster").GetComponent<PlayerSpellCaster>().setBadSpell(type);
        }*/
    }

    public void generateGoodSpell()
    {
        // Checks if a good spell should be queued
        if (ControlFactors.ENERGY >= ControlFactors.MAX_ENERGY)
        {
            GameObject.FindGameObjectWithTag("SpellCaster").GetComponent<PlayerSpellCaster>().queueSpell();
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (spellButton)
        {
            //((IPointerDownHandler)button).OnPointerDown(eventData);
            if (isGood)
            {
                GameObject.FindGameObjectWithTag("SpellCaster").GetComponent<PlayerSpellCaster>().setGoodSpell(type);
            }
            else
            {
                GameObject.FindGameObjectWithTag("SpellCaster").GetComponent<PlayerSpellCaster>().setBadSpell(type);
            }
        }
        
    }
}
