using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

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

    //[Header("LevelSelected")]
    //public GameObject levelSelected;
    //public GameObject[] LevelList;
    public TagertLevel tagertLevel;



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


    // Các phương thức để chọn cấp độ
    public void GetLevel_1() { StartCoroutine(SetLevel(1)); }
    public void GetLevel_2() { StartCoroutine(SetLevel(2)); }

    public void GetLevel_3() { StartCoroutine(SetLevel(3)); }
    public void GetLevel_4() { StartCoroutine(SetLevel(4)); }
    public void GetLevel_5() { StartCoroutine(SetLevel(5)); }
    public void GetLevel_6() { StartCoroutine(SetLevel(6)); }
    public void GetLevel_7() { StartCoroutine(SetLevel(7)); }
    public void GetLevel_8() { StartCoroutine(SetLevel(8)); }
    public void GetLevel_9() { StartCoroutine(SetLevel(9)); }
    public void GetLevel_10() { StartCoroutine(SetLevel(10)); }


    private IEnumerator SetLevel(int level)
    {
        print("haah");
        PlayerPrefs.SetInt("SelectedLevel", level); // Lưu cấp độ đã chọn
        Debug.Log("Loading position screen...");

        Loading.SetActive(true);
        // Thêm độ trễ ở đây (ví dụ: 2 giây)
        yield return new WaitForSeconds(3f);

        tagertLevel.ShowCubesForLevel(level);

        SceneManager.LoadScene("GamePlay"); // Chuyển đến scene Position
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
    public static int ll;
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
        ll = counter;
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
        ll = counter;

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

}

