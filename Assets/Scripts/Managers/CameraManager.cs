using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    enum VirtualCameras
    {
        NoCamera = -1,
        CockpitCamera = 0,
        ChaseCamera
    }

    [Header("Virtual Cameras")]
    [SerializeField]
    List<GameObject> virtualCameras;

    VirtualCameras CameraKey
    {
        get
        {
            for (int i = 0; i < virtualCameras.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i)) return (VirtualCameras)i;
            }
            return VirtualCameras.NoCamera;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetActiveCamera(VirtualCameras.CockpitCamera);
    }

    // Update is called once per frame
    void Update()
    {
        SetActiveCamera(CameraKey);
    }
    private void SetActiveCamera(VirtualCameras activeCamera)
    {
        if (activeCamera == VirtualCameras.NoCamera) return;
        foreach(GameObject camera in virtualCameras) 
        {
            camera.SetActive(camera.tag.Equals(activeCamera.ToString()));
        }
    }

  
}
