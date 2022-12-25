using UnityEngine;

public class Box : MonoBehaviour
{
    int index;
    Mark mark;
    bool isMarked;

    public int Index { get { return index; } }
    public Mark Mark { get { return mark; } }
    public bool IsMarked { get { return isMarked; } }

    GameObject occupyingPiece;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
        mark = Mark.None;
        isMarked = false;
    }

    public void SetAsMarked(GameObject playerPiece, Mark mark)
    {
        isMarked = true;
        this.mark = mark;

        occupyingPiece = Instantiate(playerPiece, transform.position, transform.rotation);
        GetComponent<BoxCollider>().enabled = false;
    }

    public void Clear()
    {
        isMarked = false;
        this.mark = Mark.None;
        Destroy(occupyingPiece);
        GetComponent<BoxCollider>().enabled = true;
    }
}
