using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void StarGame(string player)
    {
        PlayerPrefs.SetString("SelectedPlayer", player);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
