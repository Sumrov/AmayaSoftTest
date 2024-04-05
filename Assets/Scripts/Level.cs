using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Level : MonoBehaviour
{
    public event EventHandler<TaskSO> OnStart;
    public event EventHandler<TaskSO> OnTaskComplete;
    public event EventHandler<TaskSO> OnTaskUpdate;
    public event EventHandler<TaskSO> OnLevelRestart;
    public event EventHandler OnLevelComplete;
    [SerializeField] private List<TaskSO> _tasks;
    private int _currentTaskIndex;

    public bool CheckKey(int key)
    {
        if (_tasks[_currentTaskIndex].Key == key)
        {
            OnTaskComplete?.Invoke(this, _tasks[_currentTaskIndex]);
            NextTask();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Restart()
    {
        _currentTaskIndex = 0;
        OnLevelRestart?.Invoke(this, _tasks[_currentTaskIndex]);
    }

    private void Start()
    {
        _currentTaskIndex = 0;
        OnStart?.Invoke(this, _tasks[_currentTaskIndex]);
    }

    private void NextTask()
    {
        if (_currentTaskIndex + 1 >= _tasks.Count)
        {
            OnLevelComplete?.Invoke(this, EventArgs.Empty);
            return;
        }

        _currentTaskIndex++;
        OnTaskUpdate?.Invoke(this, _tasks[_currentTaskIndex]);
    }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this List<T> list)
    {
        Random random = new Random();

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            T temp = list[j];
            list[j] = list[i];
            list[i] = temp;
        }
    }
}
