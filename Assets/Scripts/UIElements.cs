using UnityEngine;
using Zenject;

public class UIElements : MonoBehaviour
{
    [Inject(Id = "HealButton")] private GameObject _healButton;
    [Inject(Id = "RunAwayButton")] private GameObject _runAwayButton;
    [Inject(Id = "BattleButton")] private GameObject _battleButton;
    [Inject] private PlayerController _playerController;
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    //Application.Quit();
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }

    public void ActivateBattleUI()
    {
        _healButton.SetActive(false);
        _battleButton.SetActive(false);
        _runAwayButton.SetActive(true);
    }

    public void DeactivateBattleUI()
    {
        _healButton.SetActive(true);
        _battleButton.SetActive(true);
        _runAwayButton.SetActive(false);
    }
}
