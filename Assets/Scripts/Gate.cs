using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Gate : MonoBehaviour
{
    [SerializeField] private GateController[] _ghosts;
    [SerializeField] private Tilemap _tileGround;
    [SerializeField] private Tilemap _tileLevel;
    [SerializeField] private TileBase _tileBlack;
    [SerializeField] private Vector3Int _gatePosition;
    void Update()
    {
        for (int i = 0; i < _ghosts.Length; ++i)
        {
            if (!_ghosts[i].GetFree)
            {
                return;
            }
        }
        _tileGround.SetTile(_gatePosition, _tileBlack);
        _tileLevel.SetTile(_gatePosition, _tileBlack);
    }
}
