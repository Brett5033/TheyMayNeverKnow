using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunflower : Spell
{
    public float LovePerSecond;
    public float range;
    public GameObject Area;
    public ParticleSystem particles;

    float refreshRate = 1;
    float timer = 0;

    protected override void Awake()
    {
        base.Awake();
        Area.transform.localScale = new Vector3(5 * range, 5 * range, 1);
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.radius = range / 2;
        spellUsed = true;
        Collider2D[] hitBoxes = sampleArea(transform.position, range);
        if (hitBoxes.Length > 0)
        {
            //Debug.Log("Man found");
            foreach(Collider2D m in hitBoxes)
            {
                m.GetComponent<Man>().brain.changeLove(LovePerSecond / refreshRate);
            }
        }
    }

    public override void Update()
    {
        if (spellUsed)
        {
            if(timer >= refreshRate)
            {
                timer = 0;
                Collider2D[] hitBoxes = sampleArea(transform.position, range);
                if (hitBoxes.Length > 0)
                {
                    spellUsed = true;
                    foreach (Collider2D m in hitBoxes)
                    {
                        m.GetComponent<Man>().brain.changeLove(LovePerSecond / refreshRate);
                    }
                }
            } else
            {
                timer += Time.deltaTime;
            }
        }

    }
}
