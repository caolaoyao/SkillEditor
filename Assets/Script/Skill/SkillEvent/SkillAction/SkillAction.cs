using UnityEngine;
using System.Collections;

public class SkillAction : ScriptableObject
{
    public bool isDurative = false; //该action是否是技续性的
    protected Skill _skill; //拥有该action的技能
    public Skill OwerSkill
    {
        set { _skill = value; }
    }

    public virtual void Execute()
    {
    }

    public virtual void Finish()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void Reset()
    {
    }
}
