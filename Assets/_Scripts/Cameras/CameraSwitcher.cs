using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

namespace _Scripts.Cameras
{
    public class CameraSwitcher : MonoBehaviour
    {        
        [Required, SerializeField, LabelWidth(100)] private CinemachineBrain brain;

        [ShowInInspector]
        public List<CinemachineVirtualCamera> cameras { get; private set; } = new List<CinemachineVirtualCamera>();
        public CinemachineVirtualCamera ActiveCamera { get; private set; }

        public static event Action<CinemachineVirtualCamera> OnCameraAdd;

        /// <summary>
        /// Indicates if the given camera is the active one
        /// </summary>
        public bool IsActiveCamera(CinemachineVirtualCamera cam)
        {
            return cam == ActiveCamera;
        }

        /// <summary>
        /// Set a new active camera
        /// </summary>
        public void SetActiveCamera(CinemachineVirtualCamera cam)
        {
            if (IsActiveCamera(cam))
                return;

            //Set active cam
            ActiveCamera = cam;
            ActiveCamera.Priority = 10;

            foreach (CinemachineVirtualCamera camera in cameras)
                camera.Priority = 5;
        }

        /// <summary>
        /// Add a virtual camera to the list
        /// </summary>
        public void Register(CinemachineVirtualCamera cam)
        {
            if (cameras.Contains(cam))
                return;

            cameras.Add(cam);
        }

        /// <summary>
        /// Remove a virtual camera from the list
        /// </summary>
        /// <param name="cam"></param>
        public void Unregister(CinemachineVirtualCamera cam)
        {
            if (!cameras.Contains(cam))
                return;

            cameras.Remove(cam);
        }
    }
}
