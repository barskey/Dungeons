using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    // returns true if random int chosen between 1 and chance was 1
    public static bool OneIn(int chance)
    {
        if (Random.Range(1, chance) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
