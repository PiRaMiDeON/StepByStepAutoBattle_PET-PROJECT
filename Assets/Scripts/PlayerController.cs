using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [Inject] private PlayerData _playerData;
    [Inject] private ParticleSystem _swapParticles;
    [Inject] private AnimationController _animationController;
    [Inject] private GameController _gameController;
    [Inject] private EnemiesManager _enemiesManager;

    [Inject(Id = "PrepareImage")] private Image _prepareImage;
    [Inject(Id = "SwapImage")] private Image _swapImage;
    [Inject(Id = "StateText")] private TMP_Text _stateText;
    [Inject(Id = "HealthText")] private TMP_Text _healthText;
    [Inject(Id = "SwapText")] private TMP_Text _swapText;
    [Inject(Id = "DeadAnimation")] private AnimationClip _deadAnimation;
    [Inject(Id = "MeleeAnimation")] private AnimationClip _meleeAnimation;
    [Inject(Id = "RangeAnimation")] private AnimationClip _rangeAnimation;

    private int _healthCount;
    private int _protectionCount;
    private float _saveFillValue;

    private AttackType _attackType = AttackType.Melee;

    private bool _attacking;
    private bool _swapping;
    private bool _needSwap;
    private bool _preparing;
    [HideInInspector] public bool _dead;
    private bool _firstBattle = true;

    public void Initialize()
    {
        if (_firstBattle)
        {
            _firstBattle = false;
            _healthCount = _playerData.HealthCount;
            _protectionCount = _playerData.ProtectionCount;
        }

        _healthText.text = $"Health: {_healthCount}/{_playerData.HealthCount}";

        _swapImage.fillAmount = 0;
        _prepareImage.fillAmount = 0;

        StartCoroutine(Prepare());
    }

    private IEnumerator Attack()
    {
        if (!_swapping && !_dead)
        {
            _attacking = true;

            _stateText.text = "Attack";
            _stateText.color = Color.red;

            switch (_attackType)
            {
                case AttackType.Melee:

                    _animationController.MeleeAttackAnimate();

                    yield return new WaitForSeconds(_meleeAnimation.length);

                    _enemiesManager._currentEnemy.TakeDamage(_playerData.MeleeDamage);

                    break;

                case AttackType.Range:

                    _animationController.RangeAttackAnimate();

                    yield return new WaitForSeconds(_rangeAnimation.length);

                    _enemiesManager._currentEnemy.TakeDamage(_playerData.RangeDamage);

                    break;
            }

            _attacking = false;

            if (_needSwap)
            {
                _needSwap = false;
                StartCoroutine(SwapAttackType());
                yield break;
            }

            StartCoroutine(Prepare());
        }
    }
    public IEnumerator Prepare()
    {
        if (!_swapping && !_dead)
        {
            _preparing = true;

            _stateText.text = "Prepare";
            _stateText.color = Color.yellow;

            _prepareImage.enabled = true;
            _prepareImage.fillAmount = _saveFillValue;

            while (_prepareImage.fillAmount < 1 && !_swapping)
            {
                _prepareImage.fillAmount += 0.1f / _playerData.Cooldown;
                _saveFillValue = _prepareImage.fillAmount;
                yield return new WaitForSeconds(0.1f);
            }

            if (_prepareImage.fillAmount >= 1)
            {
                _prepareImage.fillAmount = 0;
            }

            if (_saveFillValue >= 1)
            {
                _saveFillValue = 0;
            }

            _prepareImage.enabled = false;

            _preparing = false;

            StartCoroutine(Attack());
        }
    }
    public void SwapAttack()
    {
        if (!_swapping && !_dead)
        {
            if (_preparing)
            {
                StopCoroutine(Prepare());
            }

            StartCoroutine(SwapAttackType());
        }
    }
    private IEnumerator SwapAttackType()
    {
        if (_attacking)
        {
            _needSwap = true;
            yield break;
        }

        _swapping = true;

        _stateText.text = "Swap";
        _stateText.color = Color.green;

        if (_swapText.text == "Melee")
        {
            _swapText.text = "Range";
        }
        else
        {
            _swapText.text = "Melee";
        }

        _swapParticles.Play();

        _prepareImage.enabled = false;
        _swapImage.enabled = true;

        while (_swapImage.fillAmount < 1)
        {
            _swapImage.fillAmount += 0.25f;
            yield return new WaitForSeconds(0.5f);
        }

        _swapImage.fillAmount = 0;
        _swapImage.enabled = false;

        if (_attackType == AttackType.Melee)
        {
            _attackType = AttackType.Range;
        }
        else
        {
            _attackType = AttackType.Melee;
        }

        _swapping = false;

        if (_gameController._inBattle)
        {
            StartCoroutine(Prepare());
        }
    }

    public void HealPlayer()
    {
        if (!_dead)
        {
            _healthCount = _playerData.HealthCount;
        }
    }
    public void RunAway()
    {
        StopAllCoroutines();
        _enemiesManager._currentEnemy.StopAttack();
        _gameController.StopBattle();
    }

    public void Victory()
    {
        StopAllCoroutines();

        _attacking = false;
        _preparing = false;
        _swapping = false;
        _needSwap = false;

        _gameController.StopBattle();
    }

    private IEnumerator Dead()
    {
        _dead = true;
        _attacking = false;
        _preparing = false;

        _animationController.DeadAnimate();

        yield return new WaitForSeconds(_deadAnimation.length);

        _gameController.GameOver();
    }
    public void TakeDamage(int damage)
    {
        if (!_dead)
        {
            int resultDamage = damage - _protectionCount;

            if (resultDamage <= 0)
            {
                resultDamage = 1;
            }

            _healthCount -= resultDamage;

            if (_healthCount <= 0)
            {
                _healthText.text = $"Health: 0/{_playerData.HealthCount}";
                StopAllCoroutines();
                StartCoroutine(Dead());
            }
            else
            {
                _healthText.text = $"Health: {_healthCount}/{_playerData.HealthCount}";
            }
        }
    }

}
