using UnityEngine;
using UnityEngine.UI;

namespace Scratch
{
    [RequireComponent(typeof(Text))]
    public class BuildVersion : MonoBehaviour
    {

        [SerializeField] private string androidVersion;

#if UNITY_STANDALONE_WIN || UNITY_WEBGL
        
        void Awake()
        {
            string guid = "";
            if (!string.IsNullOrEmpty(Application.buildGUID))
            {
                guid = Application.buildGUID.Substring(0,5);
            }
            else
            {
                guid = "editor";
            }
            GetComponent<Text>().text = "v" + Application.version + "." +guid;
        }

#elif  UNITY_ANDROID

        void Awake()
        {
            string guid = "";
            if (!string.IsNullOrEmpty(Application.buildGUID))
            {
                guid = Application.buildGUID.Substring(0, 5);
            }
            else
            {
                guid = "editor";
            }
            GetComponent<Text>().text = "v" + androidVersion + "." + guid;
        }
#endif

    }
}


