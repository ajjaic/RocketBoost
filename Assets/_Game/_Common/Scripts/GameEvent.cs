using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    public delegate void CallBackListener();
    public event CallBackListener CallbackListener;
    
    public void RaiseEvent()
    {
        CallbackListener?.Invoke();
    }
}
