using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[CustomEditor(typeof(AnimationRenderer))]
public class NalydInspector : Editor
{
    public float TotalFrames;
    public string Path;
    AnimationRenderer animationRenderer;
    GUIStyle renderButtonStyle;
    //GUIStyle LinkButtonStyle;
    Texture2D[] icons = new Texture2D[3];
    
    //bool Rendering;
    private void OnEnable()
    {
        animationRenderer = target as AnimationRenderer;
        Path = PlayerPrefs.GetString(SceneManager.GetActiveScene().name);
        renderButtonStyle = SetRButtonStyle();
        //LinkButtonStyle = SetLinkButtonStyle();
        icons[0] = Resources.Load("Play", typeof(Texture2D)) as Texture2D;
        icons[1] = Resources.Load("Record", typeof(Texture2D)) as Texture2D;
        icons[2] = Resources.Load("Offline", typeof(Texture2D)) as Texture2D;
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
        TotalFrames = (animationRenderer.End_Frame - animationRenderer.Start_Frame) + 1;
        ProgressBar(animationRenderer.SavedFrameCount / TotalFrames, "現在算到第" + animationRenderer.SavedFrameCount.ToString() + "張，總共 " + TotalFrames + " 張");
        //EditorGUILayout.LabelField("輸出資料夾：", path);
        if (GUILayout.Button("前往輸出資料夾, 目前路徑：" + Path)) {
            string to = Path.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", to);
        }
        RenderButton();
    }

    private void ProgressBar(float v, string Label) {
        Rect r = GUILayoutUtility.GetRect(10, 20);
        EditorGUI.ProgressBar(r, v, Label);
    }

    private void RenderButton() {
        if (!animationRenderer.Rendering)
        {
            if (animationRenderer.Ani_Clip != null)
            {
                if (animationRenderer.Capture_FPS != 0)
                {
                    if (animationRenderer.EventCapture)
                    {
                        if (GUILayout.Button(icons[0], renderButtonStyle))
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
            GUILayout.Button(icons[1], renderButtonStyle);
            return;
        }
        GUILayout.Button(icons[2], renderButtonStyle);
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
