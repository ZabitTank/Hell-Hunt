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
}
