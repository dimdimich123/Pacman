using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostMovingRed : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3Int _respawnPosition;

    private AudioSource _audio;
    private Animator _anim;
    private PacmanMoving _pacman;
    private Vector3Int _direction;
    private Vector3Int _gridPosition;
    private float _time = 0;
    private float _time2 = 0;
    private float _timeChase = 0;
    private float _timeFunky = 0;
    private List<PathFind.Point> _path = new List<PathFind.Point>();
    private PathFind.Grid grid;
    private bool isChase = false;
    private bool isFunky = false;

    public delegate void ScoreDelegate(int value);
    public event ScoreDelegate ScoreChange;

    public bool GetFunky => isFunky;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _pacman = _target.GetComponent<PacmanMoving>();
        _pacman.TakeBonus += SetFunky;
        _direction = new Vector3Int(1, 0, 0);
        _gridPosition = _tilemap.WorldToCell(transform.position);
        print(_gridPosition);

        bool[,] tilesmap = new bool[_tilemap.size.x, _tilemap.size.y];

        for (int y = _tilemap.origin.y; y < (_tilemap.origin.y + _tilemap.size.y); y++)
        {
            for (int x = _tilemap.origin.x; x < (_tilemap.origin.x + _tilemap.size.x); x++)
            {
                if(_tilemap.HasTile(new Vector3Int(x, y, 0)))
                {
                    tilesmap[x - _tilemap.origin.x, y - _tilemap.origin.y] = true;
                }
                else
                {
                    tilesmap[x - _tilemap.origin.x, y - _tilemap.origin.y] = false;
                }
            }
        }
        tilesmap[_respawnPosition.x - _tilemap.origin.x, _respawnPosition.y + 1 - _tilemap.origin.y] = true;

        grid = new PathFind.Grid(_tilemap.size.x, _tilemap.size.y, tilesmap);
    }

    void FixedUpdate()
    {
        if (!isFunky)
        {
            if (isChase)
                _timeChase += Time.deltaTime;

            if (_tilemap.WorldToCell(transform.position) == _tilemap.WorldToCell(_target.position))
            {
                _audio.Play();
                Time.timeScale = 0;
                Debug.Log("GameOver");

            }

            _time += Time.deltaTime;
            if (_time > 0.275f)
            {
                if (!isChase)
                {
                    Patrol();
                    isChase = SeePacman();
                }
                else
                {
                    Chase();
                    if (SeePacman())
                        _timeChase = 0;
                    if (_timeChase > 5.0f)
                    {
                        isChase = false;
                        _gridPosition = _tilemap.WorldToCell(transform.position);
                    }
                }
                _time = 0;
            }
        }
        else
        {
            Funky();
            if (_tilemap.WorldToCell(transform.position) == _tilemap.WorldToCell(_target.position))
            {
                _gridPosition = _respawnPosition;
                transform.position = _tilemap.CellToWorld(_gridPosition);
                isFunky = false;
                _anim.SetBool("Funky", false);
                AnimationMoving();
                ScoreChange?.Invoke(200);
            }
        }
    }

    private void SetFunky()
    {
        isChase = false;
        isFunky = true;
        _gridPosition = _tilemap.WorldToCell(transform.position);
        _anim.SetBool("Funky", true);
        _timeFunky = 5.0f;
    }

    private void Funky()
    {
        _timeFunky -= Time.deltaTime;
        if(_timeFunky < 0)
        {
            isFunky = false;
            _gridPosition = _tilemap.WorldToCell(transform.position);
            _anim.SetBool("Funky", false);
            AnimationMoving();
        }
        else
        {
            _time2 += Time.deltaTime;
            if(_time2 > 0.2f)
            {
                _time2 = 0;
                Patrol();
            }
            
        }
    }

    private void Chase()
    {
        PathFind.Point _from = new PathFind.Point(_tilemap.WorldToCell(transform.position).x - _tilemap.origin.x,
                                                _tilemap.WorldToCell(transform.position).y - _tilemap.origin.y);
        PathFind.Point _to = new PathFind.Point(_tilemap.WorldToCell(_target.position).x - _tilemap.origin.x,
                                                _tilemap.WorldToCell(_target.position).y - _tilemap.origin.y);

        _path = PathFind.Pathfinding.FindPath(grid, _from, _to);
        int x = _from.x - _path[0].x;
        int y = _from.y - _path[0].y;

        if (x != 0 && y != 0)
        {
            if (_tilemap.HasTile(new Vector3Int(_path[0].x + x + _tilemap.origin.x, _path[0].y + _tilemap.origin.y, 0)))
                _path[0].Set(_from.x, _path[0].y);
            else
                _path[0].Set(_path[0].x, _from.y);
        }
        transform.position = _tilemap.CellToWorld(new Vector3Int(_path[0].x + _tilemap.origin.x, _path[0].y + _tilemap.origin.y, 0));
        _direction = _tilemap.WorldToCell(transform.position) - _gridPosition;
        AnimationMoving();
        _gridPosition = _tilemap.WorldToCell(transform.position);
    }

    private void Patrol()
    {
        List<Vector3Int> MoveTo = new List<Vector3Int>();
        Vector3Int sub = (_gridPosition + _direction) - _gridPosition;
        Vector3Int cell1 = _gridPosition + new Vector3Int(sub.y, sub.x, 0);
        Vector3Int cell2 = _gridPosition - new Vector3Int(sub.y, sub.x, 0);
        Vector3Int cell3 = _gridPosition + _direction;
        if(_tilemap.HasTile(cell1))
            MoveTo.Add(cell1);
        if (_tilemap.HasTile(cell2))
            MoveTo.Add(cell2);
        if (_tilemap.HasTile(cell3))
            MoveTo.Add(cell3);

        if (MoveTo.Count == 0)
        {
            _direction = -_direction;
            return;
        }

        int randDirection = new System.Random().Next(0, MoveTo.Count);
        _direction = MoveTo[randDirection] - _gridPosition;
        if(!isFunky)
            AnimationMoving();
        _gridPosition = MoveTo[randDirection];
        transform.position = _tilemap.CellToWorld(_gridPosition);
    }

    private void AnimationMoving()
    {
        if(_direction.x == 1 && _direction.y == 0)
        {
            _anim.SetBool("Right", true);
            _anim.SetBool("Left", false);
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", false);
        }
        else if (_direction.x == -1 && _direction.y == 0)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", true);
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", false);
        }
        else if (_direction.x == 0 && _direction.y == 1)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", false);
            _anim.SetBool("Up", true);
            _anim.SetBool("Down", false);
        }
        else if (_direction.x == 0 && _direction.y == -1)
        {
            _anim.SetBool("Right", false);
            _anim.SetBool("Left", false);
            _anim.SetBool("Up", false);
            _anim.SetBool("Down", true);
        }
    }

    private bool SeePacman()
    {
        if(_pacman.GridPosition.x == _gridPosition.x)
        {
            for (int i = Mathf.Min(_pacman.GridPosition.y, _gridPosition.y); i < Mathf.Max(_pacman.GridPosition.y, _gridPosition.y); ++i)
            {
                if (_tilemap.HasTile(new Vector3Int(_gridPosition.x, i, 0)) == false)
                {
                    return false;
                }
            }
            return true;
        }
        else if(_pacman.GridPosition.y == _gridPosition.y)
        {
            for (int i = Mathf.Min(_pacman.GridPosition.x, _gridPosition.x); i < Mathf.Max(_pacman.GridPosition.x, _gridPosition.x); ++i)
            {
                if (_tilemap.HasTile(new Vector3Int(i, _gridPosition.y, 0)) == false)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private void OnDisable()
    {
        _pacman.TakeBonus -= SetFunky;
    }

}
