public class AnimaEvent: SkillEvent
{
    //动画事件，动画执行到指定的时间再执行
    public AnimaEvent()
        : base()
    {
    }

    public float normalTime = 0.0f; //触发的时间
}