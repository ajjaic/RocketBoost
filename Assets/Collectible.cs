using UnityEngine;

public class Collectible : MonoBehaviour
{
    #pragma warning disable 649
    [SerializeField] private BoolVariable isAllStarsCollected;
    #pragma warning restore 649

    private int _currentStarIndex;
    private StarCollectible[] _collectibles;

    // messages
    private void Start()
    {
        _collectibles = transform.GetComponentsInChildren<StarCollectible>(true);
        ActivateNextStar();
    }

    // methods
    private void SetIsStarsCollectedVar()
    {
        var activeChildCount = transform.GetComponentsInChildren<StarCollectible>(false).Length;
        isAllStarsCollected.SetBool(activeChildCount == 0);
    }

    // API
    public void ActivateNextStar()
    {
        if (_currentStarIndex < _collectibles.Length)
        {
            _collectibles[_currentStarIndex].gameObject.SetActive(true);
            _currentStarIndex += 1;
        }
        
        SetIsStarsCollectedVar();
    }
}
