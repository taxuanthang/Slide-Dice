using UnityEngine;

[CreateAssetMenu(fileName = "Base", menuName = "StateMachine/FSM/Base")]
public class FiniteStateMachine : ScriptableObject
{
    [Header("Current State")]
    protected IState currentState;

    // khởi tạo các biến reference đến các state
    
    public virtual void Init()
    {
        //Instantiate các state để ko dùng lại
    }

    public virtual void StartFSM()
    {

    }

    public virtual void OnDestroy()
    {
        // Xóa hết các instance của các state
    }

    public void ChangeNewState(IState newState)
    {
        if(newState == currentState)
        {
            currentState = newState;
            return;
        }
        currentState.OnExit(this);
        currentState = newState;
        currentState.OnEnter(this);

    }

    public void Tick()
    {
        currentState.Tick(this);
    }

}
