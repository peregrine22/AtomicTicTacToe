using UnityEngine;

public class Board : MonoBehaviour
{
    Box[] boardBoxes;

    public Box[] GetBoardBoxes() { return boardBoxes; }


    private void Awake()
    {
        boardBoxes = gameObject.transform.GetComponentsInChildren<Box>();
    }

}
