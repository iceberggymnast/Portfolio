using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AimDof : MonoBehaviour
{
    public PlayerFire playerFire;
    public Volume volume;
    DepthOfField depthOfField;
    private void Start()
    {
        volume.profile.TryGet<DepthOfField>(out depthOfField);
    }

    void Update()
    {

                if (playerFire.isAiming)
                {
                    depthOfField.focalLength.value = Mathf.Lerp(depthOfField.focalLength.value, 32, Time.deltaTime * 10.0f);
                }
                else
                {
                    depthOfField.focalLength.value = Mathf.Lerp(depthOfField.focalLength.value, 20, Time.deltaTime * 10.0f);
                }

    }
}
