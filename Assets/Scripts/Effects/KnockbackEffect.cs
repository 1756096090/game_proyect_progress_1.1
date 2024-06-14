using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class KnockbackEffect : MonoBehaviour
    {
        [SerializeField] private float knockbackTime = 0.2f;
        [SerializeField] private float hitDirectionForce = 10f;
        [SerializeField] private float constForce = 5f;
        [SerializeField] private float inputForce = 7.5f;

        public bool IsBeingKnockedBack { get; private set; }

        private Rigidbody2D rb;
        private Coroutine knockbackCoroutine;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
        {
            IsBeingKnockedBack = true;

            Vector2 hitForce;
            Vector2 constantForce;
            Vector2 knockbackForce;
            Vector2 combinedForce;

            hitForce = hitDirection * hitDirectionForce * inputForce;
            constantForce = constantForceDirection * constForce;

            float elapsedTime = 0;
            while(elapsedTime < knockbackTime)
            {
                //Iterate the timer
                elapsedTime += Time.fixedDeltaTime;
                knockbackForce = hitForce + constantForce;

                if(inputDirection != 0)
                {
                    combinedForce = knockbackForce + new Vector2(inputDirection, 0f);
                } else
                {
                    combinedForce = knockbackForce;
                }
                rb.velocity = combinedForce;

                yield return new WaitForFixedUpdate();
            }

            IsBeingKnockedBack = false;

        }

        public void CallKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
        {
            knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));
        }
    }
}