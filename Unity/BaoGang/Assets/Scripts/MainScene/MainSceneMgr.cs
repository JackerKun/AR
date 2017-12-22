using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HopeRun;
using UnityEngine.Assertions.Must;

public class MainSceneMgr : MonoBehaviour
{
    public static MainSceneMgr MainMgr;
    public static bool LazyQuit = false;
    private InputField ipInputField;

    private TDButtonItem[] btns;
    private Text tiptext;

    bool isIpFieldShow = true;

    //    void OnGUI()
    //    {
    //        GUILayout.Label(SystemInfo.deviceName);
    //        GUILayout.Label(SystemInfo.deviceModel);
    //        GUILayout.Label("Net State: " + Application.internetReachability);
    //    }

    // Use this for initialization
    void Start()
    {
        //屏幕常量
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.runInBackground = true;
        if (MainMgr == null)
        {
            MainMgr = this;
            DontDestroyOnLoad(GameObject.Find("MainCanvas"));
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(GameObject.Find("MainCanvas"));
            Destroy(gameObject);
        }

        Init();
        SceneManager.activeSceneChanged += ((first, second) =>
        {
            //记录上一个场景号
            Debug.LogError(first.name + " --->> " + second.name);
        });
    }

    void Init()
    {
        btns = new TDButtonItem[3];
        btns[0] = GameObject.Find("Canvas/btns/btn_jiayao").GetComponent<TDButtonItem>();
        btns[1] = GameObject.Find("Canvas/btns/btn_xunjian").GetComponent<TDButtonItem>();
        btns[2] = GameObject.Find("Canvas/btns/btn_guandao").GetComponent<TDButtonItem>();
        tiptext = GameObject.Find("Canvas/textbg/name").GetComponent<Text>();
        ipInputField = GameObject.Find("SettingCanvas/IPInputField").GetComponent<InputField>();
        Switch();
        ipInputField.text = GlobalManager.IP;
        //		WebManager.Init ();
        //
        //		WebErrorProccess.Init ();
        //		StartCoroutine (RefreshSocket ());

        btns[0].OnClick += () => { FirstLoadScene("Tank");};
        btns[1].OnClick += () => { FirstLoadScene("Inspection"); };
        btns[0].OnOver += () => { tiptext.text = "加药"; };
        btns[1].OnOver += () => { tiptext.text = "巡检"; };

        btns[0].OnOut += () => { tiptext.text = "请选择场景"; };
        btns[1].OnOut += () => { tiptext.text = "请选择场景"; };
    }


    #region 显示输入框操作
    private bool isready = true;
    public void Switch()
    {
        if (!isready) return;
        isready = false;
        isIpFieldShow = !isIpFieldShow;
        if (isIpFieldShow)
        {
            ipInputField.transform.DOMoveX(ipInputField.transform.position.x - 600, 0.2f).OnComplete(() => { isready = true; });
        }
        else
        {
            ipInputField.transform.DOMoveX(ipInputField.transform.position.x + 600, 0.2f).OnComplete(() => { isready = true; });
        }
    }
    #endregion




    public void FirstLoadScene(string sceneName)
    {
        //释放场景资源
        GlobalManager.IP = ipInputField.text;
        if (sceneName == "Tank")
        {
            GlobalManager.PORTAL = ":1234";
        }
        else if (sceneName == "Inspection")
        {
            GlobalManager.PORTAL = ":1235";
        }
        Debug.Log(GlobalManager.IP + GlobalManager.PORTAL);

        WebManager.Instance.Init(sceneName.ToLower());
        GlobalManager.LoadScene(sceneName);
        //WebErrorProccess.Init();
//        StartCoroutine(RefreshSocket());
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        else if (LazyQuit)
        {
            StartCoroutine(CalLazyQuit());
        }
    }

    IEnumerator CalLazyQuit()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("Quit!");
        LazyQuit = false;
        Application.Quit();
    }
}
