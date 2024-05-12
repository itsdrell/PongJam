using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Boss : MonoBehaviour
{
    [Header("How long should the boss disappear between phase resets")]
    public float TransitionDuration = 3.0f;

    [Header("Reference to the phase clearer object")]
    public GameObject StageClearObject;

    // this could be cool as an array of possible locations and maybe inside of the phase data? Out of scope I think
    [Header("The location where the balls will shoot from. Typically a empty gameobject on the boss prefab")]
    public GameObject ShootLocation;

    [Header("Boss Phases")]
    public List<BossPhaseData> BossPhases;

    // Local Vars
    private int CurrentPhase = 0;
    private BossPhaseData CurrentPhaseData;
    
    private float AttackCooldownTimer = 1.0f;
    private float TransitionTimer = 0.0f;
    private bool TransitionJustEnded = true;

    // Both of these should be read only OR made private and then we make public setters
    private GameObject GameManagerObject;
    private Transform OriginalSpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        TheGameManager gm = GameManagerObject.GetComponent<TheGameManager>();
        gm.OnEnemyHealthChange += OnBossDamanged;

        // could error check that you actually have boss phases but would be better as an editor error for design
        // "if array empty, you probs want to fill that out designer"
        CurrentPhaseData = BossPhases[CurrentPhase];

        SetSpline();
    }

    public void SetReferences(GameObject theGameManager, Transform originalSpawnLocation)
    {
        GameManagerObject = theGameManager;
        OriginalSpawnLocation = originalSpawnLocation;
    }

    // Update is called once per frame
    void Update()
    {
        if(TransitionTimer <= 0.0f)
        {
            EndTransition();
            AttackUpdate();
        }
        else
        {
            TransitionTimer -= Time.deltaTime;
        }
    }

    void AttackUpdate()
    {
        if(AttackCooldownTimer <= 0.0f)
        {
            float speed = CurrentPhaseData.BallToShoot.GetComponent<Ball>().InitialSpeed;
            Vector2 direction = new Vector2(-1, Random.Range(-1.0f, 1.0f)).normalized;
            Vector2 velocity = direction * speed;
            Shoot(CurrentPhaseData.BallToShoot, velocity);

            AttackCooldownTimer = CurrentPhaseData.AttackCooldown;
        }
        else
        {
            AttackCooldownTimer -= Time.deltaTime;
        }
    }

    void Shoot(GameObject objectToShoot, Vector2 velocity)
    {
        GameObject objectShot = Instantiate(objectToShoot, ShootLocation.transform.position, ShootLocation.transform.rotation);
        objectShot.GetComponent<Rigidbody2D>().AddForce(velocity);
    }

    void OnBossDamanged(float newHealth)
    {
        int nextPhase = CurrentPhase + 1;
        if (nextPhase >= BossPhases.Count)
        {
            return;
        }

        BossPhaseData nextPhaseData = BossPhases[nextPhase];
        if (newHealth <= nextPhaseData.HealthPercentToTrigger)
        {
            CurrentPhaseData = nextPhaseData;
            CurrentPhase = nextPhase;

            TransitionTimer = TransitionDuration;

            OnTransition();
        }
    }

    void OnTransition()
    {
        AttackCooldownTimer = CurrentPhaseData.AttackCooldown;
        this.transform.position = OriginalSpawnLocation.position;
        
        // dont move or show
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<SplineAnimate>().Pause();

        Instantiate(StageClearObject, this.OriginalSpawnLocation.position, this.OriginalSpawnLocation.rotation);

        TransitionJustEnded = false;
    }

    void EndTransition()
    {
        if(TransitionJustEnded)
        {
            return;
        }
        
        SetSpline();

        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        TransitionJustEnded = true;
    }

    void SetSpline()
    {
        SplineAnimate splineComp = this.gameObject.GetComponent<SplineAnimate>();
        splineComp.Container = CurrentPhaseData.SplineToFollow;
        splineComp.Duration = CurrentPhaseData.DurationToFinishSpline;
        splineComp.Restart(true);
        splineComp.Play();
    }
}
