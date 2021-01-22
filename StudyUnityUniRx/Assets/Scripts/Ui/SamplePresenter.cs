using System;
using System.Collections;
using UniRx;
using UniRxSample;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
            
            AddLabel("FromCoroutineSample");
            AddAction("処理の順序を理解する", FromCoroutineSample);
            
            AddLabel("WhenAllSample");
            AddAction("ReturnUnit あり", WhenAllEitherReturnUnitSample);
            AddAction("両方 FromCoroutine", WhenAllBothCorotineSample);

            AddLabel("IEnumerable -> Observable -> Last で1ストリームにして実行");
            AddAction("実行", ToObservableAndLastSample);
        }
        
        #region UnityWebRequest
        
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
        
        #endregion

        private void FromCoroutineSample()
        {
            Observable.FromCoroutine(CoroutineSample, publishEveryYield: false)
                .Subscribe(
                    _ => Debug.Log("OnNext"),
                    () => Debug.Log("OnCompleted")
                ).AddTo(gameObject);
        }

        private IEnumerator CoroutineSample()
        {
            Debug.Log("Coroutine started.");

            //なんか処理して待ち受ける的な
            yield return new WaitForSeconds(3);

            Debug.Log("Coroutine finished.");
        }
        
        private void WhenAllEitherReturnUnitSample()
        {
            Observable.WhenAll(
                Observable.FromCoroutine(_ => Coroutine5s()),
                Observable.ReturnUnit().Do(_ => Debug.Log("超高速で流れるログ"))
            )
                .Subscribe(_ => Debug.Log("Subscribe!!"));
        }

        private void WhenAllBothCorotineSample()
        {
            Observable.WhenAll(
                Observable.FromCoroutine(_ => Coroutine5s()),
                Observable.FromCoroutine(_ => Coroutine2s())
            )
            .Subscribe(_ => Debug.Log("Subscribe!!"));

        }

        private IEnumerator Coroutine5s()
        {
            Debug.Log("5s start.");

            //なんか処理して待ち受ける的な
            yield return new WaitForSeconds(5);

            Debug.Log("5s finished.");
        }
        
        
        private IEnumerator Coroutine2s()
        {
            Debug.Log("2s start.");

            //なんか処理して待ち受ける的な
            yield return new WaitForSeconds(2);

            Debug.Log("2s finished.");
            
        }

        private void  ToObservableAndLastSample()
        {
            Observable.ReturnUnit()
                .SelectMany(_ => EnumerableToObservable())
                .Subscribe();
        }
        
        private IObservable<int> EnumerableToObservable()
        {
            return Increment().ToObservable()
                .Do(id => Debug.Log($"{id} frame passed."))
                .Last()
                .Do(_ => Debug.Log("finished!"));
        }

        private IEnumerable<int> Increment()
        {
            int id = 0;
            while (true)
            {
                yield return ++id;
                if (id == 5) { break; }
            }
        }
    }
}
