using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindManager : MonoBehaviour
{
    public static FindManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            var found = FindChildByName(child, name);
            if (found != null)
                return found;
        }
        return null;
    }
}
