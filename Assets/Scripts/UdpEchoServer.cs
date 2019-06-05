using Agones;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UdpEchoServer : MonoBehaviour
{
    private int Port { get; set; } = 7777;
    private UdpClient client = null;
    private AgonesRestClient agones = null;

    async void Start()
    {
        client = new UdpClient(Port);

        agones = GetComponent<AgonesRestClient>();
        bool ok = await agones.Ready();
        if (ok)
        {
            Log($"Server - Ready");
        }
        else
        {
            Log($"Server - Cound not ready");
            Application.Quit();
        }
    }

    async void Update()
    {
        while (client.Available > 0)
        {
            IPEndPoint remote = null;
            byte[] recvBytes = client.Receive(ref remote);
            string recvText = Encoding.UTF8.GetString(recvBytes);

            string[] recvTexts = recvText.Split(' ');
            bool ok = false;
            switch (recvTexts[0])
            {
                case "Shutdown":
                    ok = await agones.Shutdown();
                    Log($"Server - Shutdown {ok}");
                    Application.Quit();
                    break;

                case "Allocate":
                    ok = await agones.Allocate();
                    Log($"Server - Allocate {ok}");
                    break;

                case "Label":
                    if (recvTexts.Length == 3)
                    {
                        (string key, string value) = (recvTexts[1], recvTexts[2]);
                        ok = await agones.SetLabel(key, value);
                        Log($"Server - SetLabel {ok}");
                    }
                    break;

                case "Annotation":
                    if (recvTexts.Length == 3)
                    {
                        (string key, string value) = (recvTexts[1], recvTexts[2]);
                        ok = await agones.SetAnnotation(key, value);
                        Log($"Server - SetAnnotation {ok}");
                    }
                    break;
            }

            byte[] echo = Encoding.UTF8.GetBytes($"Echo : {recvText}");
            client.Send(echo, echo.Length, remote);

            Log($"Server - Receive[{remote.ToString()}] : {recvText}");
        }
    }

    void OnDestroy()
    {
        client.Close();
        Log("Server - Close");
    }

    void Log(object message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#else
        Console.WriteLine(message);
#endif
    }
}