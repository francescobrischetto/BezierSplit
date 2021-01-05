using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPanelRuntimeManagement : MonoBehaviour
{
    public bool visibility;

    public void SetControlPolyVisible(bool val)
    {
        GameObject.FindGameObjectWithTag("FullPolygon").GetComponent<SetControlPolygon>().visible = val;
    }

    public void SetSurfaceVisible(bool val)
    {
        GameObject.FindGameObjectWithTag("FullSurface").GetComponent<SetFullBezierSurface>().visible = val;
    }

    public void SetVolumeVisible(bool val)
    {
        GameObject.FindGameObjectWithTag("FullVolume").GetComponent<SetVolumeVisualizer>().visible = val;
    }
}
