using System.Collections;
public class FinishSkill : SkillAction
{
    public FinishSkill():
        base()
    {
    }

    //动画播放的时间
    public float normaledTime = 1.0f;

    private Task _waitingAnimatorStop = null;

    public override void Execute()
    {
        if (_waitingAnimatorStop != null)
        {
            _waitingAnimatorStop.Stop();
        }
        _waitingAnimatorStop = new Task(WaitingAnimatorStop());
    }

    private IEnumerator WaitingAnimatorStop()
    {
        while (true)
        {
            if (_skill.centerController.statusController.GetNormalizedTime() >= normaledTime)
            {
                EndSkill();
                break;
            }
            yield return 0;
        }
        yield return 0;
    }

    private void EndSkill()
    {
        if (_waitingAnimatorStop != null)
        {
            _waitingAnimatorStop.Stop();
            _waitingAnimatorStop = null;
        }
        _skill.Finish();
        //放完技能自动回到idle
        _skill.centerController.statusController.Play(Status.Idle);
    }

    public override void Reset()
    {
        if (_waitingAnimatorStop != null)
        {
            _waitingAnimatorStop.Stop();
            _waitingAnimatorStop = null;
        }
    }
}
