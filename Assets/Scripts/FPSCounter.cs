using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;

    private void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        // FPS hesaplama
        int fps = Mathf.CeilToInt(1.0f / deltaTime);
        string fpsText = $"{fps} FPS";

        // Yazı stili
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        // Arkaplan kutusu
        Color originalColor = GUI.color;
        GUI.color = new Color(0.01f, 0.01f, 0.01f, 0.99f);
        Rect rect = new Rect(10, 10, 250, 80);
        GUI.Box(rect, "");
        GUI.color = Color.white;

        // Performans verileri
        string statsText = fpsText;

#if UNITY_EDITOR
        // UnityStats sadece editörde kullanılabilir
        int setPassCalls = UnityEditor.UnityStats.setPassCalls;
        int batches = UnityEditor.UnityStats.batches;
        statsText += $"\nSetPass Calls: {setPassCalls}\nBatches: {batches}";
#endif

        // Yazıları ekrana çiz
        GUI.Label(new Rect(20, 20, 230, 60), statsText, style);
    }
}
