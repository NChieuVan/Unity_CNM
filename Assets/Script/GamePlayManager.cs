using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] Cars;
    public GameObject[] CityLevels;
    
    void Start()
    {
        GameObject currentLevel = CityLevels[PlayerPrefs.GetInt("LevelNumber") - 1];
        currentLevel.SetActive(true);
        GameObject currentCar = Instantiate(Cars[PlayerPrefs.GetInt("CarNumber")]); 
        currentCar.transform.position = currentLevel.transform.GetChild(0).transform.position;
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
