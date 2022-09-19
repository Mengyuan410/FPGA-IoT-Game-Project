using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class TeamManager : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteRenderer sr;
    public List<Sprite> skins = new List<Sprite>();
    private int selectedSkin = 0;
    public GameObject playerSkin;
    public string Team;
    private void Start()
    {
        Team = "A"; 

    }
    public void NextOption()
    {
        selectedSkin = selectedSkin + 1;
        if(selectedSkin == skins.Count)
        {
            
            selectedSkin = 0;

        }
        sr.sprite = skins[selectedSkin];
        if (selectedSkin == 0)
        {
            Team = "A";
        }
        else
        {
            Team = "B";
        }
    }
    public void BackOption()
    {
        selectedSkin = selectedSkin -1;
        if (selectedSkin < 0)
        {
            
            Debug.Log("count" + skins.Count);
            selectedSkin = skins.Count - 1;
            Debug.Log("Baack" + selectedSkin);
        }
        sr.sprite = skins[selectedSkin];
        if (selectedSkin == 0)
        {
            Team = "A";
        }
        else
        {
            Team = "B";
        }
    }
}
