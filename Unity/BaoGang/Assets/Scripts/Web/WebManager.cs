using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AR.Common;
using AR.Configs;
using SimpleJSON;
using HopeRun.Message;
using HopeRun;

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

    public SocketService socket
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

    public void Init(string sceneName)
    {
        #region 服务器注册
        SocketService socketService = socket;
        IsConnect = false;
        //通用的注册服务
        PublicRegistListener(socketService);
        GlobalManager.CURRENT_SCENE_SERVICE = sceneName;

        socketService.InitScene(sceneName,
            (socket, packet, args) =>
            {
                Debug.LogError("Init Scene.." + packet.Payload);
                //GlobalManager.CURRENT_SCENE_SERVICE = sceneName;
                DealState(packet.Payload, true);
            });

        if (sceneName == "tank")
        {
            TankSocketService.Instance.RegistServices();
        }
        else if (sceneName == "inspection")
        {
            //监听流程
            socketService.AddListener(EventConfig.AR_WORKFLOW,
                (socket, packet, args) =>
                {
                    Debug.Log(packet.Payload);
                    DealState(packet.Payload);
                });
        }

        StopAllCoroutines();
        StartCoroutine(RefreshSocket());

        #endregion
    }


    private void PublicRegistListener(SocketService socketService)
    {
        //心跳连接
        socketService.Register(
            EventConfig.REQUEST_HEART,
            EventConfig.RESPONSE_HEART,
            (socket, packet, args) =>
            {
                Debug.Log("yes!");
                IsConnect = true;
            });
        //监听错误信息
        socketService.AddListener(EventConfig.WARN_MESSAGE,
            (socket, packet, args) =>
            {
                Debug.Log(packet.Payload);
                UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
            });
        socketService.AddListener(
            EventConfig.AR_DISCONNECT,
            (socket, packet, args) =>
            {
                Debug.Log(packet.Payload);
                UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
            });
    }


    static void DealState(string payload, bool isOnline = false)
    {
        Debug.Log(JSON.Parse(payload));
        JSONNode jn = JSON.Parse(payload)[1];
        if (jn["status"] == "error")
        {
            UIManager.ShowErrorMessage(jn["message"]);
        }
        else
        {
            if (GlobalManager.CURRENT_SCENE_SERVICE == "tank")
            {
                SceneMsgDealer.DealTankMsg(jn, isOnline);
            }
            else if (GlobalManager.CURRENT_SCENE_SERVICE == "inspection")
            {
                SceneMsgDealer.DealInspectionMsg(jn, isOnline);
            }
        }
    }

    public void WARN_MESSAGE()
    {
        SocketService socketService = socket;
        socketService.AddListener(EventConfig.WARN_MESSAGE,
            (socket, packet, args) =>
            {
                Debug.Log(packet.Payload);
                UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
            });
        socketService.AddListener(
            EventConfig.AR_DISCONNECT,
            (socket, packet, args) =>
            {
                Debug.Log(packet.Payload);
                UIManager.ShowErrorMessage(JSON.Parse(packet.Payload)[1]["message"]);
            });
    }


    IEnumerator RefreshSocket()
    {
        while (true)
        {
            IsConnect = false;
            yield return new WaitForSeconds(3);
            //断开了连接，重连
            if (IsConnect)
            {
                UIManager.ChangeScreenEdgeColor(Color.white);
            }
            else
            {
                UIManager.ShowErrorMessage("网络已断开！");
                UIManager.ChangeScreenEdgeColor(Color.red);
                socket.Reconnect();
                Init(GlobalManager.CURRENT_SCENE_SERVICE);
            }
        }
    }
}