using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public class BattleManager : MonoBehaviour
{
    // khi bđ game, battle setup sẽ nhận thông tin về số người chơi, thông tin từng người chơi, số kẻ địch, và thông tin từng kẻ địch để setup, tạo hàng đợi site vào trong init của TurnManager luôn

    // Bắt đầu game thì sẽ trigger TurnManager bắt đầu màn đánh nhau
    [Header("Sites")]
    [SerializeField] Site playerSite;
    [SerializeField] Site enemySite;

    [Header("Characters' List")]
    public List<Character> characters_Player;
    public List<Character> characters_Enemy;

    public BattleState battleState;

    [SerializeField] private Camera mainCamera;

    public float holdThreshold = 0.5f; // thời gian tối thiểu để tính là hold
    bool isPointerDown = false;
    float holdTime = 0f;

    IClickable clickObject;
    private void Update()
    {

        // sẽ có một tổ chức đen tối nào đó sẽ truyền vào playerSite đầu game, và cập nhập enemySite đầu game
        MouseInputOrTouchInputHandler();
    }

    private void MouseInputOrTouchInputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPointerDown = true;
            holdTime = 0f;
            clickObject = null;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo,float.PositiveInfinity))
            {
                clickObject = hitInfo.collider.GetComponentInParent<IClickable>();
            }
        }

        if (!isPointerDown || clickObject == null)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            holdTime += Time.deltaTime;

            if(holdTime >= holdThreshold)
            {
                isPointerDown =false;
                clickObject.OnHold();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isPointerDown = false;
            if (holdTime >= holdThreshold)
            {
                return;
            }
            clickObject.OnClick();
        }
    }

    private void Start()
    {
        // khi bđ scene hay khi chuyển từ scene mainMenu sang sẽ thực hiện các hành động khởi tạo 

        TurnManager.instance.Init(new List<Site>() { enemySite, playerSite } ); // Gán hai site vào hàng đợi

        TriggerSitesToCreateSlots(); // yêu cầu site Player tạo nhân vật
                                        // yêu cầu site Enemy tạo nhân vật quái cho màn đầu tiên, các màn sau sẽ được site quá tạo lại

    }

    private void StartGame()
    {
        // có thể kiểm tra cái gì đó idk
        TurnManager.instance.StartBattle();  // bắt đầu trận đấu
    }

    private void TriggerSitesToCreateSlots()
    {
        playerSite.CreateSlots(characters_Player);
    }
}
