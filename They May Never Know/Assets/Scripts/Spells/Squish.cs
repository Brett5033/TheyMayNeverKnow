using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : Spell
{
    protected override void Awake()
    {
        base.Awake();
        Collider2D[] hitBoxes = sampleArea(transform.position, 1);
        if (hitBoxes.Length > 0)
        {
            spellUsed = true;
            Man hitMan = hitBoxes[0].GetComponent<Man>();
            hitBoxes[0].GetComponent<Man>().suprise.Stop();
            hitBoxes[0].GetComponent<Man>().suprise.Play();
            hitBoxes[0].GetComponent<Man>().ani.SetTrigger("Squish");
        }
        else
        {
            spellUsed = false;
        }
    }
}
