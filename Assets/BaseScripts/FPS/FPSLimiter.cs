using UnityEngine;

namespace Game.Scripts.Common.FPSCounter
{
    public class FPSLimiter : MonoBehaviour
    {
        [SerializeField][Range(15,120)] private int _fpsLimit = 60;
        void Start()
        {
#if UNITY_EDITOR

            Application.targetFrameRate = _fpsLimit;

#endif
        }
    }
}
