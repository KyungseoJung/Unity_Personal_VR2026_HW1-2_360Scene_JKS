using UnityEngine;
using System.Collections;

public class FaceAnimation : MonoBehaviour
{
    [Header("Face Parts")]
    public Transform mouth;
    public Transform rightEye;
    public Transform leftEye;
    public Transform rightBrow;
    public Transform leftBrow;

    // 초기값 저장
    private Vector3 mouthOrigScale;
    private Quaternion mouthOrigRot;   // 추가 설정
    private Vector3 rightEyeOrigScale;
    private Quaternion rightEyeOrigRot; // 추가 설정
    private Vector3 leftEyeOrigScale;
    private Quaternion leftEyeOrigRot;  // 추가 설정
    private Vector3 rightBrowOrigPos;
    private Vector3 leftBrowOrigPos;
    private Quaternion rightBrowOrigRot;
    private Quaternion leftBrowOrigRot;

    private float blinkTimer    = 0f;
    private float blinkInterval = 3f;
    private bool  isSpeaking    = false;

    void Start()
    {
        if (mouth)     {mouthOrigScale     = mouth.localScale;}
        if (mouth)     {mouthOrigRot     = mouth.localRotation;}    // 추가 설정

        if (rightEye)  
        {
            rightEyeOrigScale  = rightEye.localScale;
            rightEyeOrigRot = rightEye.localRotation;
        }
        if (leftEye)   
        {
            leftEyeOrigScale   = leftEye.localScale;
            leftEyeOrigRot = leftEye.localRotation;
        }

        if (rightBrow) { rightBrowOrigPos = rightBrow.localPosition; rightBrowOrigRot = rightBrow.localRotation; }
        if (leftBrow)  { leftBrowOrigPos  = leftBrow.localPosition;  leftBrowOrigRot  = leftBrow.localRotation; }
    }

    void Update()
    {
        // 자동 눈깜빡임
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval) { StartCoroutine(Blink()); blinkTimer = 0f; }

        // 표정 키 입력
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetExpression("joy");     // 1키
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetExpression("sadness"); // 2키
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetExpression("anger");   // 3키
        // if (Input.GetKeyDown(KeyCode.F)) SetExpression("frown");    // 기존 frown 유지
        if (Input.GetKeyDown(KeyCode.M) && !isSpeaking) StartCoroutine(Speaking());
        if (Input.GetKeyDown(KeyCode.R)) SetExpression("reset");
    }

    void SetExpression(string type)
    {
        // 표정 바꾸기 전에 항상 초기화 먼저
        ResetFace();

        if (type == "joy")
        {
            // 입: 가로로 넓히되 세로도 약간 있어야 웃음처럼 보임
            if (mouth)
            {
                // mouth.localScale = new Vector3(
                //     // mouthOrigScale.x * 1.6f,
                //     // mouthOrigScale.y * 0.85f,  // 너무 납작하지 않게
                //     // mouthOrigScale.z

                //     // 추가 수정
                //     mouthOrigScale.x * 1.35f,
                //     mouthOrigScale.y * 0.45f,
                //     mouthOrigScale.z * 0.8f
                //     );
                // mouth.localPosition = mouthOrigRot + new Vector3(0f, 0f, 0.002f);  // 추가 수정
                mouth.localRotation = Quaternion.Euler(60f, 0, 0);   // 추가 수정
            }

            // 눈: 약간 세로로 찌그러뜨리기 (눈 가늘게 = 웃는 눈)
            if (rightEye)
            {
                rightEye.localScale = new Vector3(
                    rightEyeOrigScale.x, 
                    rightEyeOrigScale.y,  
                    rightEyeOrigScale.z * 0.3f // 가로로 늘리기 (실눈처럼)
                    );

                rightEye.localRotation = Quaternion.Euler(0, -15f, 0);  // 추가 수정 (순하게 웃는 느낌)
            }

            if (leftEye)
            {
                leftEye.localScale = new Vector3(
                    leftEyeOrigScale.x,
                    leftEyeOrigScale.y,
                    leftEyeOrigScale.z* 0.3f // 가로로 늘리기 (실눈처럼)
                    );
                leftEye.localRotation = Quaternion.Euler(0, 15f, 0);    // 추가 수정 (순하게 웃는 느낌)
            }

            // 눈썹: 위로 올리기 + 약간 바깥쪽으로 기울이기
            if (rightBrow)
            {
                rightBrow.localPosition = rightBrowOrigPos + new Vector3(0, 0, 0.002f);
                // rightBrow.localRotation = Quaternion.Euler(0, 12f, 0);
            }
            if (leftBrow)
            {
                leftBrow.localPosition = leftBrowOrigPos + new Vector3(0, 0, 0.002f);
                // leftBrow.localRotation = Quaternion.Euler(0, -12f, 0);
            }
        }
        else if (type == "sadness")
        {
            // 입: 가로로 약간 줄이고 — 슬픈 입은 아래로 처지는 느낌
            if (mouth)
                mouth.localScale = new Vector3(
                    mouthOrigScale.x * 0.75f,
                    mouthOrigScale.y * 0.7f,
                    mouthOrigScale.z);

            // 눈: 세로로 약간 늘리기 (축 처진 눈)
            if (rightEye)
                rightEye.localScale = new Vector3(
                    rightEyeOrigScale.x,
                    rightEyeOrigScale.y * 1.2f,
                    rightEyeOrigScale.z);
            if (leftEye)
                leftEye.localScale = new Vector3(
                    leftEyeOrigScale.x,
                    leftEyeOrigScale.y * 1.2f,
                    leftEyeOrigScale.z);

            // 눈썹: 안쪽이 올라가고 바깥쪽이 내려가는 기울기 (슬픈 눈썹)
            if (rightBrow)
                rightBrow.localRotation = Quaternion.Euler(0, -20f, 0); // 안쪽↑
            if (leftBrow)
                leftBrow.localRotation  = Quaternion.Euler(0,  20f, 0);
        }
        else if (type == "anger")
        {
            // 입: 꽉 다문 느낌 (세로로 매우 납작)
            if (mouth)
                mouth.localScale = new Vector3(
                    mouthOrigScale.x * 0.8f,
                    mouthOrigScale.y * 0.3f,
                    mouthOrigScale.z);

            // 눈: 세로로 많이 줄이기 (눈 부릅뜨기)
            if (rightEye)
                rightEye.localScale = new Vector3(
                    rightEyeOrigScale.x * 0.4f,
                    rightEyeOrigScale.y ,
                    rightEyeOrigScale.z);
            if (leftEye)
                leftEye.localScale = new Vector3(
                    leftEyeOrigScale.x * 0.4f,
                    leftEyeOrigScale.y,
                    leftEyeOrigScale.z);

            // 눈썹: 안쪽이 내려오고 바깥쪽이 올라가는 기울기 (화난 눈썹)
            // + 눈썹 전체를 아래로 내리기
            if (rightBrow)
            {
                rightBrow.localPosition = rightBrowOrigPos + new Vector3(0, 0, -0.002f);
                rightBrow.localRotation = Quaternion.Euler(0, 25f, 0); // 안쪽↓
            }
            if (leftBrow)
            {
                leftBrow.localPosition = leftBrowOrigPos + new Vector3(0, 0, -0.002f);
                leftBrow.localRotation = Quaternion.Euler(0, -25f, 0);
            }
        }
        else if (type == "frown")
        {
            // 기존 frown 유지
            if (mouth)
                mouth.localScale = new Vector3(
                    mouthOrigScale.x * 0.6f,
                    mouthOrigScale.y * 0.6f,
                    mouthOrigScale.z);
            if (rightBrow) rightBrow.localRotation = Quaternion.Euler(0, -20f, 0);
            if (leftBrow)  leftBrow.localRotation  = Quaternion.Euler(0,  20f, 0);
        }
        else if (type == "reset")
        {
            // ResetFace()가 이미 위에서 호출됨, 추가 작업 없음
        }
    }

    void ResetFace()
    {
        if (mouth)     
        {
            mouth.localScale = mouthOrigScale;   
            mouth.localRotation = mouthOrigRot; // 추가 설정 (입 원상태로)
        }
        if (rightEye)  
        {
            rightEye.localScale     = rightEyeOrigScale;
            rightEye.localRotation = rightEyeOrigRot;
        }
        if (leftEye)   
        {
            leftEye.localScale      = leftEyeOrigScale;
            leftEye.localRotation = leftEyeOrigRot;
        }

        if (rightBrow) { rightBrow.localPosition = rightBrowOrigPos; rightBrow.localRotation = rightBrowOrigRot; }
        if (leftBrow)  { leftBrow.localPosition  = leftBrowOrigPos;  leftBrow.localRotation  = leftBrowOrigRot; }
    }

    IEnumerator Blink()
    {
        Vector3 origRight = rightEye ? rightEye.localScale : Vector3.one;
        Vector3 origLeft  = leftEye  ? leftEye.localScale  : Vector3.one;
        if (rightEye) rightEye.localScale = new Vector3(origRight.x, origRight.y, 0.05f);
        if (leftEye)  leftEye.localScale  = new Vector3(origLeft.x,  origLeft.y,  0.05f);
        yield return new WaitForSeconds(0.12f);
        if (rightEye) rightEye.localScale = origRight;
        if (leftEye)  leftEye.localScale  = origLeft;
    }

    IEnumerator Speaking()
    {
        isSpeaking = true;
        Vector3 openScale = new Vector3(mouthOrigScale.x, mouthOrigScale.y * 2.2f, mouthOrigScale.z);
        for (int i = 0; i < 5; i++)
        {
            if (mouth) mouth.localScale = openScale;
            yield return new WaitForSeconds(0.13f);
            if (mouth) mouth.localScale = mouthOrigScale;
            yield return new WaitForSeconds(0.13f);
        }
        isSpeaking = false;
    }
}