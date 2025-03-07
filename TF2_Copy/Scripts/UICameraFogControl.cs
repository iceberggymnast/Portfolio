using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraFogControl : MonoBehaviour
{
    private bool originalFogState;

    void OnPreRender()
    {
        originalFogState = RenderSettings.fog;
        RenderSettings.fog = false;
    }

    void OnPostRender()
    {
        RenderSettings.fog = originalFogState;
    }
}
