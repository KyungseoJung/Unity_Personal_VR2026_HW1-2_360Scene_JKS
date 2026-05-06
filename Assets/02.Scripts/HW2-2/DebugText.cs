using UnityEngine;

public class DebugText : MonoBehaviour
{
    // [Header("설정")]
    private string debugMessage = "Body animation \nQ: Walk, B: Bow, H: Wave Hand \n \nFacial expression \n1: Laugh, 2: Sad, 3: Angry(Frown), M: Speaking, R: Reset"; // 표시할 내용
    // [Range(10, 200)] 
    private int fontSize = 12;                  // 실시간 조절할 크기
    private Color textColor = Color.black;      // 실시간 조절할 색상

    void OnGUI()
    {
        // 1. 스타일 동적 생성 및 설정
        GUIStyle style = new GUIStyle();
        style.fontSize = fontSize;      // 변수와 연결
        style.normal.textColor = textColor; 
        style.fontStyle = FontStyle.Bold;

        // 2. 화면에 표시 (내용도 변수와 연결)
        // Rect의 너비/높이는 글자 크기에 맞춰 충분히 크게 설정하세요.
        GUI.Label(new Rect(50, 200, 1000, 300), debugMessage, style);
    }
}
