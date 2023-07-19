using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NPCData : ScriptableObject
{
    [TextArea(2, 5)]
    public List<string> Sentences;
    public string NpcName;
}
