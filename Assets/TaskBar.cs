using UnityEngine;

public class TaskBar : MonoBehaviour
{
    // Quản lý các skill và các chức năng ở thanh bên dưới

    public void CreateSkill()
    {

    }

    // Pre-DoneRolling
    public void RollAllPlayerSiteDice()
    {
        TurnManager.instance.RequestThisTurnSiteToRoll();
    }

    public void DoneRollingPlayerSiteDice()
    {
        TurnManager.instance.RequestThisTurnSiteToDoneRolling();
    }

    // Post-DoneRolling
    public void ReturnToPreDoneRollingPhase()
    {
        
    }

    public void UndoThisSiteAction()
    {
        // nếu chưa thực hiện hành động gì thì sẽ quay về thời điểm trước khi done Rolling
        // nếu đã thực hiện 1 hành động thì sẽ quay về hành động đó
    }

    public void EndTurn()
    {
        TurnManager.instance.EndThisSiteTurn();
    }
}
