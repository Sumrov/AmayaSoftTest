using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup _loadPanel;
    [SerializeField] private CanvasGroup _restartPanel;
    [SerializeField] private Button _restartButton;
    [SerializeField] private TextMeshProUGUI _taskDiscriptionText;
    [SerializeField] private ParticleSystem _particalSystem;
    [SerializeField] private Transform _cell;
    [SerializeField] private Transform _container;
    [SerializeField] private Level _level;

    private void Awake()
    {
        _restartButton.onClick.AddListener(() =>
        {
            _loadPanel.alpha = 0f;
            _loadPanel.gameObject.SetActive(true);
            _loadPanel.DOFade(1f, 1f).OnComplete(_level.Restart);
        });

        _level.OnStart += Level_OnStart;
        _level.OnTaskUpdate += Level_OnTaskUpdate;
        _level.OnLevelComplete += Level_OnLevelComplete;
        _level.OnTaskComplete += Level_OnTaskComplete;
        _level.OnLevelRestart += Level_OnLevelRestart;
    }

    private void Level_OnLevelRestart(object sender, TaskSO task)
    {
        _loadPanel.DOFade(.0f, .5f).OnComplete(() => _loadPanel.gameObject.SetActive(false));
        _restartPanel.gameObject.SetActive(false);
        UpdateVisual(task, true);
    }

    private void Level_OnTaskComplete(object sender, TaskSO task)
    {
        for (int i = 0; i < _container.childCount; i++)
        {
            if (i == task.Key)
            {
                _particalSystem.transform.position = _container.GetChild(i).transform.position;
                _particalSystem.Play();
                break;
            }
        }
    }

    private void Level_OnLevelComplete(object sender, System.EventArgs e)
    {
        _restartPanel.alpha = 0f;
        _restartPanel.gameObject.SetActive(true);
        _restartPanel.DOFade(.8f, 1f);
    }

    private void Level_OnTaskUpdate(object sender, TaskSO task)
    {
        UpdateVisual(task, false);
    }

    private void Level_OnStart(object sender, TaskSO task)
    {
        _loadPanel.gameObject.SetActive(false);
        _restartPanel.gameObject.SetActive(false);
        UpdateVisual(task, true);
    }

    private void UpdateVisual(TaskSO task, bool animation)
    {
        _taskDiscriptionText.text = task.Discription;
        if (animation)
        {
            _taskDiscriptionText.alpha = .0f;
            _taskDiscriptionText.DOFade(1f, 1f);
        }

        DeleteCells();
        CreateCells(task, animation);
    }

    private void CreateCells(TaskSO task, bool animation)
    {
        for (int i = 0; i < task.Options.Length; i++)
        {
            int key = i;
            Sprite sprite = task.Options[i];
            Transform cell = Instantiate(_cell, _container);
            cell.GetComponent<CellUI>().Show(sprite, () => _level.CheckKey(key));
            if (animation)
                cell.transform.DOPunchScale(Vector3.one * .1f, .5f);
        }
    }

    private void DeleteCells()
    {
        foreach (Transform child in _container)
            Destroy(child.gameObject);
    }

    private void OnDestroy()
    {
        _level.OnStart -= Level_OnStart;
        _level.OnTaskUpdate -= Level_OnTaskUpdate;
        _level.OnLevelComplete -= Level_OnLevelComplete;
        _level.OnTaskComplete -= Level_OnTaskComplete;
        _level.OnLevelRestart -= Level_OnLevelRestart;
    }
}
