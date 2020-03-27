using UnityEngine;

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
