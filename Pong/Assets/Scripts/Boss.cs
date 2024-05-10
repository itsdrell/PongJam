using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Boss : MonoBehaviour
{
    [System.Serializable]
    public struct BossPhaseData
    {
        public float HealthPercentToTrigger;
        public SplineContainer SplineToFollow;
        public float DurationToFinishSpline;
        public float AttackCooldown;
        public GameObject BallToShoot;
    }

    public float TransitionDuration = 3.0f;
    public GameObject GameManagerObject;
    public GameObject StageClearObject;
    public Transform OriginalSpawnLocation;

    // this could be cool as an array of possible locations and maybe inside of the phase data? Out of scope I think
    public GameObject ShootLocation;
    
    public List<BossPhaseData> BossPhases;

    private int CurrentPhase = 0;
    private BossPhaseData CurrentPhaseData;
    private float AttackCooldownTimer = 1.0f;
    private float TransitionTimer = 0.0f;
    private bool TransitionJustEnded = true;


    // Start is called before the first frame update
    void Start()
    {
        TheGameManager gm = GameManagerObject.GetComponent<TheGameManager>();
        gm.OnEnemyHealthChange += OnBossDamanged;

        //OriginalSpawnLocation = this.transform;

        // could error check this but would be better as an editor error for design
        // "if array empty, you probs want to fill that out designer"
        CurrentPhaseData = BossPhases[CurrentPhase];

        SetSpline();

        // todo check if our public required variables are set. I swore there was a [required] but unity documentations says no??
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
            Vector2 direction = new Vector2(-1, Random.Range(-1.0f, 1.0f));
            direction.Normalize();
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

    // Spawn the Deleter, Reset Attack Timer, Teleport to original location, Set the Spline
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
