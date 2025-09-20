using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum OBJAttribute
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    Orange
}

[System.Serializable]
public class OBJAttributePair
{
    public OBJAttribute attribute;
    public Material material;
}

public class OBJAttributeHolder : MonoBehaviour
{
    [SerializeField] private List<OBJAttributePair> attributePairs;

    private Dictionary<OBJAttribute, Material> attributeDictionary;

    private void Awake()
    {
        attributeDictionary = attributePairs.ToDictionary(pair => pair.attribute, pair => pair.material);
    }

    public Material GetMaterial(OBJAttribute attribute)
    {
        attributeDictionary.TryGetValue(attribute, out var material);
        return material;
    }
}