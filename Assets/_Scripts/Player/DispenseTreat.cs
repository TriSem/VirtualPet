using UnityEngine;

// Lets the player spawn treats in their hand.
public class DispenseTreat : MonoBehaviour
{
    [SerializeField] Transform treatPrefab = default;
    [SerializeField] Transform cursor = default;
    [SerializeField] ObjectSelection objectSelection = null;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && !objectSelection.HoldingItem)
        {
            var instance = Instantiate(treatPrefab, cursor).GetComponent<WorldObject>();
            objectSelection.PlaceObjectIntoHand(instance);
        }
    }
}
