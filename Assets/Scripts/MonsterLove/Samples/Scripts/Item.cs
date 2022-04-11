using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool isTarget;

    public void OnMouseDown()
    {
        if (Triggered != null) Triggered(this);
    }

    public event Action<Item> Triggered;
}