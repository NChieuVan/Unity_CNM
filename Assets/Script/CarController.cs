using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CarController : MonoBehaviour
{


    public GameObject car; // Đối tượng xe
    public GameObject Camera;
    public Vector3 desiredPosition = new Vector3(155f, 3.45f, 40f); // Vị trí mong muốn
    public CarList carList;

    void Start()
    {
        


        carList = new CarList();
        carList.SetActice();
        carList.SetCammn();
        int ll = MainMenu.ll;
        car = carList.carList[ll];
        Camera = carList.CamereaMn[ll];
        
        carList.carList[ll].SetActive(true);
        carList.CamereaMn[ll].SetActive(true);
        

        // Đặt vị trí cho xe khi scene được tải
        if (car != null)
        {
            car.transform.position = desiredPosition;
            //car.transform.rotation = (0, 88.262f, 0);
            Debug.Log("Car position set to: " + desiredPosition);
        }
        else
        {
            Debug.LogError("Car is not assigned in the Inspector!");
        }
    }
}
