using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    //[Header("LevelSelected")]
    //public GameObject levelSelected;
    //public GameObject[] LevelList;



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

    
    
    /*public void LevelSelect(int levelNo)
    {
        PlayerPrefs.SetInt("LevelNumber", levelNo);

        Loading.SetActive(true);
        

        // Gọi hàm chờ 3s để hiển thị màn hình mới
        StartCoroutine(LoadLevelAfterDelay());
    }*/

    public int level_car ;
    Vector3 desiredPosition= new Vector3(91.3f, 4.0f, 930f);
    public GameObject Car;

    // Các phương thức để chọn cấp độ
    public void GetLevel_1() { SetLevel(1); }
    public void GetLevel_2() { SetLevel(2); }
    public void GetLevel_3() { SetLevel(3); }
    public void GetLevel_4() { SetLevel(4); }
    public void GetLevel_5() { SetLevel(5); }
    public void SetLevel(int level)
    {
        level_car = level;
        Debug.Log(level_car + "_0000");
        Loading.SetActive(true);
        StartCoroutine(LoadLevelAfterDelay());
    }
    private IEnumerator LoadLevelAfterDelay()
    {
        // Thiết lập vị trí dựa trên cấp độ
        switch (level_car)
        {
            case 1:
                desiredPosition = new Vector3(91.3f, 4.0f, 935f);
                break;
            case 2:
                desiredPosition = new Vector3(100f, 4.0f, 923.0f);
                Debug.Log(level_car + "2_0000");
                break;
            // Thêm các trường hợp cho các cấp độ khác nếu cần
            case 3:
                desiredPosition = new Vector3(110f, 5f, 910f);
                break;
            case 4:
                desiredPosition = new Vector3(120f, 6f, 905f);
                break;
            case 5:
                desiredPosition = new Vector3(130f, 7f, 900f);
                break;
            default:
                desiredPosition = Vector3.zero; // Giá trị mặc định
                break;
        }
        
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GamePlay");
        Car.transform.position = desiredPosition;

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GamePlay")
        {
            // Di chuyển xe đến vị trí đã chỉ định
            if (Car != null)
            {
                Car.transform.position = desiredPosition;
                Debug.Log("Car Position Set To: " + desiredPosition); // In ra vị trí xe
            }
            Loading.SetActive(false); // Ẩn màn hình loading
        }
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

   }

