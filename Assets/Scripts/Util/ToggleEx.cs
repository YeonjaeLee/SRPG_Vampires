// Decompiled with JetBrains decompiler
// Type: UnityEngine.UI.Toggle
// Assembly: UnityEngine.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9F0ABE59-6D72-4F5D-9054-6F0939386D1C
// Assembly location: /Applications/Unity/Unity.app/Contents/UnityExtensions/Unity/GUISystem/UnityEngine.UI.dll

using System;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
  /// <summary>
  ///   <para>A standard toggle that has an on / off state.</para>
  /// </summary>
  [AddComponentMenu("UI/Toggle Ex", 32)]
  [RequireComponent(typeof (RectTransform))]
  public class ToggleEx : Selectable, IPointerClickHandler, ISubmitHandler, ICanvasElement, IEventSystemHandler
  {
    /// <summary>
    ///   <para>Transition mode for the toggle.</para>
    /// </summary>
    public ToggleEx.ToggleTransition toggleTransition = ToggleEx.ToggleTransition.Fade;
    /// <summary>
    ///   <para>Callback executed when the value of the toggle is changed.</para>
    /// </summary>
    public ToggleEx.ToggleEvent onValueChanged = new ToggleEx.ToggleEvent();
    /// <summary>
    ///   <para>Graphic affected by the toggle.</para>
    /// </summary>
    public Graphic graphic;
    //[SerializeField]
    private ToggleGroupEx m_Group;
    [FormerlySerializedAs("m_IsActive")]
    [Tooltip("Is the toggle currently on or off?")]
    [SerializeField]
    private bool m_IsOn;

    protected ToggleEx()
    {
    }

    /// <summary>
    ///   <para>Group the toggle belongs to.</para>
    /// </summary>
    public ToggleGroupEx group
    {
      get
      {
        return this.m_Group;
      }
      set
      {
        this.m_Group = value;
        if (!Application.isPlaying)
          return;
//        this.SetToggleGroup(this.m_Group, true);
//        this.PlayEffect(true);
      }
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
      base.OnValidate();
      if (PrefabUtility.GetPrefabType((UnityEngine.Object) this) == PrefabType.Prefab || Application.isPlaying)
        return;
      CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild((ICanvasElement) this);
    }
#endif
    /// <summary>
    ///   <para>Handling for when the canvas is rebuilt.</para>
    /// </summary>
    /// <param name="executing"></param>
    public virtual void Rebuild(CanvasUpdate executing)
    {
      if (executing != CanvasUpdate.Prelayout)
        return;
      this.onValueChanged.Invoke(this.m_IsOn);
    }

    /// <summary>
    ///   <para>See ICanvasElement.LayoutComplete.</para>
    /// </summary>
    public virtual void LayoutComplete()
    {
    }

    /// <summary>
    ///   <para>See ICanvasElement.GraphicUpdateComplete.</para>
    /// </summary>
    public virtual void GraphicUpdateComplete()
    {
    }

    protected override void OnEnable()
    {
      base.OnEnable();
//      this.SetToggleGroup(this.m_Group, false);
      this.PlayEffect(true);
    }

    /// <summary>
    ///   <para>See MonoBehaviour.OnDisable.</para>
    /// </summary>
    protected override void OnDisable()
    {
  //    this.SetToggleGroup((ToggleGroupEx) null, false);
      base.OnDisable();
    }

    protected override void OnDidApplyAnimationProperties()
    {
      if ((UnityEngine.Object) this.graphic != (UnityEngine.Object) null)
      {
        bool flag = !Mathf.Approximately(this.graphic.canvasRenderer.GetColor().a, 0.0f);
        if (this.m_IsOn != flag)
        {
          this.m_IsOn = flag;
          this.Set(!flag);
        }
      }
      base.OnDidApplyAnimationProperties();
    }

//    private void SetToggleGroup(ToggleGroupEx newGroup, bool setMemberValue)
//    {
//      ToggleGroupEx group = this.m_Group;
//      if ((UnityEngine.Object) this.m_Group != (UnityEngine.Object) null)
//        this.m_Group.UnregisterToggle(this);
//      if (setMemberValue)
//        this.m_Group = newGroup;
//      if ((UnityEngine.Object) newGroup != (UnityEngine.Object) null && this.IsActive())
//        newGroup.RegisterToggle(this);
//      if (!((UnityEngine.Object) newGroup != (UnityEngine.Object) null) || !((UnityEngine.Object) newGroup != (UnityEngine.Object) group) || (!this.isOn || !this.IsActive()))
//        return;
//      newGroup.NotifyToggleOn(this);
//    }

    /// <summary>
    ///   <para>Return or set whether the Toggle is on or not.</para>
    /// </summary>
    public bool isOn
    {
      get
      {
        return this.m_IsOn;
      }
      set
      {
        this.Set(value);
      }
    }

    private void Set(bool value)
    {
      this.Set(value, true);
    }

    private void Set(bool value, bool sendCallback)
    {
      if (this.m_IsOn == value)
        return;
      this.m_IsOn = value;
      if ((UnityEngine.Object) this.m_Group != (UnityEngine.Object) null && this.IsActive() && (this.m_IsOn || !this.m_Group.AnyTogglesOn() && !this.m_Group.allowSwitchOff))
      {
        this.m_IsOn = true;
        this.m_Group.NotifyToggleOn(this);
      }
      this.PlayEffect(this.toggleTransition == ToggleEx.ToggleTransition.None);
      if (!sendCallback)
        return;
      UISystemProfilerApi.AddMarker("Toggle.value", (UnityEngine.Object) this);
      this.onValueChanged.Invoke(this.m_IsOn);
    }

    private void PlayEffect(bool instant)
    {
      if ((UnityEngine.Object) this.graphic == (UnityEngine.Object) null)
        return;
      if (!Application.isPlaying) {
        foreach(var g in this.graphic.GetComponentsInChildren<Graphic>())
        {
            g.canvasRenderer.SetAlpha(!this.m_IsOn ? 0.0f : 1f);
        }
        
      } else {
        foreach(var g in this.graphic.GetComponentsInChildren<Graphic>())
        {
          g.CrossFadeAlpha(!this.m_IsOn ? 0.0f : 1f, !instant ? 0.1f : 0.0f, true);
        }
      
      }
    }

    protected override void Start()
    {
      this.PlayEffect(true);
    }

    private void InternalToggle()
    {
      if (!this.IsActive() || !this.IsInteractable())
        return;
      this.isOn = !this.isOn;
    }

    /// <summary>
    ///   <para>Handling for when the toggle is 'clicked'.</para>
    /// </summary>
    /// <param name="eventData">Current event.</param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
      if (eventData.button != PointerEventData.InputButton.Left)
        return;
      this.InternalToggle();
    }

    /// <summary>
    ///   <para>Handling for when the submit key is pressed.</para>
    /// </summary>
    /// <param name="eventData">Current event.</param>
    public virtual void OnSubmit(BaseEventData eventData)
    {
      this.InternalToggle();
    }

//    Transform ICanvasElement.get_transform()
//    {
//      return this.transform;
//    }

    /// <summary>
    ///   <para>Display settings for when a toggle is activated or deactivated.</para>
    /// </summary>
    public enum ToggleTransition
    {
      None,
      Fade,
    }

    /// <summary>
    ///   <para>UnityEvent callback for when a toggle is toggled.</para>
    /// </summary>
    [Serializable]
    public class ToggleEvent : UnityEvent<bool>
    {
    }
  }
}
