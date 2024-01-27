using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOMutation : SO_Item
{
    [SerializeField] public EnumsAction Skill { get; set; }
    [SerializeField] public Color Color { get; set; }
}
