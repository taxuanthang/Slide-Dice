using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "StateMachine/State/IdleState")]
public class IdleState : State
{
    public override void OnEnter(FiniteStateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        Debug.Log("EnterIdle");
    }
    public override void Tick(FiniteStateMachine stateMachine)
    {
        base.Tick(stateMachine);
        FSMPlayerSite fsm = stateMachine as FSMPlayerSite;
        //Debug.Log("Idle");
        if (fsm.ownerSite.isTurnStarted)
        {
            stateMachine.ChangeNewState(fsm.rolling);
        }
        else
        {
            stateMachine.ChangeNewState(fsm.idle);
        }

    }
}
