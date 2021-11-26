using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickButton : MonoBehaviour
{
    [SerializeField] private Transform _pacman;
    private PacmanMoving _moving;
    void Start()
    {
        _moving = _pacman.GetComponent<PacmanMoving>();
    }

    public void SetDirectionLeft()
    {
        _moving.SetDirection(new Vector3Int(-1, 0, 0));
    }

    public void SetDirectionRight()
    {
        _moving.SetDirection(new Vector3Int(1, 0, 0));
    }

    public void SetDirectionUp()
    {
        _moving.SetDirection(new Vector3Int(0, 1, 0));
    }

    public void SetDirectionDown()
    {
        _moving.SetDirection(new Vector3Int(0, -1, 0));
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    } 

    public void ApplicationExit()
    {
        SceneManager.LoadScene(0);
    }



}
