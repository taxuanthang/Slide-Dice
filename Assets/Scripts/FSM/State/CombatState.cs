using UnityEngine;

[CreateAssetMenu(fileName = "CombatState", menuName = "StateMachine/State/CombatState")]
public class CombatState : State
{
    public override void OnEnter(FiniteStateMachine stateMachine)
    {
        base.OnEnter(stateMachine);
        Debug.Log("EnterCombat");
    }
    public override void Tick(FiniteStateMachine stateMachine)
    {
        base.Tick(stateMachine);
        FSMPlayerSite fsm = stateMachine as FSMPlayerSite ;
        if (fsm.ownerSite.isCombatFinished)
        {
            stateMachine.ChangeNewState(fsm.idle);
        }
        else
        {
            stateMachine.ChangeNewState(fsm.rolling);
        }
    }
}
