using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Site : MonoBehaviour
{
    // nhận thông tin về số player trong site của mình, và thông tin về từng player của mình, khởi tạo các playerSlot tương ứng

    List<CharacterSlot> slots = new List<CharacterSlot>();
    List<Character> characters = new List<Character>();

    [Header("Prefabs")]
    public GameObject characterSlotPrefabs;

    bool IsAllCharactersDead; // intend là nếu một lúc nào đó tất cả nhân vật trong site chết thì sẽ perform hành động luôn

    [Header("ToArrangeThePositionOfDice")]
    public Vector3 firstPositionOfDice;
    float rangeOfContainer = 14f;

    public bool isTurnStarted;
    public bool isRollingFinished;
    public bool isCombatFinished;
    // State: Pre-DoneRolling,
    // Post-DoneRolling, 
    // ProccessEffect
    // CheckVictory
    // 

    
    public virtual void Start()
    {

    }

    public virtual void Update()
    {

    }

    public void CreateSlots(List<Character> charactersToCreate)
    {
        if (characters.Count >= 1)
        {
            Debug.LogError("Site" + this.name + "Đang có sẵn character");
            return;// sau này sửa thành hành động khác
        }
        characters = charactersToCreate;

        float rangeBetweenDice = rangeOfContainer / charactersToCreate.Count;

        for (int i = 0; i < characters.Count; i++)
        {
            //Tạo các slot player trong ô site
            CharacterSlot slot = Instantiate(characterSlotPrefabs, this.gameObject.transform).GetComponent<CharacterSlot>();

            // gán realworldpos cho từng slot
            slot.realWorldPos = firstPositionOfDice - new Vector3(0,0,(rangeBetweenDice * i));


            // add slot to list
            slots.Add(slot);

            //yêu cầu các slot khởi tạo nhân vật, gán thanh máu...
            slots[i].SetUp(characters[i]);
        }
    }

    public void ClearSlots()
    {

    }

    public void RemoveSLot(int idx)
    {

    }

    public void AddSLot(int idx)
    {

    }

    public virtual void DoActionAtBeginOfTurn()
    {
        // roll all dice of any slot is unlock
        //RerollAllDice();
    }

    // pre-DoneRolling
    public void RerollAllDice()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].RerollDice();
        }
    }

    public void LockAllDice()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].LockDice();
        }
    }

    //Post-DoneRolling
}
