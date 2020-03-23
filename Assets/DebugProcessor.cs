using UnityEngine;

public class DebugProcessor : MonoBehaviour
{
    #pragma warning disable 649
    [SerializeField] private BoolVariable varColliderDisabler;
    #pragma warning restore 649
    
    
    // messages
    private void Update()
    {
        ProcessDebugKeys();
    }
    
    // methods
    private void ProcessDebugKeys()
    {
        if (Debug.isDebugBuild)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                StartCoroutine(SceneLoader.LoadNextLevel(0));
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                varColliderDisabler.SetBool(!varColliderDisabler.GetBool());
            }
        }
    }

}
