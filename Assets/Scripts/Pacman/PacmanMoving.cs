using UnityEngine;
using UnityEngine.Tilemaps;

public class PacmanMoving : MonoBehaviour
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private ScoreDigits _score;
    [SerializeField] private TileBase _emptyTile;
    [SerializeField] private TileBase _dotTile;

    private SpriteRenderer _sprite;
    private AudioSource[] _audio;
    private Vector3Int _gridPosition;
    private Vector3Int _direction;
    private float _time = 0;
    private float _timeFunky = 0;
    private bool isFunky = false;

    public delegate void ScoreDelegate(int value);
    public event ScoreDelegate ScoreChange;

    public delegate void Bonus();
    public event Bonus TakeBonus, EndBonus;

    public Vector3Int GridPosition => _gridPosition;

    public Tilemap TileMap => _tilemap;

    void Start()
    {
        _direction = new Vector3Int(1, 0, 0);
        _sprite = GetComponent<SpriteRenderer>();
        _audio = GetComponents<AudioSource>();
        _gridPosition = _tilemap.WorldToCell(transform.position);
    }

    void FixedUpdate()
    {
        if (isFunky)
        {
            _timeFunky -= Time.deltaTime;
            if (_timeFunky < 0)
            {
                isFunky = false;
                EndBonus?.Invoke();
            }
        }
            

        _time += Time.deltaTime;
        if (_time > 0.25f)
        {
            _time = 0;
            TryMove();
        }
    }

    protected bool TryMove()
    {
        Vector3Int newGridPosition = _gridPosition + _direction;

        if (CanMove(newGridPosition) == false)
        {
            return false;
        }

        _gridPosition = newGridPosition;
        transform.position = _tilemap.CellToWorld(_gridPosition);
        return true;
    }

    protected bool CanMove(Vector3Int gridPos)
    {
        if (_tilemap.GetTile(gridPos).name.StartsWith("spritesheet"))
        {
            return false;
        }
        else if (_tilemap.GetTile(gridPos).name.StartsWith("pacdot"))
        {
            _audio[0].Play();
            _tilemap.SetTile(gridPos, _emptyTile);
            ScoreChange?.Invoke(10);
            if(_tilemap.ContainsTile(_dotTile) == false)
            {
                _audio[2].Play();
                Time.timeScale = 0;
                Debug.Log("Finish");
            }
        }
        else if (_tilemap.GetTile(gridPos).name.StartsWith("bonus")){
            _audio[1].Play();
            _tilemap.SetTile(gridPos, _emptyTile);
            ScoreChange?.Invoke(50);
            TakeBonus?.Invoke();
            _timeFunky = 5.0f;

        }

        return true;
    }

    public void SetDirection(Vector3Int direction)
    {
        if (_tilemap.GetTile(_gridPosition + direction).name.StartsWith("spritesheet") == false)
        {
            _direction = direction;
            if (_direction.x >= 0)
                _sprite.flipX = false;
            else
                _sprite.flipX = true;
            transform.rotation = Quaternion.Euler(0, 0, 90f * _direction.y);
        }
    }
}
