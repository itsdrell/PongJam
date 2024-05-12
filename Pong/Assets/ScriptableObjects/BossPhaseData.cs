using UnityEngine;
using UnityEngine.Splines;

[CreateAssetMenu(menuName = "Data/BossPhaseData")]
public class BossPhaseData : ScriptableObject
{
    public float HealthPercentToTrigger;
    public SplineContainer SplineToFollow;
    public float DurationToFinishSpline;
    public float AttackCooldown;
    public GameObject BallToShoot; // we didn't utilize this but future thinking
}
