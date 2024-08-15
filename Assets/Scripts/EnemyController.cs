using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [Inject] private PlayerController _playerController;
    [Inject] private EnemiesManager _enemiesManager;
    [Inject(Id = "DeadAnimation")] private AnimationClip _deadAnimation;
    [Inject(Id = "MeleeAnimation")] private AnimationClip _meleeAnimation;
    [Inject] private DiContainer _container;

    [SerializeField] private AnimationController _animationController;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Image _prepareImage;
    [SerializeField] private TMP_Text _stateText;
    [SerializeField] private TMP_Text _healthText;

    private int _healthCount;
    private int _protectionCount;
    private bool _dead;
    private bool _firstBattle = true;
    public void Initialize()
    {
        if (_firstBattle)
        {
            _firstBattle = false;
            _healthCount = _enemyData.HealthCount;
            _protectionCount = _enemyData.ProtectionCount;
        }

        _healthText.text = $"Health: {_healthCount}/{_enemyData.HealthCount}";

        _prepareImage.fillAmount = 0;

        if (_healthCount <= 0)
        {
            Destroy(gameObject);
            _enemiesManager.SpawnEnemy();
            _enemiesManager._currentEnemy.Initialize();
        }
        else
        {
            StartCoroutine(Prepare());
        }
    }

    public void StopAttack()
    {
        StopAllCoroutines();
    }

    private IEnumerator Attack()
    {
        _stateText.color = Color.red;
        _stateText.text = "Attack";

        _playerController.TakeDamage(_enemyData.DamageCount);
        _animationController.MeleeAttackAnimate();

        yield return new WaitForSeconds(_meleeAnimation.length);

        StartCoroutine(Prepare());
    }

    private IEnumerator Dead()
    {
        _dead = true;

        _prepareImage.enabled = false;
        _stateText.text = "Dead";
        _stateText.color = Color.black;

        _animationController.DeadAnimate();

        yield return new WaitForSeconds(_deadAnimation.length);

        Destroy(gameObject);
        _playerController.Victory();
        _enemiesManager.SpawnEnemy();
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
                _healthText.text = $"Health: 0/{_enemyData.HealthCount}";

                if (!_playerController._dead)
                {
                    StopAllCoroutines();
                }
                
                StartCoroutine(Dead());
            }
            else
            {
                _healthText.text = $"Health: {_healthCount}/{_enemyData.HealthCount}";
            }
        }
    }

    private IEnumerator Prepare()
    {
        _prepareImage.enabled = true;
        _stateText.color = Color.yellow;
        _stateText.text = "Prepare";
        _prepareImage.fillAmount = 0;

        while (_prepareImage.fillAmount < 1)
        {
            _prepareImage.fillAmount += 0.1f / _enemyData.Cooldown;
            yield return new WaitForSeconds(0.1f);
        }

        _prepareImage.fillAmount = 0;
        _prepareImage.enabled = false;

        StartCoroutine(Attack());
    }
}
