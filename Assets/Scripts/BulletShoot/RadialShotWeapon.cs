using System.Collections;
using UnityEngine;


    public class RadialShotWeapon : MonoBehaviour
    {
        [SerializeField] private RadialShotPattern _shotPattern;
 
    private bool _onShotPattern = false;

        

    public void TryPerformRadialShot()
    {
        Debug.Log("fire");
        if (!_onShotPattern)
        {
            StartCoroutine(ExecuteRadialShotPattern(_shotPattern));
        }
    }

    private IEnumerator ExecuteRadialShotPattern(RadialShotPattern pattern)
        {
            _onShotPattern = true;
            int lap = 0;
            Vector2 aimDirection = transform.up;
            Vector2 center = transform.position;

            yield return new WaitForSeconds(pattern.StartWait);

            while (lap < pattern.Repetitions)
            {
                if (lap > 0 && pattern.AngleOffsetBetweenReps != 0f)
                    aimDirection = aimDirection.Rotate(pattern.AngleOffsetBetweenReps);

                for (int i = 0; i < pattern.PatternSettings.Length; i++)
                {
                    ShotAttack.RadialShot(center, aimDirection, pattern.PatternSettings[i]);
                    if (i < pattern.PatternSettings.Length - 1)
                        yield return new WaitForSeconds(pattern.PatternSettings[i].CooldownAfterShot);
                }
                lap++;
                if (lap < pattern.Repetitions)
                {
                    float cooldown = pattern.PatternSettings[pattern.PatternSettings.Length - 1].CooldownAfterShot;
                    yield return new WaitForSeconds(cooldown);
                }
            }

            yield return new WaitForSeconds(pattern.EndWait);


        _onShotPattern = false;
        }
    }
