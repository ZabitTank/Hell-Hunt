using UnityEngine;
using UnityEngine.UI;

public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField]
    AIBeheviour attackBehaviour, patrolBehaviour;

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    AIDetector detector;

    [HideInInspector]
    public BaseWeapon weapon;

    public StaticCharacterStat characterStat;

    public Item[] DropItems;

    [SerializeField]
    RectTransform EnemyUI;
    [SerializeField]
    Slider HPSlider;

    private void Awake()
    {
        attackBehaviour.Parent = this;
        patrolBehaviour.Parent = this;

        detector = GetComponentInChildren<AIDetector>();

        characterController = GetComponentInChildren<CharacterController>();
        characterController.setParent(gameObject);

        weapon = GetComponentInChildren<BaseWeapon>();
        weapon.characterController = characterController;
        HPSlider.value = characterStat.HP.BaseValue;
        HPSlider.maxValue = characterStat.Attributes[3].value.BaseValue;
    }

    private void Start()
    {
        weapon.ChangeWeapon(characterStat.playerDefaultWeapon);
        characterStat.RegisterHPEvent(() =>
        {
            HPSlider.value = characterStat.HP.BaseValue;
            if (characterStat.HP.BaseValue <= 0)
            {
                Destroy(EnemyUI.gameObject);
                Destroy(gameObject);
            }
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void Update()
    {
        if(detector.TargetVisible)
        {
            attackBehaviour.PerformAction(characterController, detector);
        }
        else
        {
            patrolBehaviour.PerformAction(characterController, detector);
        }
        EnemyUI.position = gameObject.transform.localPosition;
    }

    public void TakeDamage(int damage)
    {
        characterStat.HP.UpdateBaseValue(damage);
        Instantiate(GlobalVariable.Instance.bloodEffectPrefab,transform.position,transform.rotation,null);
    }
}
