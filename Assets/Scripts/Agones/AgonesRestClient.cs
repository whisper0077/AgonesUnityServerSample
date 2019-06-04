using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Agones
{
    /// <summary>
    /// Agones Rest Client for Unity
    /// </summary>
    public class AgonesRestClient : MonoBehaviour
    {
        [Range(1, 5)]
        public float HealthIntervalSecond = 2.0f;
        public bool HealthEnabled = true;
        public bool LogEnabled = true;

        private float CurrentHealthTime { get; set; } = 0;
        private const string SidecarAddress = "http://localhost:59358";

        private struct KeyValueMessage
        {
            public string key;
            public string value;
            public KeyValueMessage(string k, string v) => (key, value) = (k, v);
        }

        #region Unity Methods
        // Use this for initialization
        void Start()
        {
            CurrentHealthTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (!HealthEnabled)
                return;

            CurrentHealthTime += Time.unscaledDeltaTime;
            if (CurrentHealthTime >= HealthIntervalSecond)
            {
                HealthAsync();
                CurrentHealthTime = 0;
            }
        }
        #endregion

        #region AgonesRestClient Public Methods
        /// <summary>
        /// Marks this Game Server as ready to receive connections
        /// </summary>
        /// <param name="onCompleted">An Action that is invoked with when this operation completed</param>
        public void ReadyAsync(Action<bool> onCompleted = null)
        {
            StartCoroutine(SendPost("/ready", onCompleted));
        }

        /// <summary>
        /// Marks this Game Server as ready to shutdown
        /// </summary>
        /// <param name="onCompleted">An Action that is invoked when this operation completed</param>
        public void ShutdownAsync(Action<bool> onCompleted = null)
        {
            StartCoroutine(SendPost("/shutdown", onCompleted));
        }

        /// <summary>
        /// Marks this Game Server as Allocated
        /// </summary>
        /// <param name="onCompleted">An Action that is invoked when this operation completed</param>
        public void AllocateAsync(Action<bool> onCompleted = null)
        {
            StartCoroutine(SendPost("/allocate", onCompleted));
        }

        /// <summary>
        /// Set a metadata label that is stored in k8s
        /// </summary>
        /// <param name="key">label key</param>
        /// <param name="value">label value</param>
        /// <param name="onCompleted">An Action that is invoked when this operation completed</param>
        public void SetLabelAsync(string key, string value, Action<bool> onCompleted = null)
        {
            string json = JsonUtility.ToJson(new KeyValueMessage(key, value));
            StartCoroutine(SendPut("/metadata/label", json, onCompleted));
        }

        /// <summary>
        /// Set a metadata annotation that is stored in k8s
        /// </summary>
        /// <param name="key">annotation key</param>
        /// <param name="value">annotation value</param>
        /// <param name="onCompleted">An Action that is invoked when this operation completed</param>
        public void SetAnnotationAsync(string key, string value, Action<bool> onCompleted = null)
        {
            string json = JsonUtility.ToJson(new KeyValueMessage(key, value));
            StartCoroutine(SendPut("/metadata/annotation", json, onCompleted));
        }
        #endregion

        #region AgonesRestClient Private Methods
        void HealthAsync()
        {
            StartCoroutine(SendPost("/health"));
        }

        IEnumerator SendPost(string api, Action<bool> onCompleted = null)
        {
            return SendRequest(api, "{}", UnityWebRequest.kHttpVerbPOST, onCompleted);
        }

        IEnumerator SendPut(string api, string json, Action<bool> onCompleted = null)
        {
            return SendRequest(api, json, UnityWebRequest.kHttpVerbPUT, onCompleted);
        }

        IEnumerator SendRequest(string api, string json, string method, Action<bool> onCompleted = null)
        {
            var req = new UnityWebRequest(SidecarAddress + api, method)
            {
                uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            req.SetRequestHeader("Content-Type", "application/json");

            yield return req.SendWebRequest();

            bool ok = req.responseCode == 200;

            if (ok)
                Log($"Agones SendRequest ok: {api}");
            else
                Log($"Agones SendRequest failed: {api} {req.error}");

            onCompleted?.Invoke(ok);
        }

        void Log(object message)
        {
            if (!LogEnabled) return;
#if UNITY_EDITOR
            Debug.Log(message);
#else
            Console.WriteLine(message);
#endif
        }
        #endregion
    }
}
