using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    public enum DestinationTag
    {
        ENDLEVEL1, 
        STARTLEVEL2, 
        GAMEOVERSCENE
    }
    public DestinationTag destinationTag;
}
