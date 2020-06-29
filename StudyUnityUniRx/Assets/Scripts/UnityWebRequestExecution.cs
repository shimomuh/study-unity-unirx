using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace UniRxSample
{
    interface IUnityWebRequestExecution
    {
        IEnumerator Get(string url);
        Texture GetTexture();
    }

    public class UnityWebRequestExecution : IUnityWebRequestExecution
    {
        private UnityWebRequest unityWebRequest;

        public IEnumerator Get(string url)
        {
            unityWebRequest = UnityWebRequest.Get(url);
            unityWebRequest.downloadHandler = new DownloadHandlerTexture();
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.isHttpError || unityWebRequest.isNetworkError)
            {
                throw new UnityException(unityWebRequest.error);
            }

            yield return null;
        }

        public Texture GetTexture()
        {
            return ((DownloadHandlerTexture) unityWebRequest.downloadHandler).texture;
        }
    }
}