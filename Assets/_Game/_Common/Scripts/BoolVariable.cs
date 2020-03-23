using UnityEngine;

[CreateAssetMenu]
public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
{
    #pragma warning disable 649
    [SerializeField] private bool initVal;
    #pragma warning restore 649
    
    private bool _runTimeVal;

    public void SetBool(bool b) { _runTimeVal = b; }

    public bool GetBool() { return _runTimeVal; }
    
    public void OnAfterDeserialize()
    {
        _runTimeVal = initVal;
    }
    
    public void OnBeforeSerialize()
    {
    }

}
