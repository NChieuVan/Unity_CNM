using UnityEngine;

public class HeadlightController : MonoBehaviour
{
    public GameObject leftHeadlight; // Đèn pha bên trái
    public GameObject rightHeadlight; // Đèn pha bên phải
    public Light leftLight; // Ánh sáng Spot Light bên trái
    public Light rightLight; // Ánh sáng Spot Light bên phải
    public Material headlightOffMaterial; // Material đèn tắt
    public Material headlightOnMaterial; // Material đèn sáng

    private bool isHeadlightOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)) // Nhấn phím L để bật/tắt đèn
        {
            isHeadlightOn = !isHeadlightOn;

            // Bật hoặc tắt ánh sáng Spot Light
            leftLight.enabled = isHeadlightOn;
            rightLight.enabled = isHeadlightOn;

            // Đổi Material
            leftHeadlight.GetComponent<Renderer>().material = isHeadlightOn ? headlightOnMaterial : headlightOffMaterial;
            rightHeadlight.GetComponent<Renderer>().material = isHeadlightOn ? headlightOnMaterial : headlightOffMaterial;
        }
    }
}