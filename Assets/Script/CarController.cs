using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{


    public GameObject car; // Đối tượng xe
    public Vector3 desiredPosition = new Vector3(155f, 3.45f, 40f); // Vị trí mong muốn


    void Start()
    {
        int selectedLevel = PlayerPrefs.GetInt("SelectedLevel");

        switch (selectedLevel)
        {
            case 1:
                desiredPosition = new Vector3(155f, 3.45f, 46f); // Vị trí cho Level 1
                break;
            case 2:
                desiredPosition = new Vector3(155f, 3.45f, 57.1f); // Vị trí cho Level 2
                break;

            case 3:
                desiredPosition = new Vector3(147f, 3.45f, 52f); // Vị trí cho Level 2
                break;
            default:

                break;
        }

        // Đặt vị trí cho xe khi scene được tải
        if (car != null)
        {
            car.transform.position = desiredPosition;
            Debug.Log("Car position set to: " + desiredPosition);
        }
        else
        {
            Debug.LogError("Car is not assigned in the Inspector!");
        }
    }
}
