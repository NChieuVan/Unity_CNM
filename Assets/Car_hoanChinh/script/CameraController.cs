using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCon : MonoBehaviour
{
    private Controller controller;
    public GameObject Player;
    public GameObject child;
    public float speed;
    public float defaltPOV = 60, desiredFOV = 85;
    [Range(0, 2)] public float smothTime = 0;

    public Vector3 leftViewOffset = new Vector3(-3, 1.5f, -5); // Vị trí camera ở góc trái
    public Vector3 rightViewOffset = new Vector3(3, 1.5f, -5); // Vị trí camera ở góc phải
    public float transitionSpeed = 5f; // Tốc độ chuyển đổi camera
    private bool isViewingLeft = false; // Chế độ xem từ góc trái
    private bool isViewingRight = false; // Chế độ xem từ góc phải
    private bool isFirstPersonView = false; // Chế độ góc nhìn thứ nhất

    public Transform firstPersonPosition; // Vị trí camera cho góc nhìn thứ nhất (ví dụ: từ ghế lái)
    public float firstPersonOffsetHeight = 1.5f; // Độ cao của góc nhìn thứ nhất (từ ghế lái)

    void Start()
    {
    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        child = Player.transform.Find("camera constraint").gameObject;
        controller = Player.GetComponent<Controller>();
        defaltPOV = Camera.main.fieldOfView;
    }

    private void FixedUpdate()
    {
        // Kiểm tra các trạng thái phím
        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.A)) // Chuyển sang góc trái
            {
                isViewingLeft = true;
                isViewingRight = false;
            }
            else if (Input.GetKey(KeyCode.D)) // Chuyển sang góc phải
            {
                isViewingLeft = false;
                isViewingRight = true;
            }
        }
        else
        {
            isViewingLeft = false;
            isViewingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.F)) // Nhấn F để chuyển sang góc nhìn thứ nhất
        {
            isFirstPersonView = !isFirstPersonView; // Chuyển đổi chế độ góc nhìn thứ nhất
        }

        if (isFirstPersonView)
        {
            switchToFirstPersonView(); // Chuyển sang góc nhìn thứ nhất (khóa camera)
        }
        else
        {
            if (isViewingLeft)
            {
                switchToSideView(leftViewOffset); // Góc trái
            }
            else if (isViewingRight)
            {
                switchToSideView(rightViewOffset); // Góc phải
            }
            else
            {
                follow(); // Chế độ mặc định (camera theo xe)
            }
        }

        boostFOV();
        speed = (controller.KPH >= 50) ? 20 : controller.KPH / 4;
    }

    private void follow()
    {
        speed = Mathf.Lerp(speed, controller.KPH / 2, Time.deltaTime);
        gameObject.transform.position = Vector3.Lerp(transform.position, child.transform.position, Time.deltaTime * speed);

        gameObject.transform.LookAt(Player.gameObject.transform.position);
    }
    
    private void boostFOV()
    {
        if (isFirstPersonView)
        {
            Camera.main.fieldOfView = 77;
        }
        else
        {
            // Khi không ở góc nhìn thứ nhất, áp dụng hiệu ứng tăng FOV khi nhấn Shift.
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, desiredFOV, Time.deltaTime * smothTime);
            }
            else
            {
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, defaltPOV, Time.deltaTime * smothTime);
            }
        }
    }

    private void switchToSideView(Vector3 sideOffset)
    {
        // Tính toán vị trí mới cho camera theo hướng cạnh xe
        Vector3 targetPosition = Player.transform.position +
                                 Player.transform.right * sideOffset.x +
                                 Player.transform.up * sideOffset.y +
                                 Player.transform.forward * sideOffset.z;

        // Di chuyển camera đến vị trí target
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * transitionSpeed);

        // Camera nhìn vào xe
        transform.LookAt(Player.transform.position + Vector3.up * 1.5f); // Nhìn vào trung tâm xe (có chút nâng cao)
    }

    private void switchToFirstPersonView()
    {
        // Kiểm tra và đặt camera vào vị trí cố định cho góc nhìn thứ nhất
        if (firstPersonPosition != null)
        {
            // Khóa vị trí camera vào vị trí góc nhìn thứ nhất
            transform.position = firstPersonPosition.position + Vector3.up * firstPersonOffsetHeight;

            // Đảm bảo camera giữ nguyên hướng nhìn (không bị ảnh hưởng bởi lực hoặc tốc độ xe)
            transform.rotation = firstPersonPosition.rotation;
        }
    }

}
