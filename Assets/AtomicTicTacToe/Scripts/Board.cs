using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    List<Box> boardBoxes;

    public List<Box> BoardBoxes { get { return boardBoxes; } }

    private void Awake()
    {
        boardBoxes = gameObject.transform.GetComponentsInChildren<Box>().ToList();
    }

    public List<Box> FindAvailableBoxes()
    {
        return boardBoxes.FindAll((box) => !box.IsMarked);
    }

    public override String ToString()
    {
        return string.Join(", ", boardBoxes.Select((box) => box.name));
    }

}
