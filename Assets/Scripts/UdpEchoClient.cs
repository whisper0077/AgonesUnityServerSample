using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UdpEchoClient : MonoBehaviour
{
    [SerializeField]
    private InputField sendTextField;
    [SerializeField]
    private Text receivedText;
    [SerializeField]
    private InputField serverHostField;
    [SerializeField]
    private InputField serverPortField;

    private UdpClient client;
    public string ServerHost { get; private set; } = "127.0.0.1";
    public int ServerPort { get; private set; } = 7777;

    void Start()
    {
        serverHostField.text = ServerHost;
        serverPortField.text = ServerPort.ToString();

        client = new UdpClient(ServerHost, ServerPort);
    }

    void Update()
    {
        if (client.Available > 0)
        {
            IPEndPoint remote = null;
            byte[] rbytes = client.Receive(ref remote);
            string received = Encoding.UTF8.GetString(rbytes);

            Debug.Log($"Client - Recv {received}");

            receivedText.text = received;
        }
    }


    public void ChangeServer()
    {
        if (IPAddress.TryParse(serverHostField.text, out IPAddress ip))
            ServerHost = ip.ToString();
        if (int.TryParse(serverPortField.text, out int port))
            ServerPort = port;

        client = new UdpClient(ServerHost, ServerPort);

        Debug.Log($"Client - ChangeServer {ServerHost}:{ServerPort}");
    }

    public void SendTextToServer()
    {
        if (string.IsNullOrWhiteSpace(sendTextField.text))
            return;

        Debug.Log($"Client - SendText {sendTextField.text}");

        byte[] bytes = Encoding.UTF8.GetBytes(sendTextField.text);
        client.Send(bytes, bytes.Length);

        sendTextField.text = "";
    }
}
