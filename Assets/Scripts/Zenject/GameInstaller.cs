using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Header("Animations parametres")]
    [SerializeField] private Animator _canvasAnimator;
    [SerializeField] private AnimationController _playerAnimationController;
    [SerializeField] private AnimationClip _deadAnimationClip;
    [SerializeField] private AnimationClip _meleeAnimationClip;
    [SerializeField] private AnimationClip _rangeAnimationClip;

    [Header("Controllers")]
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private EnemiesManager _enemiesManager;
    [SerializeField] private GameController _gameController;

    [Header("Player components")]
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private ParticleSystem _playerSwapParticles;
    [SerializeField] private TMP_Text _playerStateText;
    [SerializeField] private TMP_Text _playerHealthText;
    [SerializeField] private TMP_Text _swapText;
    [SerializeField] private Image _playerPrepareImage;
    [SerializeField] private Image _playerSwapImage;

    [Header("Enemies parametres")]
    [SerializeField] private List<GameObject> _enemies;
    [SerializeField] private Transform _enemiesSpawnPoint;

    [Header("UI")]
    [SerializeField] private UIElements _UIElements;
    [SerializeField] private GameObject _healButton;
    [SerializeField] private GameObject _runAwayButton;
    [SerializeField] private GameObject _battleButton;

    public override void InstallBindings()
    {
        Container.Bind<Animator>().WithId("CanvasAnimator").FromInstance(_canvasAnimator);
        Container.Bind<AnimationController>().FromInstance(_playerAnimationController);

        Container.Bind<AnimationClip>().WithId("DeadAnimation").FromInstance(_deadAnimationClip);
        Container.Bind<AnimationClip>().WithId("MeleeAnimation").FromInstance(_meleeAnimationClip);
        Container.Bind<AnimationClip>().WithId("RangeAnimation").FromInstance(_rangeAnimationClip);

        Container.Bind<PlayerController>().FromInstance(_playerController);
        Container.Bind<GameController>().FromInstance(_gameController);
        Container.Bind<EnemiesManager>().FromInstance(_enemiesManager);

        Container.Bind<PlayerData>().FromInstance(_playerData);
        Container.Bind<ParticleSystem>().FromInstance(_playerSwapParticles);

        Container.Bind<TMP_Text>().WithId("StateText").FromInstance(_playerStateText);
        Container.Bind<TMP_Text>().WithId("HealthText").FromInstance(_playerHealthText);
        Container.Bind<TMP_Text>().WithId("SwapText").FromInstance(_swapText);

        Container.Bind<Image>().WithId("PrepareImage").FromInstance(_playerPrepareImage);
        Container.Bind<Image>().WithId("SwapImage").FromInstance(_playerSwapImage);

        Container.Bind<List<GameObject>>().FromInstance(_enemies);
        Container.Bind<Transform>().FromInstance(_enemiesSpawnPoint);

        Container.Bind<UIElements>().FromInstance(_UIElements);
        Container.Bind<GameObject>().WithId("HealButton").FromInstance(_healButton);
        Container.Bind<GameObject>().WithId("RunAwayButton").FromInstance(_runAwayButton);
        Container.Bind<GameObject>().WithId("BattleButton").FromInstance(_battleButton);

        Container.Resolve<GameController>().StartGame();
    }
}