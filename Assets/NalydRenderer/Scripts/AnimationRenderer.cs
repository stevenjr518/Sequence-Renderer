using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[System.Serializable]
public class AnimationRenderer : MonoBehaviour {

    [Tooltip("Name of your pictures")]
    [Rename("檔案名稱")]
    public string FileName = "Animation";//檔名設定
    [Tooltip("Add Capture Events on Animaion")]
    [Rename("啟動算圖")]
    public bool EventCapture = false;
    [Tooltip("Do you want to select the folder of those saving pics?")]
    [Rename("是否設定輸出資料夾?")]
    public bool Folder_Selection = true; //是否指定存圖路徑
    private readonly string _fileType = "jpg";
    string path = "Image_Sequence"; //預設資料夾為Image_Sequence
    AnimationEvent _captureEvent;
    //[Rename("動畫Animator")]
    //public Animator animator;
    [Rename("動畫Animation")]
    public AnimationClip Ani_Clip;
    [Rename("起始Frame")]
    public int Start_Frame;
    [Rename("結尾Frame")]
    public int End_Frame;
    [Rename("每秒張數FPS")]
    public float Capture_FPS;
    [Range(1, 100)]
    public int JPG_Quality = 85;
    [Rename("輸出Frame號")]
    public bool PrintFrameNumber;
    [Rename("Frame號顏色設定")]
    public Color FrameNumColor = new Color(0, 0, 0, 255);
    [HideInInspector]
    public int CurrentFrame;
    [HideInInspector]
    public int SavedFrameCount = 0;
    [HideInInspector]
    public bool Rendering;
    //bool NoCoroutineRunning = true;
    GUIStyle LabelStyle;

    void Awake()
    {
        if (Start_Frame > End_Frame) {
            Debug.Log("開始Frame號不能大於結尾Frame號喔=_=這樣是要Ren個小叮噹喔~");
            enabled = false;
        }
        path = string.IsNullOrEmpty(PlayerPrefs.GetString(SceneManager.GetActiveScene().name)) ? "Image_Sequence" : PlayerPrefs.GetString(SceneManager.GetActiveScene().name);
        SetLabelStyle();
        if (EventCapture) {
            Time.captureFramerate = (int)Capture_FPS;
            if (Folder_Selection)
            {
                path = EditorUtility.SaveFolderPanel("你要放連續圖檔到哪?", path, FileName);
                if (string.IsNullOrEmpty(path))
                {
                    Debug.Log("你沒有設定輸出資料夾=OO=");
                    EditorApplication.isPlaying = false;
                    return;
                }
                else
                {
                    PlayerPrefs.SetString(SceneManager.GetActiveScene().name, path);
                }
            }
            else
            {
                Set_Saving_Folder();
            }

            if (Ani_Clip != null)
            {
                Rendering = true;
                for (int i = Start_Frame; i <= End_Frame; ++i)
                {
                    _captureEvent = new AnimationEvent();
                    _captureEvent.time = i / Capture_FPS;
                    _captureEvent.functionName = "Cap_Text2D";
                    _captureEvent.intParameter = i;
                    Ani_Clip.AddEvent(_captureEvent);
                }
            }
            else {
                Debug.Log("需要拉入動畫Clip!");
            }
        }
    }

    void OnGUI()
    {
        if (PrintFrameNumber) {
            GUI.Label(new Rect(10, 10, 150, 100), CurrentFrame.ToString(), LabelStyle);
        }
    }

    IEnumerator Event_Cap_Text2D(int i) {
        //yield return new WaitUntil(() => NoCoroutineRunning == true);
        CurrentFrame = i;
        //Debug.Log(i);
        //NoCoroutineRunning = false;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGBA32, false);
        yield return new WaitForEndOfFrame();
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();
        byte[] bytes = screenshot.EncodeToJPG(JPG_Quality);
        string localURL = path + "/" + FileName + "_" + i.ToString() + "." + _fileType;
        File.WriteAllBytes(localURL, bytes);
        Destroy(screenshot);
        SavedFrameCount = i;
        Debug.Log(FileName + "_" + i + "." + _fileType + " Saved to " + path);
        
        if (i == End_Frame)
        {
            Debug.Log("Capture Done");
            EditorApplication.isPlaying = false;
        }
        //NoCoroutineRunning = true;
    }

    public void Cap_Text2D(int i) {
        StartCoroutine(Event_Cap_Text2D(i));
    }

    public void Set_Saving_Folder() {
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(SceneManager.GetActiveScene().name)))
        {
            path = "Image_Sequence";
            PlayerPrefs.SetString(SceneManager.GetActiveScene().name, path);
        }
        else {
            path = PlayerPrefs.GetString(SceneManager.GetActiveScene().name);
        }
        
    }

    private void SetLabelStyle() {
        LabelStyle = new GUIStyle();
        LabelStyle.fontSize = 30;
        LabelStyle.normal.textColor = FrameNumColor;
    }

    void OnApplicationQuit()
    {
        Rendering = false;
    }
}
