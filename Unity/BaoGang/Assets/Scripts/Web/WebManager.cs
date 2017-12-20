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
		if (sceneName == "tank")
		{
			TankSocketService.Instance.RegistServices();
		}
		else if (sceneName == "inspection")
		{
			InspectionSocketService.Instance.RegistServices();
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