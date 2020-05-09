using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nimbus : Spell
{
    public Man target;
    public float damageTotal;

    protected override void Awake()
    {
        base.Awake();
        Collider2D[] hitBoxes = sampleArea(transform.position, 3);
        if (hitBoxes.Length > 0)
        {
            //Debug.Log("Man found");
            spellUsed = true;
            target = hitBoxes[0].GetComponent<Man>();
            StartCoroutine(ApplyFearOverTime(target, duration - 1, 10));
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            spellUsed = false;
        }
    }

    public override void Update()
    {
        if (spellUsed)
        {
            //Debug.Log("updated");
            transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1.5f, 20);
        }
        
    }


}
