using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarList : MonoBehaviour
   
{
    public GameObject[] carList;
    public GameObject[] CamereaMn;
    private Vector3 desiredPosition = new Vector3(155f, 3.45f, 40f);
    private Quaternion desiredRotation = Quaternion.Euler(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        SetActice();
        SetCammn();
    }
    public void SetCammn()
    {
        foreach(var item in CamereaMn)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
        int ll = MainMenu.ll;
        CamereaMn[ll].SetActive(true);
    }

    public void SetActice()
    {
        if (carList == null || carList.Length == 0)
        {
            Debug.LogWarning("Car list is null or empty. Please assign cars in the Inspector.");
            return;
        }

        foreach (var item in carList)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
            else
            {
                Debug.LogWarning("A car in the list is null.");
            }
        }
        int ll = MainMenu.ll;
        carList[ll].SetActive(true);

        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel");

        switch (selectedLevel)
        {
            case 1:
                desiredPosition = new Vector3(114.0f, 4.0f, 46.0f); // Vị trí cho Level 1
                desiredRotation = Quaternion.Euler(0f, 88.262f, 0f);
                break;
            case 2:
                desiredPosition = new Vector3(91.33232f, 6.15f, 108.0734f); // Vị trí cho Level 2
                desiredRotation = Quaternion.Euler(0f, -19.275f, 0f);
                break;

            case 3:
                desiredPosition = new Vector3(91.33232f, 6.15f, 108.0734f); // Vị trí cho Level 2
                desiredRotation = Quaternion.Euler(0f, -19.275f, 0f);
                break;
            case 4:
                desiredPosition = new Vector3(114.0f, 4.0f, 46.0f); // Vị trí cho Level 1
                desiredRotation = Quaternion.Euler(0f, 88.262f, 0f); // Vị trí cho Level 4
                break;
            case 5:
                desiredPosition = new Vector3(601.5f, 8.85f, 414.43f); // Vị trí cho Level 5
                desiredRotation = Quaternion.Euler(8.416f, 6.154f, 0f);
                break;
            case 6:
                desiredPosition = new Vector3(601.5f, 8.85f, 414.43f); 
                desiredRotation = Quaternion.Euler(8.416f, 6.154f, 0f); // Vị trí cho Level 6
                break;
            case 7:
                desiredPosition = new Vector3(471.2f, 8.85f, 231f); 
                desiredRotation = Quaternion.Euler(8.416f, 6.154f, 0f);  // Vị trí cho Level 7
                break;
            case 8:
                desiredPosition = new Vector3(471.2f, 8.85f, 231f); 
                desiredRotation = Quaternion.Euler(8.416f, 6.154f, 0f); // Vị trí cho Level 8
                break;
            case 9:
                desiredPosition = new Vector3(590.22f, 11.41f,357.15f); 
                desiredRotation = Quaternion.Euler(3.245f, 6.154f, 0f); // Vị trí cho Level 9
                break;
            case 10:
                desiredPosition = new Vector3(590.22f, 11.41f,357.15f); 
                desiredRotation = Quaternion.Euler(3.245f, 6.154f, 0f); // Vị trí cho Level 10
                break;

            default:
                break;
        }
        if (carList[ll] != null)
        {
            carList[ll].transform.position = desiredPosition;
            carList[ll].transform.rotation = desiredRotation;
            Debug.Log("Car position set to: " + desiredPosition);
        }
        else
        {
            Debug.LogError("Car is not assigned in the Inspector!");
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
