using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Colision_Xe_Tagert : MonoBehaviour
{
    public GameObject PLayer;
    public GameObject camvas;
    public Button Yes, No;
    public Text noti;
    // Start is called before the first frame update
    void Start()
    {
        Yes.onClick.AddListener(Yes_Tagert);
        No.onClick.AddListener(No_Tagert);
        camvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tagert"))
        {
            camvas.SetActive(true);
            noti.text = "Bạn đã vượt màng thành công!";
        }
    }

    public void Yes_Tagert()
    {
        SceneManager.LoadScene("SampleScene");
        //SceneManager.LoadScene("Position");
    }
    public void No_Tagert()
    {
        camvas.SetActive(false);
    }


}
