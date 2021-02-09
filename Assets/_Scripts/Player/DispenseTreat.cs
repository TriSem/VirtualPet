using UnityEngine;

public class DispenseTreat : MonoBehaviour
{
    [SerializeField] Transform treatPrefab = default;
    [SerializeField] Transform cursor = default;
    Edible instance = null;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && instance == null)
        {
            instance = Instantiate(treatPrefab, cursor).GetComponent<Edible>();
            instance.transform.localPosition = Vector3.zero;
            instance.GetComponent<Rigidbody>().isKinematic = true;
            instance.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            instance.transform.SetParent(null);
            instance.enabled = true;
            instance.GetComponent<Rigidbody>().isKinematic = false;
            instance = null;
        }
    }
}
