using UnityEngine;

public class Board : MonoBehaviour
{
    Box[] boardBoxes;

    public Box[] BoardBoxes { get { return boardBoxes; } }


    private void Awake()
    {
        boardBoxes = gameObject.transform.GetComponentsInChildren<Box>();
    }

}
