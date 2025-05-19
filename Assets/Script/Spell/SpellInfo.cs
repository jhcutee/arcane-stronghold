using UnityEngine;
[CreateAssetMenu(fileName = "SpellInfo", menuName = "ScriptableObject/SpellInfo", order = 4)]
public class SpellInfo : ScriptableObject
{
    public Sprite spellSprite;
    public Sprite spellIcon;
    public float spellCooldownTime;
    public AudioClip spellAudioClip;
}
