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
    private AgonesSdk agones = null;

    async void Start()
    {
        client = new UdpClient(Port);

        agones = GetComponent<AgonesSdk>();
        bool ok = await agones.Ready();
        if (ok)
        {
            Debug.Log($"Server - Ready");
        }
        else
        {
            Debug.Log($"Server - Ready failed");
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
                    Debug.Log($"Server - Shutdown {ok}");
                    Application.Quit();
                    break;

                case "Allocate":
                    ok = await agones.Allocate();
                    Debug.Log($"Server - Allocate {ok}");
                    break;

                case "Label":
                    if (recvTexts.Length == 3)
                    {
                        (string key, string value) = (recvTexts[1], recvTexts[2]);
                        ok = await agones.SetLabel(key, value);
                        Debug.Log($"Server - SetLabel {ok}");
                    }
                    break;

                case "Annotation":
                    if (recvTexts.Length == 3)
                    {
                        (string key, string value) = (recvTexts[1], recvTexts[2]);
                        ok = await agones.SetAnnotation(key, value);
                        Debug.Log($"Server - SetAnnotation {ok}");
                    }
                    break;
            }

            byte[] echo = Encoding.UTF8.GetBytes($"Echo : {recvText}");
            client.Send(echo, echo.Length, remote);

            Debug.Log($"Server - Receive[{remote.ToString()}] : {recvText}");
        }
    }

    void OnDestroy()
    {
        client.Close();
        Debug.Log("Server - Close");
    }
}