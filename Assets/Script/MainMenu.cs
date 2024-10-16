using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject exitPanel, Menupanel, LevelSelection,BackLevelSelection;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
        exitPanel.SetActive(true);
        Menupanel.SetActive(false);
    }

    public void YES()
    {
        Application.Quit();
    }
    public void NO()
    {
        Menupanel.SetActive(true);
        exitPanel.SetActive(false);
    }

    public void MoreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/developer?id=Broken+Diamond");
    }

    public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.brokendiamond.advance.car.parking.car.driver.simulator");
    }

    public void Play()
    {
        LevelSelection.SetActive(true);
        Menupanel.SetActive(false );
    }
    public void BackFromlevelSelection()
    {
        Menupanel.SetActive(true );
        LevelSelection.SetActive(false);
    }
}

