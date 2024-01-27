using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class SO_Item : ScriptableObject
{
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField][field: TextArea(6, 10)] public string Description { get; set; }
    [field: SerializeField] public Sprite ItemImage { get; set; }
}
