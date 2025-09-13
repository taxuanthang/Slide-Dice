using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceManager : MonoBehaviour , IClickable
{
    Rigidbody rb;
    BoxCollider boxCollider;
    [SerializeField] float force;


    [Header("6 quads dán trên 6 mặt xúc xắc")]
    [SerializeField] Renderer[] meshRendererOfEachFace = new Renderer[6];

    [Header("Material gốc dùng chung cho tất cả quads")]
    public Material baseMaterial;


    [Tooltip("Standard/Unlit dùng _MainTex, URP/Lit dùng _BaseMap")]
    public string textureProperty = "_MainTex";

    [Header("SlotDice thuộc về")]
    public CharacterSlot characterSlot;

    [Header("Kỹ năng của dice ")]
    public Skill[] skillsOfDice = new Skill[6];

    [Header("Các ngưỡng của dice ")]
    public float stopThreshold = 0.02f; // ngưỡng dừng
    public float moveDuration = 1f; // thời gian di chuyển từ bàn đến chỗ lock


    [Header("MaterialPropertyBlock để tránh việc tạo quá nhiều Material mỗi lần sử dụng cho từng mặt của xúc sắc hay của mỗi skill")]
    // Reuse 1 MPB để tránh alloc mỗi lần
    private MaterialPropertyBlock mpb;

    Vector3 previousLockWorldPos;

    Vector3 afterLockWorldPos;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponentInChildren<BoxCollider>();

        mpb = new MaterialPropertyBlock();

        // Gán material CHUNG 1 lần bằng sharedMaterial (không clone)
        foreach (var renderer in meshRendererOfEachFace)
        {
            if (renderer) 
            { 
                renderer.sharedMaterial = baseMaterial; 
            }

        }

        previousLockWorldPos = new Vector3(Random.Range(-13f, 13f), -16.42f, Random.Range(9f, -3f));

    }


    // Update is called once per frame
    void Update()
    {

        //if (HasStopped())
        //{
        //    topFace = GetTopFace();
        //    Debug.Log("Dice result: " + topFace);
        //} // Debug
    }

    public void SetCharacterSlotRealWorldPos(Vector3 pos)
    {
        afterLockWorldPos = pos;
    }
    

    #region Set Texture cho súc xắc
    public void SetMeshRendersAndSkill(Skill[] targetSkills)
    {
        skillsOfDice = targetSkills;


        for (int i = 0; i < meshRendererOfEachFace.Length; i++)
        {
            SetFaceTexture(i, skillsOfDice[i].imageTextureOfThisSKill);          // gán texture cho từng mặt quads của xúc sắc
        }

    }

    public void SetFaceTexture(int faceIndex, Texture tex)
    {
        if (faceIndex < 0 || faceIndex >= meshRendererOfEachFace.Length)
        {
            return;
        }
        var renderer = meshRendererOfEachFace[faceIndex];
        if (!renderer || tex == null)
        {
            return;
        }
        //print("da dat");
        // Lấy block hiện có (nếu ai đó đã set trước đó)
        renderer.GetPropertyBlock(mpb);

        // Ghi đè texture cho renderer này
        mpb.SetTexture(textureProperty, tex);

        // Áp lại block vào renderer
        renderer.SetPropertyBlock(mpb);


        //mpb.SetColor("_BaseColor", highlightColor); // URP/Lit
        //                                             // hoặc _mpb.SetColor("_Color", ...) với Standard/Unlit có _Color
    }
    #endregion

    #region Hỗ trợ việc roll súc xắc

    public IEnumerator RollDice()
    {
        if (!HasStopped())
        {
            yield return null;
        }
        rb.AddForce(new Vector3(Random.Range(-2, 2), 1, Random.Range(-2, 2)) * force / 2, ForceMode.Impulse); //Tung lên và xoay theo một góc bất kì của một điểm bất kì
        rb.AddTorque(Random.insideUnitSphere * force, ForceMode.Impulse);
        yield return new WaitForSeconds(1);

        if (HasStopped())
        {
            previousLockWorldPos = this.transform.position; // khả năng sai
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            previousLockWorldPos = this.transform.position; // khả năng sai
        }

    }

    bool HasStopped()
    {
        // kiểm tra vận tốc tuyến tính và góc
        return rb.linearVelocity.magnitude < stopThreshold &&
               rb.angularVelocity.magnitude < stopThreshold;
    }

    public Skill GetTopFace()
    {
        Vector3 up = Vector3.up;

        Vector3[] directions = {
            transform.up,        // mặt trên
            -transform.up,       // mặt dưới
            transform.right,     // mặt phải
            -transform.right,    // mặt trái
            transform.forward,   // mặt trước
            -transform.forward   // mặt sau
        };

        float maxDot = -Mathf.Infinity;
        int faceIndex = -1;

        for (int i = 0; i < directions.Length; i++)
        {
            float dot = Vector3.Dot(directions[i], up);
            if (dot > maxDot)
            {
                maxDot = dot;
                faceIndex = i;
            }
        }

        return skillsOfDice[FaceIndexToNumber(faceIndex)];
    }

    int FaceIndexToNumber(int index)
    {           // sau đổi thành FaceIndexToSkill sẽ trả về kiểu skill
        switch (index)
        {
            case 0: return 1; // up
            case 1: return 6; // -up
            case 2: return 4; // right
            case 3: return 3; // -right
            case 4: return 2; // forward
            case 5: return 5; // -forward
        }
        return 0;
    }
    #endregion

    #region lock and unlock xúc sắc
    public IEnumerator MoveToCharacterSlotPosition()
    {

        // untick using gravity and disable box collider 
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;


        rb.useGravity = false;
        boxCollider.enabled = false;

        //DotTween tới 1 vị trí
        transform.DOMove(afterLockWorldPos, moveDuration).SetEase(Ease.Linear);
        transform.DOLocalRotate(Vector3.zero, moveDuration).SetEase(Ease.Linear);
        yield return null;

    }

    public IEnumerator MoveToPrevioisPosition()
    {
        // nếu vị trí trước khi lock trùng với vị trí hiện tại thì sẽ random vị trí bất kì
        rb.angularVelocity = Vector3.zero;
        rb.linearVelocity = Vector3.zero;

        //DotTween tới 1 vị trí
        transform.DOMove(previousLockWorldPos, moveDuration).SetEase(Ease.Linear);
        //transform.DOLocalRotate(Vector3.zero,moveDuration).SetEase(Ease.Linear);

        // wait for transformation and tick using gravity and disable box collider 
        yield return new WaitForSeconds(moveDuration);
        // if on the fly way so skip
        if (this.transform.position == previousLockWorldPos)
        {
            rb.useGravity = true;
            boxCollider.enabled = true;
        }

    }
    #endregion

    public void OnClick()
    {
        characterSlot.LockDice();
    }

    public void OnHold()
    {
        print("Sau này sẽ show thông tin của dice");
    }


}
