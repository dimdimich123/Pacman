using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDigits : MonoBehaviour
{
    [SerializeField] private Sprite[] _digits;
    [SerializeField] private PacmanMoving _pacman;
    [SerializeField] private GhostMovingRed _ghostRed;
    [SerializeField] private GhostMovingPink _ghostPink;
    [SerializeField] private GhostMovingYellow _ghostYellow;
    [SerializeField] private GhostMovingBlue _ghostBlue;
    [SerializeField] private FruitSpawn _fruit;

    private List<Transform> _childImage = new List<Transform>();
    private int _score = 0;
    private int _combo = 1;

    void Start()
    {
        _pacman.ScoreChange += ChangeScore;
        _pacman.EndBonus += ResetCombo;
        _ghostRed.ScoreChange += ChangeScoreCombo;
        _ghostYellow.ScoreChange += ChangeScoreCombo;
        _ghostPink.ScoreChange += ChangeScoreCombo;
        _ghostBlue.ScoreChange += ChangeScoreCombo;
        _fruit.ScoreChange += ChangeScore;


        foreach (Transform child in transform)
        {
            _childImage.Add(child);
        }
    }

    private void ResetCombo() => _combo = 1;

    private void ChangeScore(int value) 
    { 
        _score += value;
        UpdateScoreUI();
    }

    private void ChangeScoreCombo(int value) 
    { 
        _score += value * _combo++;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        string str = _score.ToString();

        for (int i = _childImage.Count - str.Length, j = 0; j < str.Length; ++i, ++j)
        {
            _childImage[i].GetComponent<Image>().sprite = _digits[str[j] - 48];
        }
    }

    private void OnDisable()
    {
        _pacman.ScoreChange -= ChangeScore;
        _pacman.EndBonus -= ResetCombo;
        _ghostRed.ScoreChange -= ChangeScoreCombo;
        _ghostYellow.ScoreChange -= ChangeScoreCombo;
        _ghostPink.ScoreChange -= ChangeScoreCombo;
        _ghostBlue.ScoreChange -= ChangeScoreCombo;
        _fruit.ScoreChange -= ChangeScore;
    }
}
