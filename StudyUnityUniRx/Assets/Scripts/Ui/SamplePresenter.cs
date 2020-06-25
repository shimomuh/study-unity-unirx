using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class SamplePresenter : Base
    {
        public void Awake()
        {
            AddLabel("Test Label");
            AddAction("button test", () => { Debug.Log("clicked"); });
        }
    }
}
