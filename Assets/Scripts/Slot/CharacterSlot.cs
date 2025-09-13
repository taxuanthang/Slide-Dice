using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSlot : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    [SerializeField] Character character;

    // nguoi choi co the xem duoc thong tin nhan vat cua nguoi choi( ten, skill, mau sac, health),

    // trong Character Slot, nguoi choi co the an vao khi dang trong che do roll de lock va unlock dice,
    // trong che do DoneRolling ng choi co the an vao de thuc hien action xong se chon target de thuc hien action do,
    // moi khi xong mot man thi co tang cap nhan vat,
    // co the an giu de xem thong tin nhan vat va xem co nhung ke thu nao dang target

    // co the set character ban dau, hoac khi thang cap

    [Header("ComponentRefs")]
    [SerializeField] TextMeshProUGUI nameCharacter_UI;
    [SerializeField] Image avatar_UI;
    [SerializeField] Image skill_UI;
    [SerializeField] GameObject healthArea_UI;

    [Header("Sprite")]
    [SerializeField] GameObject heart_UI;

    [Header("Dice Prefabs of This slot")]
    [SerializeField] GameObject dicePrefab;

    [Header("Dice of This slot")]
    [SerializeField] DiceManager dice;

    [Header("Hold checking")]
    public float holdThreshold = 0.5f; // thời gian tối thiểu để tính là hold
    private bool isPointerDown = false;
    private float holdTime = 0f;

    public event System.Action OnHold;
    public event System.Action OnClick;


    [HideInInspector] public Vector3 realWorldPos;

    bool isLock;

    Skill choosingSkill;

    void Update()
    {
        HoldHandler();
    }

    public void SetUp(Character character)
    {
        // gán các nhân vật, thông số của nhân vật vào UI, skill của nhân vật vào dice, tạo dice
        this.character = character;
        nameCharacter_UI.text = character.nameCharacter;
        avatar_UI.sprite = character.avatar;

        dice = Instantiate(dicePrefab, realWorldPos , Quaternion.identity).GetComponent<DiceManager>();
        dice.characterSlot = this;

        // Set up health
        for (int i = 0; i < character.health; i++)
        {
            Instantiate(heart_UI, healthArea_UI.transform);
        }

        // gán skill cho dice của player 
        dice.SetMeshRendersAndSkill(character.skills);

        // gán vị trí characterSlot ở world cho súc xắc
        dice.SetCharacterSlotRealWorldPos(realWorldPos);

        LockDice();
    }

    #region các việc liên quan đến dice
    //Pre- DoneRolling
    public void LockDice()
    {
        // nếu đã lock rồi thì thôi
        if(isLock)
        {
            return;
        }
        isLock = true;

        StartCoroutine(dice.MoveToCharacterSlotPosition());

        // chọn skill hiện tại là mặt trên của xúc sắc

        choosingSkill = dice.GetTopFace();
        skill_UI.sprite = HelperFunctionAndClass.SpriteFromTexture(choosingSkill.imageTextureOfThisSKill as Texture2D);

    }

    public void UnlockDice()
    {
        // Nếu chưa lock thì thôi
        if (!isLock)
        {
            return;
        }
        isLock = false;

        StartCoroutine(dice.MoveToPrevioisPosition());
    }

    public void RerollDice()
    {
        if (isLock)
        {
            return; // nếu đã khóa dice rồi thì ko roll
        }
        StartCoroutine(dice.RollDice());
    }

    public void TriggerLockDice()
    {
        if (isLock)
        {
            UnlockDice();
        }
        else
        {
            LockDice();
        }
    }
    #endregion

    #region Kiểm tra người chơi đang ấn hay giữ characterSLot
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        holdTime = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (holdTime < holdThreshold)
        {
            OnClick?.Invoke(); // click ngắn

            // pre-DoneRolling và pót-DoneRolling sẽ có hai tác dụng khác nhau khi click
            TriggerLockDice();

            //OnClick = null;
            //print("1");
        }
        isPointerDown = false;
    }

    private void HoldHandler()
    {
        if (!isPointerDown)
        {
            return;
        }
        holdTime += Time.deltaTime;
        if (holdTime >= holdThreshold)
        {
            OnHold?.Invoke(); // gọi khi đủ thời gian giữ
            isPointerDown = false; // chỉ gọi 1 lần

            // Show Info của người chơi dù ở Done Rolling hay trước thì hold đều giữ nguyên tác dụng
            Debug.Log("Show Info của nhân vật"+ character.nameCharacter);
        }
    }
    #endregion

    public void TriggerCharacterAction()
    {
        character.choosingSkill = choosingSkill;
        character.DoAction();
    }

    public void ProcessEffect()
    {

    }

}
