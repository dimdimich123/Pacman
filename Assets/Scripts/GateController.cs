using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GateController : MonoBehaviour
{
    [SerializeField] private Tilemap _tileGround;
    [SerializeField] private Tilemap _tileLevel;
    [SerializeField] private Vector3Int _gatePosition;
    [SerializeField] private TileBase _tileBlack;
    [SerializeField] private TileBase _tileGate;
    [SerializeField] private float _timeToFree;

    private float _time = 0;
    private bool _ghost = false;
    private bool _freeGhost = false;
    public bool GetFree => _freeGhost;

    public delegate void GhostFreeDelegate();
    public event GhostFreeDelegate GhostFreeEvent;

    void FixedUpdate()
    {
        if (!_freeGhost)
        {
            _time += Time.deltaTime;

            if(!_ghost && _time > _timeToFree)
            {
                _ghost = true;
                _tileGround.SetTile(_gatePosition, _tileBlack);
                _tileLevel.SetTile(_gatePosition, _tileBlack);
                GhostFreeEvent?.Invoke();
                _time = 0;
            }
            else if(_ghost && _time > 5.0f)
            {
                _freeGhost = true;
                _tileGround.SetTile(_gatePosition, null);
                _tileLevel.SetTile(_gatePosition, _tileGate);
            }
        }
    }
}
