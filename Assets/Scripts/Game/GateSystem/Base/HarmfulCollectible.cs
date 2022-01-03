using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmfulCollectible : MonoBehaviour
{
    public int value;

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
