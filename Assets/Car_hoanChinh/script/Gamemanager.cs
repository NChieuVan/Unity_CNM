using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public Controller RR;
    public GameObject needle;
    public Text kph;
    private float startPosition = 220f, endPosition = -49f;
    private float desiredPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        kph.text = RR.KPH.ToString("0");
        updateNeedle();
    }
   
    public void updateNeedle()
    {
        desiredPosition = startPosition - endPosition;
        float temp = RR.KPH / 180;
        needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));
    }
   
}
