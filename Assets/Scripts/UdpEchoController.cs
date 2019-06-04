using UnityEngine;

public class UdpEchoController : MonoBehaviour
{
    [SerializeField]
    private GameObject server;
    [SerializeField]
    private GameObject client;

    void Start()
    {
        server.SetActive(false);
        client.SetActive(false);

#if (UNITY_EDITOR || UNITY_SERVER)
        server.SetActive(true);
#endif
#if !UNITY_SERVER
        client.SetActive(true);
#endif
    }
}
