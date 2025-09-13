using UnityEngine;

[CreateAssetMenu(fileName = "FSMEnemySite", menuName = "StateMachine/FSM/FSMEnemySite")]
public class FSMEnemySite : FiniteStateMachine
{
    [Header("States")]
    public CombatState combat;
    public RollingState rolling;
    public IdleState idle;

    [Header("Owner of this FSM")]
    public PlayerSite ownerSite;
    public override void Init()
    {
        // nhân bản SO để ko bị dùng chung với FSM khác
        this.combat = Instantiate(combat);
        this.rolling = Instantiate(rolling);
        this.idle = Instantiate(idle);

        // khởi tạo current state
        currentState = idle;

    }

    public override void StartFSM()
    {

    }

    public override void OnDestroy()
    {
        Destroy(combat);
        Destroy(rolling);
        Destroy(idle);
    }
}
