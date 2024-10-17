using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    
    [Header("MainMenu")]
    public GameObject exitPanel, BackLevelSelection;
    public GameObject Menupanel;

    [Space(5)]
    [Header("carSelection")]
    public GameObject carSelection;
    public GameObject[] CarList;

    [Header("LevelSelection")]
    public GameObject LevelSelection;
    
    [Header("General")]
    public GameObject canvas;

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MainMenu
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
        carSelection.SetActive(true);
        Menupanel.SetActive(false );
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        CarList[counter].SetActive(true);
    }

    #endregion

    #region LevelSelection
    public void BackFromlevelSelection()
    {
        Menupanel.SetActive(true );
        LevelSelection.SetActive(false);
    }
    public void BackFromByHome(){
        carSelection.SetActive(false);
        Menupanel.SetActive(true);
        CarList[counter].SetActive(false);

    }


    #endregion

    #region CarSelection
    int counter = 0;
    public void NextCar()
    {
        if(counter != CarList.Length -1)
        {
            counter++;
        }else
        {
            counter =0;
        }
        foreach (var item in CarList)
        {
            item.SetActive(false);
        }
        CarList[counter].SetActive(true);
    }

    public void PreviousCar()
    {
        if(counter == 0)
        {
            counter = CarList.Length - 1;
        }
        else
        {
            counter--;
        }
        foreach (var item in CarList)
        {
            item.SetActive(false);
        }
        CarList[counter].SetActive(true);
    }

    #endregion
}

