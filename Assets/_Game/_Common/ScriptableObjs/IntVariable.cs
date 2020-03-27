using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{
    #pragma warning disable 649
    [SerializeField] private int initVal;
    #pragma warning restore 649
    
    private int _runTimeVal;

    public void SetInt(int i) { _runTimeVal = i; }

    public int GetInt() { return _runTimeVal; }
    
    public void OnAfterDeserialize()
    {
        _runTimeVal = initVal;
    }
    
    public void OnBeforeSerialize()
    {
    }

}
