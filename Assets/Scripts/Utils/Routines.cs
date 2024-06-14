using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Routines 
{
    public static IEnumerator WaitAndExecute(float waitTime, System.Action callback)
    {
        yield return new WaitForSeconds(waitTime);
        callback();
    }

}
