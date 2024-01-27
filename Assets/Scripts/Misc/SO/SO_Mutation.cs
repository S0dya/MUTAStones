using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu]
public class SO_Mutation : SO_Item
{
    [field : SerializeField] public EnumsActions Skill { get; set; }
    [field : SerializeField] public Color Color { get; set; }   
}
