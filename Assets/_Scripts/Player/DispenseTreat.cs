using UnityEngine;

public class DispenseTreat : MonoBehaviour
{
    [SerializeField] Transform treatPrefab = default;
    Edible instance = null;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse1) && instance == null)
        {
            instance = Instantiate(treatPrefab).GetComponent<Edible>();
            instance.transform.SetParent(transform);
            instance.transform.localPosition = Vector3.zero;
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            instance.transform.SetParent(null);
            instance = null;
        }
    }
}
