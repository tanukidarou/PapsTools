// v1.0.1
 
 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections.Generic;
 using System;

namespace Paps
{
    public class PrintConsole : MonoBehaviour
    {
        [SerializeField] private int lenghtConsole = 20;
        [SerializeField] private int fontSize = 2;
        
        private string newString;
        private string myLog;
        private string lastLog = "";
        private int countCollapse = 0;
        private List<string> myLogQueue = new List<string>();

        private string headPrint = "";
        private Text textPrint;

        private Canvas canvas;
        private CanvasScaler canvasScaler;
        private GraphicRaycaster graphicRaycaster;

        private string color;
        private enum ColorsToPrint {White = 0,Yellow, Red};

        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            CreateCanvas();
            CreateTextComponent();
            this.gameObject.name = "PrintConsole Tool";
        }

        void Start(){
            headPrint = CreateHead();
            textPrint.text = headPrint;
        }
    
        void OnEnable () {
            Application.logMessageReceived += HandleLog;
        }
        
        void OnDisable () {
            Application.logMessageReceived -= HandleLog;
        }
    
        void HandleLog(string logString, string stackTrace, LogType type){
            myLog = logString;
            newString = "\n [" + type + "] (" + DateTime.Now.ToString("HH:mm:ss") + ") : " + myLog;

            if(myLog != lastLog)
            {
                if(countCollapse != 0)
                {
                    countCollapse = 0;
                }
            }
            else 
            {
                ++countCollapse;
                newString += " (" + countCollapse + ")";
            }

            switch (type)
            {
                case LogType.Error:
                    AddColor(ref newString, ColorsToPrint.Red);
                    break;
                case LogType.Assert:
                    AddColor(ref newString, ColorsToPrint.Red);
                    AddBold(ref newString);
                    break;
                    case LogType.Warning:
                    AddColor(ref newString, ColorsToPrint.Yellow);
                    break;
                case LogType.Exception:
                    AddColor(ref newString, ColorsToPrint.Red);
                    AddItalics(ref newString);
                break;
                default: break;
            }

            if(myLog != lastLog)
            {
                myLogQueue.Insert(0,newString);
                lastLog = myLog;
            }
            else 
            {
                myLogQueue[0] = newString;
            }

            if (type == LogType.Exception)
            {
                newString = "\n" + stackTrace;
                myLogQueue.Insert(0,newString);
            }
            textPrint.text = string.Empty;
            textPrint.text = headPrint;

            for (int i = 0; i < myLogQueue.Count; i++)
            {
                textPrint.text += myLogQueue[i];

                if(i >= lenghtConsole)
                    break;
            }

        }

        private string CreateHead()
        {
            string guid;
            if(Application.buildGUID.Length == 0)
            {
                guid = "unityEditor";
            }
            else
            {
                guid = Application.buildGUID.Substring(0,5);
            }

            return Application.productName + " - " + 
            Application.companyName + " - v" + 
            Application.version + " (" + 
            guid + ") [Unity " + 
            Application.unityVersion + "]\n";
        }
    

        private void AddItalics(ref string value)
        {
            value = "<i>" + value + "</i>";
        }

        private void AddBold(ref string value)
        {
            value = "<b>" + value + "</b>";
        }

        private void AddColor(ref string value, ColorsToPrint colorValue)
        {
            switch ((int)colorValue)
            {
                case 0: color = "white"; break;
                case 1: color = "yellow"; break;
                case 2: color = "red"; break;
                default: color = "white"; break;
            }

            value = "<color=" + color + ">" + value + "</color>";
        }

        private void CreateCanvas()
        {
            canvas = this.gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.targetDisplay = 0;
            canvasScaler = this.gameObject.AddComponent<CanvasScaler>();
            canvasScaler.scaleFactor = 10.0f;
            canvasScaler.dynamicPixelsPerUnit = 10f;
            graphicRaycaster = this.gameObject.AddComponent<GraphicRaycaster>();
            graphicRaycaster.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 3.0f);
            graphicRaycaster.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 3.0f);
        }

        private void CreateTextComponent()
        {
            textPrint = this.gameObject.AddComponent<Text>();
            textPrint.alignment = TextAnchor.UpperLeft;
            textPrint.horizontalOverflow = HorizontalWrapMode.Overflow;
            textPrint.verticalOverflow = VerticalWrapMode.Overflow;
            Font ArialFont = (Font)Resources.GetBuiltinResource (typeof(Font), "Arial.ttf");
            textPrint.font = ArialFont;
            textPrint.fontSize = fontSize;
            textPrint.text = "Start Console";
            textPrint.enabled = true;
            textPrint.color = Color.white;
            textPrint.raycastTarget = false;
        }

    }
}
