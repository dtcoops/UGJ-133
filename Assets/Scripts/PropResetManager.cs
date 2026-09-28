using UnityEngine;
using System.Collections.Generic;

public class PropResetManager : MonoBehaviour
{
    public static PropResetManager Instance { get; private set; }

    readonly List<BreakableProp> props = new List<BreakableProp>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(BreakableProp prop)
    {
        props.Add(prop);
    }

    public void ResetAllProps()
    {
        foreach (BreakableProp prop in props)
        {
            prop.ResetProp();
        }
    }
}