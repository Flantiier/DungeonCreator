using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

namespace _Scripts.Cameras
{
    public class SwitchableCamera : MonoBehaviour
    {
        [TitleGroup("Properties")]
        [LabelWidth(100)]
        [SerializeField] private CinemachineVirtualCamera vCam;
        [LabelWidth(100)]
        [SerializeField] private bool activeAtStart;

        private void OnEnable()
        {
            //CameraSwitcher.Register(vCam);
        }
        private void OnDisable()
        {

            //CameraSwitcher.Unregister(vCam);
        }
    }
}