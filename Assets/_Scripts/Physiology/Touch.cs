using System.Collections.Generic;
using UnityEngine;

public class Touch : MonoBehaviour
{
    [SerializeField] List<TouchReceptor> touchReceptors = null;

    public bool PetIsTouched()
    {
        foreach (var receptor in touchReceptors)
        {
            if (receptor.Fired)
                return true;
        }
        return false;
    }
}
