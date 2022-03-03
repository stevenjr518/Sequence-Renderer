using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[CustomEditor(typeof(AnimationRenderer))]
public class NalydInspector : Editor
{
    public float TotalFrames;
    public string path;
    AnimationRenderer AR;
    GUIStyle RenderButtonStyle;
    GUIStyle LinkButtonStyle;
    Texture2D[] pikas = new Texture2D[3];
    
    //bool Rendering;
    private void OnEnable()
    {
        AR = target as AnimationRenderer;
        path = PlayerPrefs.GetString(SceneManager.GetActiveScene().name);
        RenderButtonStyle = SetRButtonStyle();
        //LinkButtonStyle = SetLinkButtonStyle();
        pikas[0] = Resources.Load("pika", typeof(Texture2D)) as Texture2D;
        pikas[1] = Resources.Load("pikago", typeof(Texture2D)) as Texture2D;
        pikas[2] = Resources.Load("pika_offline", typeof(Texture2D)) as Texture2D;
    }

    private GUIStyle SetRButtonStyle() {
        GUIStyle style = new GUIStyle
        {
            fixedHeight = 300,
            fixedWidth = 300,
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter
        };
        return style;
    }

    private GUIStyle SetLinkButtonStyle() {
        GUIStyle style = new GUIStyle
        {
            fixedHeight = 35,
            fixedWidth = 400,
            fontSize = 20,
            alignment = TextAnchor.MiddleCenter
        };
        return style;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TotalFrames = (AR.End_Frame - AR.Start_Frame) + 1;
        ProgressBar(AR.SavedFrameCount / TotalFrames, "現在算到第" + AR.SavedFrameCount.ToString() + "張，總共 " + TotalFrames + " 張");
        //EditorGUILayout.LabelField("輸出資料夾：", path);
        if (GUILayout.Button("前往輸出資料夾, 目前路徑：" + path)) {
            string to = path.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", to);
        }
        RenderButton();
    }

    private void ProgressBar(float v, string Label) {
        Rect r = GUILayoutUtility.GetRect(10, 20);
        EditorGUI.ProgressBar(r, v, Label);
    }

    private void RenderButton() {
        if (!AR.Rendering)
        {
            if (AR.Ani_Clip != null)
            {
                if (AR.Capture_FPS != 0)
                {
                    if (AR.EventCapture)
                    {
                        if (GUILayout.Button(pikas[0], RenderButtonStyle))
                        {
                            EditorApplication.isPlaying = true;
                        }
                        return;
                    }
                }
            }
        }
        else
        {
            GUILayout.Button(pikas[1], RenderButtonStyle);
            return;
        }
        GUILayout.Button(pikas[2], RenderButtonStyle);
    }
    
}

[CustomPropertyDrawer(typeof(RenameAttribute))]
public class RenameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RenameAttribute rename = (RenameAttribute)attribute;
        label.text = rename.name;
        EditorGUI.PropertyField(position, property, label);
    }
}
