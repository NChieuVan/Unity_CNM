using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Controller : MonoBehaviour
{
    internal enum driveType
    {
        frontWheelDrive,
        reaWheelDrive,
        allWheelDrive
    }
    [SerializeField] private driveType drive;

    [Header("biến số")]
    public AnimationCurve speedCurve;

    private inputManeger IM;
    private Rigidbody rigidbody;
    public AnimationCurve enginePower;
    public float[] gears;
    private WheelFrictionCurve forwardFriction, sidewaysFriction;
    public float handBrakeFrictionMultiplier = 2f;
    public float handBrakeFriction;
    [HideInInspector] public float engineRPM;
    public float maxRPM, minRPM;
    public float thrust = 1000;

    public WheelCollider[] wheels = new WheelCollider[4];
    public Transform[] wheelMesh = new Transform[4];
    public GameObject centerOfMass;
    private float radius = 4, breakPower = 1000000000f, DownForceValue = 10f, wheelsRPM, driftFactor, lastValue, horizontal, vertical, totalPower;
    private bool flag = false;
    public float KPH;
    public int motorTorque = 1500;
    public float steeringMax = 30;
    private float smoothTime = 0.09f;
    public float[] slip = new float[4];

    [Header("Hiệu ứng")]
    public TrailRenderer[] tireTrails; // Mảng Trail Renderer cho bánh xe
    public ParticleSystem[] tireSmokeEffects; // Mảng Particle System cho khói

    public Transform[] validCarParts;
    public Transform steeringWheel; // Biến tham chiếu đến vô lăng
    public float steeringWheelRotationMultiplier = 5f; // Hệ số nhân để vô lăng quay
    private AudioSource engineSound; // Nguồn phát âm thanh động cơ
    private AudioSource brakeSound;
    private AudioSource collisionSound;
    public AudioClip engineClip;    // Tệp âm thanh động cơ
    public AudioClip brakeAudioClip;    // Nguồn âm thanh phanh
    public AudioClip collisionAudioClip; // Nguồn âm thanh va chạm
    // Start is called before the first frame update
    void Start()
    {
        // Tắt tất cả Trail Renderers khi khởi động
        foreach (TrailRenderer trail in tireTrails)
        {
            trail.emitting = false; // Vô hiệu hóa hiệu ứng trail
        }
        getObject();
        InitializeAudio();
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        addDownForce();
        animateWheels();
        //moveVehicle();
        steerVehicle();
        //getFriction();
        calculateEnginePower();
       // adjustTraction();
        checkHandbrake();
        addDynamicDownForce();
        applyCounterForce();
        resetFriction();
        checkWheelSpin();
        //UpdateSteeringWheel();
    }
    private void InitializeAudio()
    {
        // Khởi tạo các AudioSource
        engineSound = gameObject.AddComponent<AudioSource>();
        brakeSound = gameObject.AddComponent<AudioSource>();
        collisionSound = gameObject.AddComponent<AudioSource>();

        // Gán các tệp âm thanh
        engineSound.clip = engineClip;
        engineSound.loop = true;
        engineSound.playOnAwake = false;
        engineSound.volume = 0.5f; // Mặc định âm lượng động cơ

        brakeSound.clip = brakeAudioClip;
        brakeSound.loop = true;
        brakeSound.playOnAwake = false;

        collisionSound.clip = collisionAudioClip;
        collisionSound.loop = false;
        collisionSound.playOnAwake = false;

        engineSound.Play(); // Phát âm thanh động cơ khi bắt đầu
    }
    
    private void steerVehicle()
    {
        if (Mathf.Abs(IM.horizontal) > 0.1f && Mathf.Abs(KPH) > 0.1f)
        {
            // Giới hạn góc lái tối đa dựa trên tốc độ
            float dynamicSteeringMax = Mathf.Lerp(steeringMax, steeringMax / 5, KPH / 100);
            float steeringAngle = dynamicSteeringMax * IM.horizontal;

            // Đảo hướng rẽ nếu xe đang lùi
            if (KPH < 0)
            {
                steeringAngle = -steeringAngle;
            }

            // Áp dụng góc lái cho bánh trước
            wheels[0].steerAngle = steeringAngle;
            wheels[1].steerAngle = steeringAngle;

            // Cập nhật hướng quay của xe
        }
        else
        {
            // Reset góc lái nếu không có input
            wheels[0].steerAngle = 0f;
            wheels[1].steerAngle = 0f;
        }
    }
   /*
    void UpdateSteeringWheel()
    {
        float horizontalInput = IM.horizontal/2;
        // Định nghĩa các giá trị góc quay và vị trí cho các mức độ quay
        Vector3[] steeringPositions = new Vector3[]
        {
        new Vector3(0, 0, 0),                    // Vị trí ban đầu
        new Vector3(0.3827674f, 0.2694766f, 0),  // Vị trí khi rẽ trái 1
        new Vector3(-0.301883f, -0.06394559f, 0), // Vị trí khi rẽ phải 1
        new Vector3(0.6148403f, 0.8203001f, 0),  // Vị trí khi rẽ trái 2
        new Vector3(-0.6968892f, -0.0002602339f, 0) // Vị trí khi rẽ phải 2
        };

        float[] steeringAngles = new float[]
        {
        0,               // Góc quay ban đầu
        28.012f,         // Góc quay khi rẽ trái 1
        -18.361f,        // Góc quay khi rẽ phải 1
        64.014f,         // Góc quay khi rẽ trái 2
        -42.237f         // Góc quay khi rẽ phải 2
        };

        // Tính toán góc quay và vị trí mới dựa trên input
        int positionIndex = Mathf.RoundToInt(horizontalInput * 2);  // Tính toán chỉ số của vị trí (từ -2 đến 2)
        positionIndex = Mathf.Clamp(positionIndex, -2, 2); // Giới hạn chỉ số để tránh vượt quá phạm vi

        // Cập nhật vị trí và góc quay cho vô lăng
        steeringWheel.localPosition = steeringPositions[positionIndex + 2];  // Dịch chuyển theo vị trí tương ứng
        steeringWheel.localRotation = Quaternion.Euler(0, 0, steeringAngles[positionIndex + 2]);  // Cập nhật góc quay
    }
   */

    /*
    private void steerVehicle()
    {
        if (Mathf.Abs(IM.horizontal) > 0.1f && Mathf.Abs(KPH) > 0.1f)
        {
            float steeringAngle = steeringMax * IM.horizontal * Time.deltaTime;
            if (KPH < 0)
            {
                steeringAngle = -steeringAngle;
            }
            if (IM.horizontal >0) {

                wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
                wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            }
            else if (IM.horizontal < 0 ) {                                                          
            wheels[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * horizontal;
            wheels[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontal;
			//transform.Rotate(Vector3.up * steerHelping);

            } else {
                wheels[0].steerAngle =0;
                wheels[1].steerAngle =0;
            }

            // Đảo ngược hướng rẽ khi xe lùi

            //Quaternion turnOffset = Quaternion.Euler(0, steeringAngle, 0);
            //rigidbody.MoveRotation(rigidbody.rotation * turnOffset);
        }

    }
    */
    private void applyCounterForce()
    {
        // Lực đối kháng ngang để giảm trượt
        Vector3 lateralVelocity = Vector3.Dot(rigidbody.velocity, transform.right) * transform.right;
        rigidbody.AddForce(-lateralVelocity * 3f); // Hệ số điều chỉnh (3f) có thể thay đổi theo nhu cầu
    }

    private void addDynamicDownForce()
    {
        float dynamicDownForce = DownForceValue * (1 + KPH / 50f);
        rigidbody.AddForce(-transform.up * dynamicDownForce);
    }
    /*
    private void checkHandbrake()
    {
        if (!IM.handbrake)
        {
            for (int i = 2; i < 4; i++)
                wheels[i].brakeTorque = 0f;

            if (brakeSound.isPlaying)
                brakeSound.Stop();
        }
        else if (Mathf.Abs(KPH) > 0.1f)
        {
            for (int i = 2; i < 4; i++)
                wheels[i].brakeTorque = breakPower;

            if (!brakeSound.isPlaying)
            {
                brakeSound.volume = 0.5f;
                brakeSound.Play();
            }
        }
        else
        {
            if (brakeSound.isPlaying)
                brakeSound.Stop();
        }
    }
    */
    private void checkHandbrake()
    {
        if (!IM.handbrake)
        {
            for (int i = 2; i < 4; i++)
            {
                wheels[i].brakeTorque = 0f;

                // Tắt hiệu ứng khói và vệt bánh xe khi không phanh
                if (tireSmokeEffects[i - 2] != null)
                {
                    tireSmokeEffects[i - 2].Stop();
                }
                if (tireTrails[i - 2] != null)
                {
                    tireTrails[i - 2].emitting = false;
                }
            }

            if (brakeSound.isPlaying)
                brakeSound.Stop();
        }
        else if (Mathf.Abs(KPH) > 0.1f)
        {
            for (int i = 2; i < 4; i++)
            {
                wheels[i].brakeTorque = breakPower;

                // Bật hiệu ứng khói và vệt bánh xe khi phanh
                if (tireSmokeEffects[i - 2] != null)
                {
                    tireSmokeEffects[i - 2].Play();
                }
                if (tireTrails[i - 2] != null)
                {
                    tireTrails[i - 2].emitting = true;
                }
            }

            if (!brakeSound.isPlaying)
            {
                brakeSound.volume = 0.5f;
                brakeSound.Play();
            }
        }
        else
        {
            for (int i = 2; i < 4; i++)
            {
                // Tắt hiệu ứng khi xe dừng hẳn
                if (tireSmokeEffects[i - 2] != null)
                {
                    tireSmokeEffects[i - 2].Stop();
                }
                if (tireTrails[i - 2] != null)
                {
                    tireTrails[i - 2].emitting = false;
                }
            }

            if (brakeSound.isPlaying)
                brakeSound.Stop();
        }
    }
    private void resetFriction()
    {
        if (IM.horizontal == 0) // Khi không nhấn phím rẽ
        {
            for (int i = 0; i < wheels.Length; i++)
            {

                sidewaysFriction = wheels[i].sidewaysFriction;
                sidewaysFriction.extremumValue = 1f;
                sidewaysFriction.asymptoteValue = 1f;
                wheels[i].sidewaysFriction = sidewaysFriction;
            }
        }
    }
    private void calculateEnginePower()
    {
        wheelRPM();
        float velocity = 0.0f;

        // Tính toán vòng tua máy (engineRPM)

        engineRPM = Mathf.SmoothDamp(engineRPM, maxRPM * Mathf.Clamp01(vertical), ref velocity, smoothTime);

        // Công suất động cơ
        totalPower = enginePower.Evaluate(engineRPM) * vertical;

        // Điều chỉnh âm thanh động cơ
        UpdateEngineSound();

        moveVehicle(); // Di chuyển xe
    }
    private void UpdateEngineSound()
    {
        if (engineSound != null)
        {
            engineSound.volume = Mathf.Clamp(KPH / 200f, 0.1f, 1f); // Âm lượng động cơ dựa trên tốc độ
            engineSound.pitch = Mathf.Lerp(0.8f, 2.0f, engineRPM / maxRPM); // Độ cao động cơ dựa trên vòng tua

            if (!engineSound.isPlaying && KPH > 0.1f)
                engineSound.Play();
            else if (KPH <= 0.1f)
                engineSound.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Kiểm tra nếu va chạm xảy ra với bộ phận hợp lệ của xe
        if (IsCarPart(collision.transform))
        {
            // Kiểm tra va chạm có đủ mạnh không
            if (collision.relativeVelocity.magnitude > 10f)
            {
                
                    collisionSound.Play();
                
            }
        }
    }

    bool IsCarPart(Transform collidedTransform)
    {
        // Kiểm tra xem đối tượng va chạm có phải là một bộ phận hợp lệ của xe không
        foreach (var part in validCarParts)
        {
            // Kiểm tra nếu Transform của đối tượng va chạm là một trong các bộ phận hợp lệ
            if (collidedTransform == part)
            {
                return true;  // Đây là bộ phận hợp lệ của xe
            }
        }
        return false;  // Không phải bộ phận hợp lệ của xe
    }
    private void wheelRPM()
    {
        float sum = 0;
        int R = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += wheels[i].rpm;
            R++;
        }
        wheelsRPM = (R != 0) ? sum / R : 0;


    }


    private void moveVehicle()
    {

        if (drive == driveType.allWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = IM.vertical * (motorTorque / 4);
            }
        }
        else if (drive == driveType.reaWheelDrive)
        {
            for (int i = 0; i < wheels.Length; i++)
            {
                wheels[i].motorTorque = IM.vertical * (motorTorque / 2);
            }
        }
        else
        {
            for (int i = 0; i < wheels.Length - 2; i++)
            {
                wheels[i].motorTorque = IM.vertical * (motorTorque / 2);
            }

        }
        KPH = rigidbody.velocity.magnitude * 3.6f;
       // rigidbody.MovePosition(transform.position + transform.forward * KPH/2 * Time.deltaTime);
        if (IM.handbrake)
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = breakPower * 2;
        }
        else
        {
            wheels[3].brakeTorque = wheels[2].brakeTorque = 0;

        }
        if (IM.boosting)
        {
            rigidbody.AddForce(Vector3.forward * thrust);
        }

    }



    private void animateWheels()
    {
        Vector3 wheelPosition = Vector3.zero;
        Quaternion wheelRotation = Quaternion.identity;

        for (int i = 0; i < 4; i++)
        {
            wheels[i].GetWorldPose(out wheelPosition, out wheelRotation);

            // Cập nhật vị trí bánh xe
            wheelMesh[i].transform.position = wheelPosition;

            // Nếu là bánh trước (0 và 1), thêm góc xoay steerAngle
            if (i < 2)
            {
                Quaternion steerRotation = Quaternion.Euler(0, wheels[i].steerAngle, 0);
                wheelMesh[i].transform.rotation = steerRotation * wheelRotation;
            }
            else
            {
                wheelMesh[i].transform.rotation = wheelRotation;
            }
        }
    }
    private void addDownForce()
    {
        rigidbody.AddForce(-transform.up * DownForceValue * rigidbody.velocity.magnitude);
    }
    private void getObject()
    {
        IM = GetComponent<inputManeger>();
        rigidbody = GetComponent<Rigidbody>();
        centerOfMass = GameObject.Find("mass");
        rigidbody.centerOfMass = centerOfMass.transform.localPosition;
    }
    
    private void adjustTraction()
    {
        if (IM.horizontal != 0 && KPH > 50)
        {
            if (!IM.handbrake)
            {

                forwardFriction = wheels[0].forwardFriction;
                sidewaysFriction = wheels[0].sidewaysFriction;

                forwardFriction.extremumValue = forwardFriction.asymptoteValue = ((KPH * handBrakeFrictionMultiplier) / 300) + 1;
                sidewaysFriction.extremumValue = sidewaysFriction.asymptoteValue = ((KPH * handBrakeFrictionMultiplier) / 300) + 1;

                for (int i = 0; i < 4; i++)
                {
                    wheels[i].forwardFriction = forwardFriction;
                    wheels[i].sidewaysFriction = sidewaysFriction;
                }
            }
            if (IM.handbrake)
            {
                sidewaysFriction = wheels[0].sidewaysFriction;
                forwardFriction = wheels[0].forwardFriction;

                float velocity = 0;
                sidewaysFriction.extremumValue = Mathf.SmoothDamp(sidewaysFriction.extremumValue, handBrakeFriction, ref velocity, 0.05f * Time.deltaTime);
                forwardFriction.extremumValue = Mathf.SmoothDamp(forwardFriction.extremumValue, handBrakeFriction, ref velocity, 0.05f * Time.deltaTime);

                // Giảm mạnh ma sát ngang bánh sau
                for (int i = 2; i < 4; i++)
                {
                    sidewaysFriction = wheels[i].sidewaysFriction;
                    forwardFriction = wheels[i].forwardFriction;

                    sidewaysFriction.extremumValue = handBrakeFriction * 0.2f;
                    forwardFriction.extremumValue = handBrakeFriction * 0.5f;

                    wheels[i].sidewaysFriction = sidewaysFriction;
                    wheels[i].forwardFriction = forwardFriction;
                }

                // Giữ ma sát ngang ổn định ở bánh trước
                for (int i = 0; i < 2; i++)
                {
                    sidewaysFriction = wheels[i].sidewaysFriction;
                    forwardFriction = wheels[i].forwardFriction;

                    sidewaysFriction.extremumValue = Mathf.Max(sidewaysFriction.extremumValue, 1f);
                    forwardFriction.extremumValue = Mathf.Max(forwardFriction.extremumValue, 1f);

                    wheels[i].sidewaysFriction = sidewaysFriction;
                    wheels[i].forwardFriction = forwardFriction;
                }
            }
        }


    }
    



    private float tempo;
    
    void checkWheelSpin()
    {
        float slipThreshold = 0.8f; // Ngưỡng trượt ngang
        for (int i = 0; i < wheels.Length; i++)
        {
            WheelHit wheelHit;
            wheels[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.sidewaysSlip) > slipThreshold)
            {
                sidewaysFriction = wheels[i].sidewaysFriction;

                // Giảm ma sát ngang khi slip vượt ngưỡng
                sidewaysFriction.extremumValue = Mathf.Lerp(sidewaysFriction.extremumValue, 1f, Time.deltaTime * 5f);
                wheels[i].sidewaysFriction = sidewaysFriction;
            }
        }
    }

}