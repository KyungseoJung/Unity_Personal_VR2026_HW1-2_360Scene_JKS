using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
    [Header("Joints")]
    public Transform j1_Root;
    public Transform j3_RightShoulder;
    public Transform j4_LeftShoulder;
    public Transform j5_RightHip;
    public Transform j6_LeftHip;

    private bool isWalking = false;
    private bool isBowing  = false;
    private bool isWaving  = false;

    private float walkTime = 0f;
    private float bowAngle = 0f;
    private float waveTime = 0f;

    void Update()
    {
        // 키 입력
        if (Input.GetKeyDown(KeyCode.Q)) isWalking = !isWalking;
        if (Input.GetKeyDown(KeyCode.B)) isBowing  = !isBowing;
        if (Input.GetKeyDown(KeyCode.H)) isWaving  = !isWaving;

        HandleWalk();
        HandleBow();
        HandleWave();
    }

    void HandleWalk()
    {
        if (!isWalking)
        {
            // 걷기 멈추면 원위치
            walkTime = 0f;
            if (j5_RightHip) j5_RightHip.localRotation = Quaternion.identity;
            if (j6_LeftHip)  j6_LeftHip.localRotation  = Quaternion.identity;
            if (j3_RightShoulder) j3_RightShoulder.localRotation = Quaternion.identity;
            if (j4_LeftShoulder)  j4_LeftShoulder.localRotation  = Quaternion.identity;
            return;
        }

        walkTime += Time.deltaTime * 3f;
        float legAngle  = Mathf.Sin(walkTime) * 30f;  // 다리: -30 ~ +30도
        float armAngle  = Mathf.Sin(walkTime) * 20f;  // 팔: 반대로

        // 다리: 교대로 앞뒤
        if (j5_RightHip) j5_RightHip.localRotation = Quaternion.Euler( legAngle, 0, 0);
        if (j6_LeftHip)  j6_LeftHip.localRotation  = Quaternion.Euler(-legAngle, 0, 0);

        // 팔: 다리 반대로
        if (j3_RightShoulder) j3_RightShoulder.localRotation = Quaternion.Euler(-armAngle, 0, 0);
        if (j4_LeftShoulder)  j4_LeftShoulder.localRotation  = Quaternion.Euler( armAngle, 0, 0);
    }

    void HandleBow()
    {
        // 인사: J1_Root를 앞으로 숙임
        float targetAngle = isBowing ? 45f : 0f;
        bowAngle = Mathf.Lerp(bowAngle, targetAngle, Time.deltaTime * 4f);
        if (j1_Root) j1_Root.localRotation = Quaternion.Euler(bowAngle, 0, 0);
    }

    void HandleWave()
    {
        if (!isWaving)
        {
            waveTime = 0f;
            if (j3_RightShoulder) j3_RightShoulder.localRotation = Quaternion.identity;
            return;
        }
        waveTime += Time.deltaTime * 4f;
        float angle = Mathf.Sin(waveTime) * 35f;
        // 오른팔을 Z축으로 들고 왕복
        // 수정 코드
        if (j3_RightShoulder)
            j3_RightShoulder.localRotation = Quaternion.Euler(0, 60f + angle, 0);   // 80f -> 60f로 변경
    }
}