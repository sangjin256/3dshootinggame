using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _staminaRegenRate = 15f;
    [SerializeField] private float _exhaustedRecoveryTime = 3f;

    private PlayerStat _stats;
    private bool _isExhausted;
    private bool _isUsingStamina;

    public void Initialize(PlayerController controller)
    {
        _stats = new PlayerStat(controller.DataSO.MaxHealth, controller.DataSO.MaxStamina);
    }

    private void Update()
    {
        HandleStaminaRegeneration();
    }

    private void HandleStaminaRegeneration()
    {
        if (_stats.Stamina < _stats.MaxStamina && !_isUsingStamina)
        {
            ChargeStamina(_staminaRegenRate * Time.deltaTime);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        _stats.Health -= damageAmount;
        NotifyHealthChanged();

        if (_stats.Health <= 0)
        {
            GameManager.I.GameOver();
        }
    }

    public void UseStamina(float amount)
    {
        _stats.Stamina -= amount;
        if (_stats.Stamina <= 0)
        {
            _stats.Stamina = 0;
            if (!_isExhausted)
            {
                StartCoroutine(HandleExhaustion());
            }
        }
        NotifyStaminaChanged();
    }

    private IEnumerator HandleExhaustion()
    {
        _isExhausted = true;
        NotifyStaminaChanged();
        yield return new WaitForSeconds(_exhaustedRecoveryTime);
        _isExhausted = false;
        _isUsingStamina = false;
    }

    public void ChargeStamina(float amount)
    {
        if (!_isExhausted)
        {
            _stats.Stamina += amount;
            if (_stats.Stamina >= _stats.MaxStamina)
            {
                _stats.Stamina = _stats.MaxStamina;
            }
            NotifyStaminaChanged();
        }
    }

    private void NotifyHealthChanged()
    {
        PlayerEventManager.I.OnHealthChanged?.Invoke(_stats.Health);
    }

    private void NotifyStaminaChanged()
    {
        PlayerEventManager.I.OnStaminaChanged?.Invoke(_stats.Stamina);
    }

    public bool IsExhausted() => _isExhausted;
    public bool IsUsingStamina() => _isUsingStamina;
    public void SetUsingStamina(bool value) => _isUsingStamina = value;
    public float GetCurrentHealth() => _stats.Health;
    public float GetMaxHealth() => _stats.MaxHealth;
    public float GetCurrentStamina() => _stats.Stamina;
    public float GetMaxStamina() => _stats.MaxStamina;
}
