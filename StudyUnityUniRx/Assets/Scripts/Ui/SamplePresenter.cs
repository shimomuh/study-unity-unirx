using UniRx;
using UniRxSample;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class SamplePresenter : Base
    {
        public void Awake()
        {
            AddLabel("UnityWebRequestSample");
            AddAction("フレーム待ちなし", UnityWebRequestNoWait);
            AddAction("1フレーム待ち", UnityWebRequest1sWait);
            AddAction("10フレーム待ち", UnityWebRequest10sWait);
        }
        
        private void UnityWebRequestNoWait()
        {
            UnityWebRequestSample();
        }

        private void UnityWebRequest1sWait()
        {
            UnityWebRequestSample(1);
        }
       
        private void UnityWebRequest10sWait()
        {
            UnityWebRequestSample(10);
        }

        private void UnityWebRequestSample(int delayFrame = 0)
        {
            string imageUrl = "https://pbs.twimg.com/profile_images/998489163136622592/A4o6jH9h_400x400.jpg";
            UnityWebRequestExecution execution = new UnityWebRequestExecution();
            Observable.ReturnUnit()
                .Select(_ => StartCoroutine(execution.Get(imageUrl)))
                .DelayFrame(delayFrame)
                .Select(_ => execution.GetTexture())
                .Do(texture => OpenModalWindow(texture))
                .Subscribe();
        }

        private void OpenModalWindow(Texture texture)
        {
            Canvas canvas = gameObject.AddComponent<Canvas>();
            RawImage image = canvas.gameObject.AddComponent<RawImage>();
            image.texture = texture;
            image.transform.localScale = new Vector2(0.01f, 0.01f);
        }
    }
}
