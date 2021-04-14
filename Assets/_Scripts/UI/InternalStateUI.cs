using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class InternalStateUI : MonoBehaviour
{
    [SerializeField] PetAgent agent = null;
    Text text;
    StringBuilder builder = new StringBuilder();

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        builder.Clear();
        foreach (var state in agent.InternalModel.ToHashSet())
        {
            builder.Append(state.ToString());
            builder.Append("\n");
        }
        text.text = builder.ToString();
    }
}
