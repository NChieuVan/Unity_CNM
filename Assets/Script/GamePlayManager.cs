// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GamePlayManager : MonoBehaviour
// {
//     // Start is called before the first frame update
//     public GameObject[] Cars;
//     public GameObject[] CityLevels;
    
//     void Start()
//     {
//         GameObject currentLevel = CityLevels[PlayerPrefs.GetInt("LevelNumber") - 1];
//         currentLevel.SetActive(true);
//         GameObject currentCar = Instantiate(Cars[PlayerPrefs.GetInt("CarNumber")]); 
//         currentCar.transform.position = currentLevel.transform.GetChild(0).transform.position;
//         //Debug.. 
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GamePlayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Cars;
    public GameObject[] CityLevels;
    
    public GameObject PausePanel;
    public GameObject CompletePanel;
    void Start()
    {
        GameObject currentLevel = CityLevels[PlayerPrefs.GetInt("LevelNumber") - 1];
        currentLevel.SetActive(true);
        GameObject currentCar = Instantiate(Cars[PlayerPrefs.GetInt("CarNumber")]); 
        currentCar.transform.position = currentLevel.transform.GetChild(0).transform.position;
        //Debug.. 
    }


    // public void LevelComplete(){

    //     CompletePanel.SetActive(true);
    //     LevelUnlock();
    //     Time.timeScale = 0;
    // }

    


    public void LevelPause(){
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume(){
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void Home(){
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
