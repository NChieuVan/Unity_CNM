using UnityEngine;

public class BrakeLightController : MonoBehaviour
{
    public GameObject rearLightLeft;  // Đèn phanh trái
    public GameObject rearLightRight; // Đèn phanh phải
    public Material brakeLightOnMaterial; // Material sáng
    public Material brakeLightOffMaterial; // Material tắt
    public Rigidbody carRigidbody;  // Rigidbody của xe

    private Renderer leftLightRenderer;
    private Renderer rightLightRenderer;

    void Start()
    {
        // Lấy Renderer từ đèn
        leftLightRenderer = rearLightLeft.GetComponent<Renderer>();
        rightLightRenderer = rearLightRight.GetComponent<Renderer>();
    }

    void Update()
    {
        // Kiểm tra nếu người chơi phanh xe (dựa vào Input hoặc tốc độ giảm đột ngột)
        if (Input.GetKey(KeyCode.Space) || carRigidbody.velocity.magnitude < 0.1f)
        {
            // Chuyển sang Material sáng
            leftLightRenderer.material = brakeLightOnMaterial;
            rightLightRenderer.material = brakeLightOnMaterial;
        }
        else if (Vector3.Dot(carRigidbody.velocity,transform.forward)<0)
        {
            // Chuyển sang Material sáng
            leftLightRenderer.material = brakeLightOnMaterial;
            rightLightRenderer.material = brakeLightOnMaterial;
        }
        else
        {
            // Chuyển sang Material tắt
            leftLightRenderer.material = brakeLightOffMaterial;
            rightLightRenderer.material = brakeLightOffMaterial;
        }
    }
}