using UnityEngine.SceneManagement;
using UnityEngine;
using GamePlay.Script;
public class SceneManagerScript : MonoBehaviour
{
    public void SwitchScreen(string nameSong)
    {
        SceneManager.LoadScene("GamePlay");
        Date.NameSong = nameSong;
    }
}
