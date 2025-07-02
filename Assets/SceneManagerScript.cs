using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using GamePlay.Script;
public class SceneManagerScript : MonoBehaviour
{
    public Dictionary<string, string> nameVideo = new Dictionary<string, string>()
    {
        { "sunset_butttttttt", "Phone1" },
        { "joakim", "Phone2" },
        { "music", "Phone3" },
        { "satara", "Phone4" }
    };
    public void SwitchScreen(string nameSong)
    {
        Date.NameSong = nameSong;
        Date.NameVideo = nameVideo[nameSong];
        SceneManager.LoadScene("GamePlay");
    }
}
