using System.Collections;
using UnityEngine;
using AR.Common;
using AR.Configs;
using HopeRun;
using HopeRun.Model;

public class WebManager : MonoBehaviour
{
    public bool IsConnect = true;

    SocketService _socketService;

    public static WebManager Instance;

    void Awake()
    {
        Instance = this;
        _socketService = new SocketService();
    }

    private SocketService socket
    {
        get
        {
            if (_socketService == null)
            {
                _socketService = new SocketService();
            }
            return _socketService;
        }
    }

    public IRegistServer _registServer { private set; get; }

    public T GetServer<T>()
    {
        return (T)_registServer;
    }

    public void Init(IRegistServer registServer)
    {
        _registServer = registServer;
        IsConnect = false;
        AddCommonEvent();
        _registServer.AddSelfEvent();

        StopAllCoroutines();
        StartCoroutine(CheckNetwork());
    }

    IEnumerator CheckNetwork()
    {
        while (true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                UIManager.ShowErrorMessage("网络未连接");
                UIManager.ChangeScreenEdgeColor(Color.red);
                break;
            }
            //IsConnect = false;
            yield return new WaitForSeconds(2f);
            if (!IsConnect)
            {
                UIManager.ShowErrorMessage("服务断开连接");
                UIManager.ChangeScreenEdgeColor(Color.red);
                break;
            }
        }
        socket.CloseServer();
        Init(_registServer);
    }
    /// <summary>
    /// 注册通用事件
    /// </summary>
    private void AddCommonEvent()
    {

        socket.AddConnectListener(() =>
        {
            IsConnect = true;
            UIManager.ShowMessage("服务连接成功");
            UIManager.ChangeScreenEdgeColor(Color.white);
        });
        socket.AddDisconnectListener(() =>
        {
            IsConnect = false;
        });
        //心跳检测
        //On(EventConfig.RESPONSE_HEART, node =>
        //{
        //    IsConnect = true;
        //});
        #region 旧代码
        //心跳连接
        //        socketService.Register(
        //            EventConfig.REQUEST_HEART,
        //            EventConfig.RESPONSE_HEART,
        //            (socket, packet, args) =>
        //            {
        //                IsConnect = true;
        //            });
        //监听错误信息
        //socketService.AddListener(EventConfig.WARN_MESSAGE,
        //    (socket, packet, args) =>
        //    {
        //        Debug.Log(packet.Payload);
        //        UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
        //    });
        //socketService.AddListener(
        //    EventConfig.AR_DISCONNECT,
        //    (socket, packet, args) =>
        //    {
        //        Debug.Log(packet.Payload);
        //        UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
        //    });
        #endregion
        On(EventConfig.WARN_MESSAGE,
            node =>
            {
                UIManager.ShowErrorMessage(node[1]["message"]);
            });
        On(EventConfig.AR_DISCONNECT,
            node =>
            {
                UIManager.ShowErrorMessage(node[1]["message"]);
            });
    }



    #region 新添加的

    public void Connect(string sceneName, KNodeCallback callback)
    {
        On(EventConfig.AR_ONLINE, callback);

        socket.Open();

        Emit(EventConfig.ONLINE, JsonUtility.ToJson(new LocalDeviceRequest(sceneName)));
        //心跳检测
        //Emit(EventConfig.REQUEST_HEART, JsonUtility.ToJson(new TargetDeviceRequest(true, GlobalManager.DeviceID, "1")));
    }

    public void Disconnect()
    {
        StopAllCoroutines();
        socket.CloseServer();
    }

    /// <summary>
    /// 注册事件，回调函数为jsonnode数组，0是data，1是原始json  {state:"",data:{},message:""}
    /// </summary>
    /// <param name="response">回调名称</param>
    /// <param name="callback">回调方法</param>
    public void On(string response, KNodeCallback callback)
    {
        socket.AddListener(response, callback);
    }

    public void Off(string response)
    {
        socket.RemoveListener(response);
    }

    public void Emit(string request, string data)
    {
        socket.Request(request, data);
    }

    public void StartRequestData(string request, string eventName, KNodeCallback callback)
    {
        socket.StartGetData(request, eventName, callback);
    }

    public void CancleRequestData(string request)
    {
        socket.CancleGetData(request);
    }
    #endregion
}