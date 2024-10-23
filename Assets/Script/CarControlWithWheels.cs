using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControlWithWheels : MonoBehaviour
{
    public float speed = 5f;
    public float reverseSpeed = 5f;
    public float turnSpeed = 35f;
    public float maxSteeringAngle = 35f;
    public float wheelRotationSpeed = 300f;

    public float acceleration = 5f;
    public float maxSpeed = 35f;
    public float deceleration = 10f;
    public float naturalDrag = 2f;

    // Tham chiếu đến bánh xe
    public Transform frontLeftWheel;
    public Transform frontRightWheel;
    public Transform rearLeftWheel;
    public Transform rearRightWheel;

    // Đèn xe
    public Light frontLights;
    public Light rearLeftLight;
    public Light rearRightLight;

    private bool isBraking = false;
    private bool isFrontLightsOn = true; // Đèn trước luôn sáng

    private float currentSpeed = 0f;
    private float currentSteeringAngle = 0f;

    public float fadeSpeed = 2f;

    private Rigidbody rb;

    // Tham chiếu đến các camera
    public Camera cameraFront;     // Camera trước
    public Camera cameraBack;      // Camera sau
    public Camera cameraInterior;  // Camera trong nội thất

    private Camera currentCamera;   // Camera hiện tại

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = naturalDrag;
        rb.angularDrag = 1f; // Thêm drag góc cho realism khi quay xe
        frontLights.enabled = true; // Đèn trước luôn sáng
        rearLeftLight.enabled = false;
        rearRightLight.enabled = false;

        // Chọn camera mặc định (ví dụ: camera trước)
        currentCamera = cameraFront;
        ActivateCamera(cameraFront);
    }

    private void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");   // Nhấn lên hoặc xuống
        float turnInput = Input.GetAxis("Horizontal"); // Rẽ trái hoặc phải

        // Xử lý phanh
        if (Input.GetKey(KeyCode.Space))
        {
            Brake();  // Phanh xe khi nhấn phím Space
        }
        else if (Mathf.Abs(moveInput) > 0.1f)
        {
            // Kiểm tra nếu xe đang tiến hay lùi
            if (moveInput > 0)
            {
                MoveForward(moveInput);  // Di chuyển tiến
            }
            else if (moveInput < 0)
            {
                MoveBackward(moveInput); // Di chuyển lùi
            }

            RotateWheels(moveInput); // Quay bánh xe theo hướng di chuyển
        }
        else
        {
            Decelerate(); // Giảm tốc khi không có đầu vào
            RotateWheels(moveInput);
        }

        SteerWheels(turnInput); // Xoay bánh trước theo hướng rẽ

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            SteerVehicle(turnInput); // Xử lý rẽ thân xe
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleFrontLights();
        }

        // Kiểm tra nếu đang phanh hoặc lùi để bật đèn sau
        if (isBraking || moveInput < 0)
        {
            rearLeftLight.enabled = true;
            rearRightLight.enabled = true;
        }
        else
        {
            rearLeftLight.enabled = false;
            rearRightLight.enabled = false;
        }

        // Chuyển camera khi nhấn các phím C và V
        if (Input.GetKeyDown(KeyCode.C))
        {
            ActivateCamera(cameraFront); // Chuyển sang camera trước
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            ActivateCamera(cameraBack); // Chuyển sang camera sau
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            ActivateCamera(cameraInterior); // Chuyển sang camera trong nội thất
        }
    }



    // Xử lý phanh xe
    private void Brake()
    {
        float targetSpeed = currentSpeed > 0 ? 0 : -0; // Phanh về 0 hoặc -0 tuỳ hướng
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
        isBraking = true;
    }

    private void MoveForward(float moveInput)
    {
        isBraking = false;

        // Tăng tốc khi nhấn tiến
        currentSpeed += moveInput * acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed); // Giới hạn tốc độ tiến tối đa

        // Di chuyển xe theo tốc độ hiện tại
        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
    }

    private void MoveBackward(float moveInput)
    {
        isBraking = false;

        // Giảm tốc khi nhấn lùi
        currentSpeed += moveInput * acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, -reverseSpeed, 0); // Giới hạn tốc độ lùi tối đa

        // Di chuyển xe theo tốc độ hiện tại
        rb.MovePosition(transform.position + transform.forward * currentSpeed * Time.deltaTime);
    }

    private void Decelerate()
    {
        isBraking = false;

        if (currentSpeed > 0)
        {
            // Giảm tốc độ một cách tự nhiên khi tiến
            currentSpeed = Mathf.Lerp(currentSpeed, 0, naturalDrag * Time.deltaTime);
        }
        else if (currentSpeed < 0)
        {
            // Giảm tốc độ một cách tự nhiên khi lùi
            currentSpeed = Mathf.Lerp(currentSpeed, 0, naturalDrag * Time.deltaTime);
        }

        // Cập nhật vị trí xe theo tốc độ đã giảm dần
        Vector3 moveDirection = transform.forward * Mathf.Sign(currentSpeed);
        rb.MovePosition(transform.position + moveDirection * Mathf.Abs(currentSpeed) * Time.deltaTime);
    }

    private void SteerVehicle(float turnInput)
    {
        if (Mathf.Abs(turnInput) > 0.1f && Mathf.Abs(currentSpeed) > 0.1f)
        {
            float turnAmount = turnInput * turnSpeed * Time.deltaTime;

            // Đảo ngược hướng rẽ khi xe lùi
            if (currentSpeed < 0)
            {
                turnAmount = -turnAmount;
            }

            Quaternion turnOffset = Quaternion.Euler(0, turnAmount, 0);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
    }

    // Xoay bánh trước theo đầu vào rẽ
    private void SteerWheels(float turnInput)
    {
        currentSteeringAngle = Mathf.Clamp(turnInput * maxSteeringAngle, -maxSteeringAngle, maxSteeringAngle); // Giới hạn góc xoay
        frontLeftWheel.localRotation = Quaternion.Euler(0, currentSteeringAngle, 0);
        frontRightWheel.localRotation = Quaternion.Euler(0, currentSteeringAngle, 0);
    }

    // Quay bánh xe theo hướng di chuyển
    private void RotateWheels(float moveInput)
    {
        // Sử dụng tốc độ hiện tại để tính toán độ quay của bánh xe
        float wheelRotation = currentSpeed * wheelRotationSpeed * Time.deltaTime;

        // Bánh trước sẽ quay theo hướng di chuyển
        frontLeftWheel.Rotate(Vector3.right, wheelRotation);
        frontRightWheel.Rotate(Vector3.right, wheelRotation);

        // Bánh sau cũng quay theo hướng di chuyển
        rearLeftWheel.Rotate(Vector3.right, wheelRotation);
        rearRightWheel.Rotate(Vector3.right, wheelRotation);
    }

    private IEnumerator FadeLights(Light light, bool turnOn)
    {
        float targetIntensity = turnOn ? 1 : 0;
        while (Mathf.Abs(light.intensity - targetIntensity) > 0.01f)
        {
            light.intensity = Mathf.Lerp(light.intensity, targetIntensity, fadeSpeed * Time.deltaTime);
            yield return null;
        }
        light.enabled = turnOn;
    }

    private void ToggleFrontLights()
    {
        isFrontLightsOn = !isFrontLightsOn;
        StartCoroutine(FadeLights(frontLights, isFrontLightsOn));
    }

    // Hàm để kích hoạt camera đã chọn
    private void ActivateCamera(Camera cam)
    {
        cameraFront.enabled = false;
        cameraBack.enabled = false;
        cameraInterior.enabled = false;

        cam.enabled = true; // Bật camera được chọn
        currentCamera = cam; // Cập nhật camera hiện tại
    }
}
