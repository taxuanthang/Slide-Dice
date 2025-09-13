using UnityEngine;

[CreateAssetMenu(menuName = "Character")]
public class Character : ScriptableObject
{
    [Header("CharacterInfo")]
    public string nameCharacter;
    public int health;
    public Sprite avatar;
    public Skill[] skills;

    [HideInInspector] public Skill choosingSkill;

    public void DoAction()
    {
        choosingSkill.DoAction();
    }



}
