using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float spellCost;
    public float duration;
    public bool spellUsed = true;
    public bool BadSpell;
    public LayerMask whatIsHit;
    
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        if(duration != -1)
        {
            Destroy(gameObject, duration);
        } 
    }

    public virtual void Update()
    {
        
    }

    protected Collider2D[] sampleArea(Vector3 centerPos, float radius)
    {
        return Physics2D.OverlapCircleAll(centerPos, radius, whatIsHit);
    }

    protected IEnumerator ApplyFearOverTime(Man man,float duration, float totalApplied)
    {
        float currentDamageCount = 0;
        while(man.brain.Fear > 0 && currentDamageCount < totalApplied)
        {
            man.brain.changeFear(totalApplied / duration);
            yield return new WaitForSeconds(1f);
            currentDamageCount += (totalApplied / duration);
        }
    }
}
