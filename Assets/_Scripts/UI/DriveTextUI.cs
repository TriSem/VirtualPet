using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DriveTextUI : MonoBehaviour
{
    [SerializeField] 
    Drive drive = default;

    [SerializeField] 
    PetAgent agent = default;

    Text text = default;
    string baseString = default;

    void Start()
    {
        text = GetComponent<Text>();
        baseString = text.text;
    }

    void Update()
    {
        text.text = baseString + agent.DriveVector.GetValue(drive).ToString("F2");
    }
}
