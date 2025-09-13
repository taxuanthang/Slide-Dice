using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Queue<Site> siteTurnQueue = new Queue<Site>();
    [SerializeField] Site currentSiteTurn;

    public static TurnManager instance;

    


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError($"Xoa {this.name} vì đã có Singleton tồn tại trong scene ");
            Destroy(gameObject);
        }

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StartBattle();
        }
    }

    public void Init(List<Site> sites)
    {
        siteTurnQueue.Clear();
        for (int i = 0; i < sites.Count; i++) {
            siteTurnQueue.Enqueue(sites[i]);
        }
    }

    public void StartBattle()
    {
        Debug.Log("Battle Start!");
        NextTurn();
    }

    private void NextTurn()
    {
        // Nếu hết nhân vật trong queue thì reset vòng mới
        if (siteTurnQueue.Count == 0)
        {
            Debug.Log("Round finished → restarting queue.");
            return;
        }

        currentSiteTurn = siteTurnQueue.Dequeue();

        currentSiteTurn.isTurnStarted = true;

        currentSiteTurn.DoActionAtBeginOfTurn(); // yêu cầu các site sẽ làm các hành động có thể làm đầu turn         // Reroll tat ca suc sac cua tung site?



        Debug.Log($"== {currentSiteTurn.name}'s Turn ==");
        // Ở đây bạn có thể show UI: "It's Hero's turn"
        // hoặc gọi logic chọn hành động tự động (nếu AI).
    }

    //public void PerformAttack(Character attacker, Character target, int dmg)
    //{
    //    if (!attacker.IsAlive || !target.IsAlive) return;

    //    Debug.Log($"{attacker.Name} attacks {target.Name}!");
    //    GameEvents.RaiseOnAttack(attacker, target);

    //    target.TakeDamage(dmg);

    //    GameEvents.RaiseOnAttacked(target);
    //}

    public void EndThisSiteTurn()
    {
        // kiểm tra các nhân vật trong site đã thực hiện hết hành động của mình chưa ko thì nhắc nhở

        // yêu cầu các site thực hiện các hành động sẽ làm cuối turn

        if (currentSiteTurn != null)
        {
            //GameEvents.RaiseOnTurnEnd(currentSiteTurn);
            // Nhân vật sống thì cho vào cuối hàng đợi
            siteTurnQueue.Enqueue(currentSiteTurn);
        }

        NextTurn();
    }

    public bool IsMySiteTurn(Site site)
    {
        return site == currentSiteTurn;
    }

    public void RequestThisTurnSiteToRoll()
    {
        currentSiteTurn.RerollAllDice();
    }

    public void RequestThisTurnSiteToDoneRolling()
    {
        currentSiteTurn.LockAllDice();
    }
}
