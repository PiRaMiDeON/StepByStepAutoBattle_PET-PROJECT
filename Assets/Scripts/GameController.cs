using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject(Id = "CanvasAnimator")] private Animator _Fader;
    [Inject] private PlayerController _playerController;
    [Inject] private EnemiesManager _enemiesManager;
    [Inject] private UIElements _UIElements;
    public bool _inBattle;

    public void StartGame()
    {
        _enemiesManager.SpawnEnemy();
    }

    public void StartBattle()
    {
        _inBattle = true;
        _UIElements.ActivateBattleUI();
        _playerController.Initialize();
        _enemiesManager._currentEnemy.Initialize();
    }

    public void StopBattle()
    {
        _inBattle = false;
        _UIElements.DeactivateBattleUI();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        _Fader.SetTrigger("GameOver");

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }
}
