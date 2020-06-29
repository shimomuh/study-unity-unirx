using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ui
{
    public interface IBase
    { 
        void AddLabel(string text);
        void AddAction(string text, Action action);
    }

    public class Base : MonoBehaviour, IBase
    {
        protected const string LABEL = "LABEL";
        protected const string CLEAR_LINE = "CLEAR_LINE";
        protected List<Element> elements = new List<Element>();

        protected bool isHorizontal;
        protected int createdBtnCnt;
        
        protected Texture2D labelBackColor;

        protected int buttonsOnRow = 3;
        protected int layoutW, layoutH, buttonW, buttonH = 60;
        protected ScaledRect scaledRect;
        protected static GUIStyle defaultStyle;
        protected int top_mergin = 50;
        
        public void AddLabel(string text)
        {
            elements.Add(new Element(LABEL, () => MakeLabel(text)));
        }

        public void AddAction(string text, Action action)
        {
            elements.Add(new Element(text, action));
        }

        protected class Element
        {
            public string text;
            public Action action;

            public Element(string text, Action action)
            {
                this.text = text;
                this.action = action;
            }
        }

        protected void MakeLabel(string text)
        {
            ClearButtonLine();
            GUILayout.Space(20);
            
            GUIStyle labelStyle = new GUIStyle(GUI.skin.box);
            labelStyle.fontSize = 24;
            labelStyle.alignment = TextAnchor.MiddleLeft;
            labelStyle.normal.background = GetLabelColor();
            GUILayout.Box($" {text} ", labelStyle);
            GUILayout.Space(5);
        }

        protected void ClearButtonLine()
        {
            createdBtnCnt = 0;
            if (isHorizontal)
            {
                GUILayout.EndHorizontal();
                isHorizontal = false;
            }
        }

        protected Texture2D GetLabelColor()
        {
            if (labelBackColor == null)
            {
                labelBackColor = new Texture2D(1, 1);
                labelBackColor.SetPixel(1, 1, Color.black);
                labelBackColor.Apply();
            }

            return labelBackColor;
        }

        protected virtual void OnGUI()
        {
            CreateGUI();
        }
        
        protected virtual void CreateGUI()
        {
            buttonsOnRow = (buttonsOnRow == 0) ? 2 : buttonsOnRow;
            layoutW = (int) (Screen.width * 0.9);
            layoutH = (int) (Screen.height * 0.9);
            buttonW = (int) (layoutW / buttonsOnRow * 0.9);
            buttonH = (int) (buttonW * 0.3);
            if (scaledRect == null)
            {
                scaledRect = new ScaledRect();
            }

            if (defaultStyle == null)
            {
                defaultStyle = new GUIStyle(GUI.skin.button);
                defaultStyle.fontSize = (int) scaledRect.CalcSize(21);
            }

            GUILayout.Space(top_mergin);

            foreach (Element element in elements)
            {
                try
                {
                    switch (element.text)
                    {
                        case LABEL:
                        case CLEAR_LINE:
                            element.action();
                            break;
                        default:
                            if (MakeButton(element.text))
                            {
                                element.action();
                            }

                            break;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }

            ClearButtonLine();
        }

        protected bool MakeButton(string label)
        {
            try
            {
                ++createdBtnCnt;
                if (createdBtnCnt % buttonsOnRow == 1)
                {
                    GUILayout.BeginHorizontal();
                    isHorizontal = true;
                }

                GUILayout.Space(15);
                GUI.depth = 5;
                bool on = GUILayout.Button(label, defaultStyle,
                    GUILayout.MinWidth(buttonW), GUILayout.MaxWidth(buttonW),
                    GUILayout.MaxHeight(buttonH));

                if (createdBtnCnt % buttonsOnRow == 0 && isHorizontal)
                {
                    GUILayout.EndHorizontal();
                    isHorizontal = false;
                    createdBtnCnt = 0;
                }

                return on;
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return false;
            }
        }
    }
}