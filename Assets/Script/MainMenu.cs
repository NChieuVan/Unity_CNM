using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("MainMenu")]
    public GameObject exitPanel, BackLevelSelection;
    public GameObject Menupanel;
    public GameObject Loading;

    [Space(5)]
    [Header("carSelection")]
    public GameObject carSelection;
    public GameObject[] CarList;

    [Header("LevelSelection")]
    public GameObject LevelSelection;

    [Header("General")]
    public GameObject canvas;

    [Header("LevelSelected")]
    public GameObject levelSelected;
    public GameObject[] LevelList;



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
        Menupanel.SetActive(false);
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
        CarList[counter].SetActive(true);
    }

    #endregion

    #region LevelSelection
    public void BackFromlevelSelection()
    {
        Menupanel.SetActive(true);
        LevelSelection.SetActive(false);
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void LevelSelect(int levelNo)
    {
        PlayerPrefs.SetInt("LevelNumber", levelNo);
        Loading.SetActive(true);
        //GameObject LevelList = PlayerPrefs.SetInt("LevelNumber", levelNo);


        //Gọi hàm chờ 3s để hiển thị màn hình mới
        StartCoroutine(LoadLevelAfterDelay(null,null));


    }

    private IEnumerator LoadLevelAfterDelay(GameObject Xe, GameObject level)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GamePlay");
        ///
        //set tọa độ level
        // chọn level
    }

    public void BackFromByHome()
    {
        carSelection.SetActive(false);
        Menupanel.SetActive(true);
        CarList[counter].SetActive(false);

    }


    #endregion

    #region CarSelection
    int counter = 0;
    public void NextCar()
    {
        if (counter != CarList.Length - 1)
        {
            counter++;
        }
        else
        {
            counter = 0;
        }
        foreach (var item in CarList)
        {
            item.SetActive(false);
        }
        CarList[counter].SetActive(true);
    }

    public void PreviousCar()
    {
        if (counter == 0)
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
    public void NextToCarSelection()
    {

        // Tắt giao diện menu chính
        Menupanel.SetActive(false);

        // Hiển thị giao diện chọn xe
        carSelection.SetActive(false);

        // Hiển thị giao diện chọn màn chơi
        LevelSelection.SetActive(true);

        PlayerPrefs.SetInt("CarNumber", counter);
        print(PlayerPrefs.GetInt("CarNumber" + 1));

        // Chuyển chế độ render của canvas nếu cần
        canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        // Tắt xe đang chọn để tránh nhầm lẫn
        foreach (var item in CarList)
        {
            item.SetActive(false);
        }


    }

    #endregion

    #region Level_Selected

    public int level = 1;
    public double[] PositionLevelSelected()
    {
        //double[]
        //return [1.0, 2, 3];
        return null;
    }
    public void LevelSelected()
    {
      
    }
    #endregion
}

