using UnityEngine;

public class BaseEnemyAI : MonoBehaviour
{
    [SerializeField]
    AIBeheviour attackBehaviour, patrolBehaviour;

    [SerializeField]
    CharacterController characterController;

    [SerializeField]
    AIDetector detector;

    public StaticCharacterStat characterStat;

    public BaseWeapon weapon;

    public GameObject EnemyBloodPrefabs;
    public Item DropItems;

    private void Awake()
    {
        attackBehaviour.Parent = this;
        patrolBehaviour.Parent = this;

        detector = GetComponentInChildren<AIDetector>();

        characterController = GetComponentInChildren<CharacterController>();
        characterController.setParent(gameObject);

        weapon = GetComponentInChildren<BaseWeapon>();
        weapon.characterController = characterController;
        weapon.ChangeWeapon(characterStat.playerDefaultWeapon);
        
    }

    private void Start()
    {
        characterStat.RegisterHPEvent(() =>
        {
            if(characterStat.HP.BaseValue <= 0)
            {
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
    }

    public void TakeDamage(int damage)
    {
        characterStat.HP.UpdateBaseValue(damage);
        Instantiate(GlobalVariable.Instance.bloodEffectPrefab,transform.position,transform.rotation,null);
    }
}
