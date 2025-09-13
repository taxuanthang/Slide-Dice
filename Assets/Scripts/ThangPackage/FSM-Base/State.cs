using UnityEngine;

[CreateAssetMenu(fileName = "Base", menuName = "StateMachine/State/Base")]
public class State : ScriptableObject, IState
{

    public virtual void OnEnter(FiniteStateMachine stateMachine)
    {

    }
    public virtual void OnExit(FiniteStateMachine stateMachine)
    {

    }
    public virtual void Tick(FiniteStateMachine stateMachine)
    {
        // mỗi khi gặp điều kiện chuyển state phù hợp sẽ dùng hàm ChangeScene trong stateMachine kia rồi ref tới state trong stateMachine kia 
    }
}
