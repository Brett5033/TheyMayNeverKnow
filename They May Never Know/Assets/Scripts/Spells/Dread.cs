using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dread : Spell
{
    protected override void Awake()
    {
        base.Awake();
        Collider2D[] hitBoxes = sampleArea(transform.position, 1);
        if (hitBoxes.Length > 0)
        {
            spellUsed = true;
            Man hitMan = hitBoxes[0].GetComponent<Man>();
            hitMan.brain.changeFear(5f);
            hitMan.celebrate.Stop();
            hitMan.celebrate.Play();
        }
        else 
        {
            spellUsed = false;
        }
    }
}
