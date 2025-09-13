using UnityEngine;

public abstract class Skill : ScriptableObject
{
    protected CharacterSlot targetSlot;

    protected int pip;

    public Texture imageTextureOfThisSKill;

    public abstract void DoAction();
}
