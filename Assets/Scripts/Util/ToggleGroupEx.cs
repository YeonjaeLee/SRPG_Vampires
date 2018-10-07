// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.ToggleGroup
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9F0ABE59-6D72-4F5D-9054-6F0939386D1C
// Assembly location: /Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/GUISystem/UnityEngine.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A component that represents a group of UI.Toggles.</para>
  /// </summary>
  [AddComponentMenu("UI/Toggle Group Extend", 33)]
  [DisallowMultipleComponent]
  public class ToggleGroupEx : UIBehaviour
  {
    [SerializeField]
    private bool m_AllowSwitchOff = false;
    [SerializeField]
    private List<ToggleEx> m_Toggles = new List<ToggleEx>();
    
    public ToggleGroupEvent onValueChanged = new ToggleGroupEvent();
    protected ToggleGroupEx()
    {
    }

    /// <summary>
    ///   <para>Is it allowed that no toggle is switched on?</para>
    /// </summary>
    public bool allowSwitchOff
    {
      get
      {
        return this.m_AllowSwitchOff;
      }
      set
      {
        this.m_AllowSwitchOff = value;
      }
    }
    protected override void OnEnable()
    {
        foreach(var t in m_Toggles)
        {
          t.group=this;
        }
    }

    private void ValidateToggleIsInGroup(ToggleEx toggle)
    {
      if ((UnityEngine.Object) toggle == (UnityEngine.Object) null || !this.m_Toggles.Contains(toggle))
        throw new ArgumentException(string.Format("Toggle {0} is not part of ToggleGroup {1}", new object[2]
        {
          (object) toggle,
          (object) this
        }));
    }

    public void NotifyToggleOn(ToggleEx toggle)
    {
      this.ValidateToggleIsInGroup(toggle);
      
      for (int index = 0; index < this.m_Toggles.Count; ++index)
      {
        if (!((UnityEngine.Object) this.m_Toggles[index] == (UnityEngine.Object) toggle))
          this.m_Toggles[index].isOn = false;
        else
        {
          if(toggle.isOn) onValueChanged.Invoke(index);
        }
      }
    }

    public void UnregisterToggle(ToggleEx toggle)
    {
      if (!this.m_Toggles.Contains(toggle))
        return;
      this.m_Toggles.Remove(toggle);
    }

    public void RegisterToggle(ToggleEx toggle)
    {
      if (this.m_Toggles.Contains(toggle))
        return;
      this.m_Toggles.Add(toggle);
    }

    /// <summary>
    ///   <para>Are any of the toggles on?</para>
    /// </summary>
    public bool AnyTogglesOn()
    {
      return (UnityEngine.Object) this.m_Toggles.Find((Predicate<ToggleEx>) (x => x.isOn)) != (UnityEngine.Object) null;
    }

    /// <summary>
    ///   <para>Returns the toggles in this group that are active.</para>
    /// </summary>
    /// <returns>
    ///   <para>The active toggles in the group.</para>
    /// </returns>
    public IEnumerable<ToggleEx> ActiveToggles()
    {
      return (IEnumerable<ToggleEx>) Enumerable.Where(this.m_Toggles, x => x.isOn);
    }
    
    public int ActiveToggleIndex()
    {
        for(int i=0;i<m_Toggles.Count;i++)
        {
            if(m_Toggles[i].isOn) return i;
        }
      return -1;
    }

    /// <summary>
    ///   <para>Switch all toggles off.</para>
    /// </summary>
    public void SetAllTogglesOff()
    {
      bool allowSwitchOff = this.m_AllowSwitchOff;
      this.m_AllowSwitchOff = true;
      for (int index = 0; index < this.m_Toggles.Count; ++index)
        this.m_Toggles[index].isOn = false;
      this.m_AllowSwitchOff = allowSwitchOff;
    }
    
    [Serializable]
    public class ToggleGroupEvent : UnityEvent<int>
    {
    }

    public void TogglesChangeOn(int i)
    {
            m_Toggles[i].isOn = true;
    }
  }
}
