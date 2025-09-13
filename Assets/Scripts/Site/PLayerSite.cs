using UnityEngine;

public class PlayerSite : Site
{
    // khi bắt đầu turn, tất cả các slot sẽ unlock dice và roll 1 lần

    // ở trong state này, người chơi có thể ấn vào slot để lock dice hoặc
    //          ấn giữ slot để view thông tin nv cũng như kẻ thù nào đang nhắm đến
    //          ấn vào slot kẻ địch để để view đang target ai và skill gì, nội tại ntn
    //          ấn giữ vào kẻ địch để view ttin kẻ địch(tên, máu, icon, xúc sắc, các mặt xúc sắc) cũng xem được đang target ai
    //          có thể reroll để đổi xúc sắc, hoặc done rolling đẻ chuyển sang trạng thái đánh nhau
    //          ấn vào lượng mana để xem số lượng mana, ấn vào skill mana để xem ttin của skill đấy


    // ở trong state này,  người chơi có thể ấn vào slot để chọn skill, xong chọn target để áp dụng skill
    //          ấn giữ slot để view thông tin nv cũng như kẻ thù nào đang nhắm đến
    //          ấn vào slot kẻ địch để để view đang target ai và skill gì, nội tại ntn
    //          ấn giữ vào kẻ địch để view ttin kẻ địch(tên, máu, icon, xúc sắc, các mặt xúc sắc) cũng xem được đang target ai
    //          Ấn undo hành động trước đó, khi ko còn hành động thì sẽ quay về state quay súc xắc
    //          Ấn endturn để end cái turn đó

    // khi end turn thì sẽ áp dụng các hiệu ứng độc
    //

    [SerializeField] FSMPlayerSite fsm;

    public override void Start()
    {
        base.Start();
        // Clone SO fsm để không bị chung với các nơi khác dùng fsm này
        fsm = Instantiate(fsm);

        fsm.ownerSite = this;
        // khởi tạo lại các SO State để không bị chung
        fsm.Init();
    }

    public override void Update()
    {
        base.Update();
        // tick các state trong fsm
        fsm.Tick();
    }

    public override void DoActionAtBeginOfTurn()
    {
        base.DoActionAtBeginOfTurn();

    }

}
