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
    public LevelManager level;
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
            noti.text = "Chúc mừng bạn đã vượt màn thành công!";
            
        }
       // if (collision.gameObject.CompareTag("hd"))
       // {
       //    Destroy(collision.gameObject);

       // }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu va chạm với đối tượng có tag "hd"
        if (other.CompareTag("hd"))
        {
            // Tắt collider để ngăn các va chạm tiếp theo
            other.enabled = false;

            // Tùy chọn: Thêm hiệu ứng biến mất (nếu cần)
            // Instantiate(effectPrefab, other.transform.position, Quaternion.identity);

            // Hủy đối tượng hoặc làm nó biến mất
            Destroy(other.gameObject);

           // Debug.Log("Va chạm với đối tượng 'hd', đối tượng đã bị hủy!");
        }
    }

    public void Yes_Tagert()
    {
        int l = PlayerPrefs.GetInt("SelectedLevel");
        print("------" + l);
        level.MarkLevelAsCompleted(l);
        SceneManager.LoadScene("SampleScene");
        
        //SceneManager.LoadScene("Position");
    }
    public void No_Tagert()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }


}
