using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Randomizer
{
    public static int RandomSide() 
    {
        return Random.Range(0,2); 
    }
}
