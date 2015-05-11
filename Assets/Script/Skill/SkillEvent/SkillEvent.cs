using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillEvent : ScriptableObject
{
    public SkillEvent()
        :base()
    {
    }

    public bool isTrigger = false; //是否已经被触发了    
    public List<SkillAction> skillActions = new List<SkillAction>(); //action列表
    protected Skill _skill; //拥有该事件的技能
    public Skill OwerSkill
    {
        set { _skill = value; }
    }

    //执行
    public virtual void Execute()
    {
        isTrigger = true;
        for (int i = 0; i < skillActions.Count; ++i)
        {
            skillActions[i].OwerSkill = _skill;
            skillActions[i].Execute();
        }
    }

    //Update由skill调用
    public virtual void Update()
    {
        for (int i = 0; i < skillActions.Count; ++i)
        {
            if (skillActions[i].isDurative)
            {
                skillActions[i].Update();
            }
        }
    }

    //FixedUpdate由skill调用
    public virtual void FixedUpdate()
    {
        for (int i = 0; i < skillActions.Count; ++i)
        {
            if (skillActions[i].isDurative)
            {
                skillActions[i].FixedUpdate();
            }
        }
    }

    //完成，把所有的技能action都执行完毕
    public virtual void Finish()
    {
        for (int i = 0; i < skillActions.Count; ++i)
        {
            skillActions[i].Finish();
        }
    }

    //重置数据
    public virtual void Reset()
    {
        for (int i = 0; i < skillActions.Count; ++i)
        {
            skillActions[i].Reset();
        }
    }
}
