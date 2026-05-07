using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sword : ActiveGun.Weapon
{
    [SerializeField] private int _damageAmount = 10;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _swingSound;

    public event EventHandler OnSwordSwing;
    private PolygonCollider2D _polygonCollider2D;

    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start()
    {
        AttackColliderTurnOffOn();
    }

    public override void Attack()
    {
        AttackColliderTurnOn();

        // ¬Œ“ «ƒ≈—‹ »√–¿≈Ã «¬”  ¬«Ã¿’¿
        if (_audioSource != null && _swingSound != null)
        {
            _audioSource.PlayOneShot(_swingSound);
        }

        OnSwordSwing?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.TryGetComponent(out Skeleton skeleton))
        {
            skeleton.TakeDamage(_damageAmount);
        }
    }

    public void AttackColliderTurnOff()
    {
        _polygonCollider2D.enabled = false;
    }

    private void AttackColliderTurnOn()
    {
        _polygonCollider2D.enabled = true;
    }

    private void AttackColliderTurnOffOn()
    {
        AttackColliderTurnOff();
        AttackColliderTurnOn();
    }
}
 