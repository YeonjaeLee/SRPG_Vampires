using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class Extension {
    
    public static void AddEvent(this Button target, System.Action ac)
    {
        target.onClick.AddListener( () => ac() );
    }

    public static void Show(this Component target)
    {
        if (target == null)
            return;

        target.gameObject.SetActive(true);
    }

    public static void Hide(this Component target)
    {
        if (target == null)
            return;

        target.gameObject.SetActive(false);
    }

    public static void Show(this Transform target)
    {
        if (target == null)
            return;

        target.gameObject.SetActive(true);
    }

    public static void Hide(this Transform target)
    {
        if (target == null)
            return;

        target.gameObject.SetActive(false);
    }
}
