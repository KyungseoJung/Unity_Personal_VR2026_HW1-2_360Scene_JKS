using UnityEngine;

public class DebugTextDating : MonoBehaviour
{
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);

        style.fontSize = 10;
        style.normal.textColor = Color.black;

        GUILayout.BeginArea(new Rect(20, 20, 700, 500));

        GUILayout.Label("Blind Date Scene Controls", style);
        GUILayout.Space(15);

        GUILayout.Label("Key 1 : Nice to meet you + Bow", style);
        GUILayout.Label("Key 2 : Hello + Wave hand", style);
        GUILayout.Label("Key 3 : Good bye + Wave hand", style);
        GUILayout.Label("Key 4 : Good Joke -> Boy Laugh", style);
        GUILayout.Label("Key 5 : Bad Joke -> Boy Frown", style);
        GUILayout.Label("Avatar A: User-controlled", style);
        GUILayout.Label("Avatar B: LLM-controlled", style);


        GUILayout.EndArea();
    }
}