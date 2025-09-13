using UnityEngine;

[CreateAssetMenu(fileName = "RollingState", menuName = "StateMachine/State/RollingState")]
public class RollingState : State
{
    public override void OnEnter(FiniteStateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        Debug.Log("EnterRolling");
        FSMPlayerSite fsm = stateMachine as FSMPlayerSite;

        fsm.ownerSite.RerollAllDice();
    }
    public override void Tick(FiniteStateMachine stateMachine)
    {
        base.Tick(stateMachine);
        FSMPlayerSite fsm = stateMachine as FSMPlayerSite;

        Debug.Log("chơi chơi");
        if (fsm.ownerSite.isRollingFinished)
        {
            stateMachine.ChangeNewState(fsm.combat);
        }
        else
        {
            stateMachine.ChangeNewState(fsm.rolling);
        }
    }
}
