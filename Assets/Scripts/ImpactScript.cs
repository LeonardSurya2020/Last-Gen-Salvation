using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactScript : MonoBehaviour
{

    public void OnDestroyAfterAnimation()
    {
        Destroy(gameObject);
    }
}
