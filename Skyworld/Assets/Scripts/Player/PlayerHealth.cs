using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public delegate void HealthAction();
    public event HealthAction OnTakeDamage;
    public event HealthAction OnHealDamage;
    public event HealthAction OnDie;
    public event HealthAction OnRespawn;
    public event HealthAction OnGainHeart;

    public int maxHearts;
    public AudioClip heal;
    public AudioClip hurt;
    public AudioClip death;
    [HideInInspector]
    public int health;                            // Health is hearts * 2

    Animator anim;
    AudioSource audioSource;
    ParticleSystem ps;
    bool invulnerable;
    bool dead = false;

    void Awake()
    {
        health = maxHearts * 2;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ps = GetComponent<ParticleSystem>();
    }

    IEnumerator ShakeScreen(float shakiness)
    {
        const float tol = 0.015f;
        const float falloff = 0.77f;
        const float magnitude = 0.9f;

        ShakeEffect effect = Camera.main.GetComponent<ShakeEffect>();
        effect.magnitude = magnitude;
        effect.falloff = falloff / shakiness;
        effect.on = true;
        while (effect.magnitude > tol)
        {
            yield return null;
        }
        effect.on = false;
    }

	public IEnumerator TakeDamage(int damage, bool bigHit = false)
    {
        if (!dead && (!invulnerable || bigHit)) // Big hits are impervious to invulnerability
        {
            GoInvulnerable();
            audioSource.PlayOneShot(hurt);
            health -= damage;
            if (health <= 0)
            {
                health = 0;
            }
            if (OnTakeDamage != null)
            {
                OnTakeDamage();
            }
            if (health == 0)
            {
                Die();
                if (OnDie != null)
                {
                    OnDie();
                }
                StartCoroutine(Regenerate());
            }
            // Pause for dramatic effect on big hits
            if (bigHit)
            {
                float ts = Time.timeScale;
                Time.timeScale = 0.1f;
                yield return new WaitForSecondsRealtime(0.5f);
                Time.timeScale = ts;
            }
            StartCoroutine(ShakeScreen(damage));
        }
    }

    public IEnumerator Heal(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (health == maxHearts * 2)
            {
                break;
            }
            health += 1;
            if (OnHealDamage != null)
            {
                OnHealDamage();
            }
            ps.Play(false);
            audioSource.PlayOneShot(heal);
            yield return new WaitForSeconds(2f / (maxHearts * 2));
        }
    }

    void GoInvulnerable()
    {
        invulnerable = true;
        anim.SetTrigger("Hit");
    }

    void GoBackToNormal()
    {
        invulnerable = false;
    }

    public void GainHeart()
    {
        maxHearts++;
        if (OnGainHeart != null)
        {
            OnGainHeart();
        }
    }

    void Die()
    {
        dead = true;
        anim.SetTrigger("Die");
        Camera.main.GetComponent<Camera2DFollow>().SnapTo(transform.position);
        audioSource.PlayOneShot(death);
    }

    IEnumerator Regenerate()
    {
        const float secondsBeforeRespawn = 0.375f;

        GameManager.Instance.paused = true;
        yield return new WaitForSeconds(secondsBeforeRespawn);  // Allow player to realize they died
        yield return StartCoroutine(GameManager.Instance.Respawn(transform));
        dead = false;
        anim.SetTrigger("Resurrect");
        StartCoroutine(Heal(maxHearts * 2));
        if (OnRespawn != null)
        {
            OnRespawn();
        }
        GameManager.Instance.paused = false;
    }
}