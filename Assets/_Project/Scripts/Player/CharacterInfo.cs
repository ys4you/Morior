using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Info", menuName = "Character/Character info")]
public class CharacterInfo : ScriptableObject
{
    public GameObject characterPrefab;
    public Sprite characterUiSprite;

    public string characterName;
    public float health;

    public bool isEnemy;
}
