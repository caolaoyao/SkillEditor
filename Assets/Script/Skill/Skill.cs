using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill : ScriptableObject
{
    public Skill() :
        base()
    {
    }

    public int skillId; //技能id
    public string desc = ""; //技能描述
    [HideInInspector]
    public CenterController centerController;
    public List<ActiveEvent> activeEvent = new List<ActiveEvent>(); //技能激活事件
    public List<AnimaEvent> animaEvent = new List<AnimaEvent>(); //技能的动画事件

    private bool _isExecute = false; //是否在执行

    //使用技能
    public void Use()
    {
        //每次使用完都要重置一次
        Reset();
        _isExecute = true;
        for (int i = 0; i < activeEvent.Count; ++i)
        {
            activeEvent[i].OwerSkill = this;
            activeEvent[i].Execute();
        }

        for (int i = 0; i < animaEvent.Count; ++i)
        {
            animaEvent[i].OwerSkill = this;
        }
    }

    //更新action的Update逻辑
    public void Update()
    {
        if (_isExecute == false)
        {
            return;
        }

        if (centerController.statusController.IsSameStatus())
        {
            for (int i = 0; i < animaEvent.Count; ++i)
            {
                if(animaEvent[i].isTrigger)
                {
                    continue;
                }
                //动画播放到指定的时间，触发事件
                if (centerController.statusController.GetNormalizedTime() >= animaEvent[i].normalTime)
                {
                    animaEvent[i].Execute();
                }
            }

            for (int i = 0; i < activeEvent.Count; ++i)
            {
                if (activeEvent[i].isTrigger)
                {
                    activeEvent[i].Update();
                }
            }

            for (int i = 0; i < animaEvent.Count; ++i)
            {
                if (animaEvent[i].isTrigger)
                {
                    animaEvent[i].Update();
                }
            }
        }
    }

    //更新action的FixedUpdate逻辑
    public void FixedUpdate()
    {
        if (_isExecute == false)
        {
            return;
        }

        if (centerController.statusController.IsSameStatus())
        {
            for (int i = 0; i < activeEvent.Count; ++i)
            {
                if (activeEvent[i].isTrigger)
                {
                    activeEvent[i].FixedUpdate();
                }
            }

            for (int i = 0; i < animaEvent.Count; ++i)
            {
                if (animaEvent[i].isTrigger)
                {
                    animaEvent[i].FixedUpdate();
                }
            }
        }        
    }

    //正常使用完成
    public void Finish()
    {
        _isExecute = false;
        for (int i = 0; i < activeEvent.Count; ++i)
        {
            activeEvent[i].Finish();
        }

        for (int i = 0; i < animaEvent.Count; ++i)
        {
            animaEvent[i].Finish();
        }
    }

    //重置技能数据
    public void Reset()
    {
        _isExecute = false;

        for (int i = 0; i < activeEvent.Count; ++i)
        {
            activeEvent[i].isTrigger = false;
            activeEvent[i].Reset();
        }

        for (int i = 0; i < animaEvent.Count; ++i)
        {
            animaEvent[i].isTrigger = false;
            animaEvent[i].Reset();
        }
    }

    //被打断
    public void Interrupt()
    {
        Finish();
    }
}
