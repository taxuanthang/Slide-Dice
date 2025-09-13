using UnityEngine;

public interface IState
{
    void OnEnter(FiniteStateMachine stateMachine);
    void OnExit(FiniteStateMachine stateMachine);
    void Tick(FiniteStateMachine stateMachine); // logic update mỗi frame
}
