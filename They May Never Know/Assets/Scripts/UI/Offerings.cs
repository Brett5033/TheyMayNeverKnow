using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Offerings
{
    // a list of available offers
    public static Offer[] offeringList1 =
    {
        //new Offer("?", -1f, 1f, 1f),
        new Offer("Should we consume the wild mushrooms of the forest?", -10f, 10f, 10f),
        new Offer("Are the dragonbs friendly?", -10f, 10f, 20f),
        new Offer("If we jump out the castle window, will it hurt?", 10f, -10f, 10f),
        new Offer("Should the blacksmith make sharper tools?", 10f, 5f, 15f),
        new Offer("Is cow poop edible?", -15f, 10f, 10f),
        new Offer("Can we harvest the sun?", -10f, 0f, 5f),
        new Offer("Are you real?", 10f, 10f, -30f),
        new Offer("Dont exist", 0f, 0f, -20f),

    };
    

    public static Offer getRandomOffer(Offer[] offArray)
    {
        int randVal = Random.Range(0, offArray.Length);
        return offArray[randVal];
    }

}


public class Offer
{
    public float yesVal, noVal, ignoreVal;
    public string Offering;

    //  The offer and then the chnage for each value: pos values add to love and reduce fear, negative values add to fear and reduce love; Silence reduces admiration but not fear or love;
    public Offer(string o, float y, float n, float i)
    {
        yesVal = y;
        noVal = n;
        ignoreVal = i;
        Offering = o;
    }
}