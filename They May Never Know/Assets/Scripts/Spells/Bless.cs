using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless : Spell
{
    protected override void Awake()
    {
        base.Awake();
        Collider2D[] hitBoxes = sampleArea(transform.position, 1);
        if (hitBoxes.Length > 0)
        {
            //Debug.Log("Blessed");
            spellUsed = true;
            Man hitMan = hitBoxes[0].GetComponent<Man>();
            hitMan.brain.changeLove(5f);
            hitMan.altarOffering.Stop();
            hitMan.altarOffering.Play();
        }
        else
        {
            spellUsed = false;
        }
    }
}
