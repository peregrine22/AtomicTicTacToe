using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    GameObject playerPawn;

    [SerializeField]
    Mark mark;

    public GameObject PlayerPawn { get { return playerPawn; } }

    public Mark Mark { get { return mark; } }

    public override string ToString()
    {
        return playerPawn.name + " ; " + mark.ToString();
    }
}
