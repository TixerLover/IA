using UnityEngine;


    public class RadialShotPatternVisualizer : MonoBehaviour
    {
        [SerializeField] private RadialShotPattern _pattern;
        [SerializeField] private float _radius;
        [SerializeField] private Color _color;
        [SerializeField, Range(0f, 5f)] private float _testTime;

        private void OnDrawGizmos()
        {
            if (_pattern == null)
                return;

            Gizmos.color = _color;

            int lap = 0;
            Vector2 aimDirection = transform.up;
            Vector2 center = transform.position;

            float timer = _testTime;

            while (timer > 0f && lap < _pattern.Repetitions)
            {
                if (lap > 0 && _pattern.AngleOffsetBetweenReps != 0f)
                    aimDirection = aimDirection.Rotate(_pattern.AngleOffsetBetweenReps);

                for (int i = 0; i < _pattern.PatternSettings.Length; i++)
                {
                    if (timer < 0f)
                        break;

                    DrawRadialShot(_pattern.PatternSettings[i], timer, aimDirection);

                    if (i < _pattern.PatternSettings.Length - 1)
                        timer -= _pattern.PatternSettings[i].CooldownAfterShot;
                }
                lap++;
                if (lap < _pattern.Repetitions)
                {
                    float cooldown = _pattern.PatternSettings[_pattern.PatternSettings.Length - 1].CooldownAfterShot;
                    timer -= cooldown;
                }
            }
        }

        private void DrawRadialShot(
            RadialShotSettings settings, float lifeTime, Vector2 aimDirection)
        {
            float angleBetweenBullets = 360f / settings.NumberOfBullets;
            if (settings.PhaseOffset != 0f || settings.AngleOffset != 0f)
                aimDirection = aimDirection.Rotate(angleBetweenBullets * settings.PhaseOffset + settings.AngleOffset);

            for (int i = 0; i < settings.NumberOfBullets; i++)
            {
                float bulletDirectionAngle = angleBetweenBullets * i;

                if (settings.RadialMask && bulletDirectionAngle > settings.MaskAngle)
                    break;

                Vector2 bulletDirection = aimDirection.Rotate(bulletDirectionAngle);
                Vector2 bulletPosition = (Vector2)transform.position
                    + bulletDirection * settings.BulletSpeed * lifeTime;
                Gizmos.DrawSphere(bulletPosition, _radius);
            }
        }
    }
