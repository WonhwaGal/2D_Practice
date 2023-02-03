using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlatformerMVC
{
    public class GeneratorController
    {
        private Tilemap _tilemap;
        public Tilemap _snowTilemap;
        public Tile _snowTile;
        private Tile _tile;
        private Tile _rightTile;
        private Tile _leftTile;
        private Tile _topTile;
        public Tile _cornerBottomRight;
        public Tile _cornerBottomleftt;
        public Tile _cornerUpRight;
        public Tile _cornerUpLeft;
        private int _mapHeight;
        private int _mapWidth;

        private int _fillPercent;
        private int _smoothPercent;

        private bool _borders;
        private int[,] _map;
        
        private MarshingSquareController _marshController;
        public GeneratorController(GeneratorLevelView view)
        {
            _tilemap = view._tilemap;
            _snowTilemap = view._snowTilemap;
            _snowTile = view._snowTile;
            _tile = view._commonTile;
            _rightTile = view._rightTile;
            _leftTile = view._leftTile;
            _topTile = view._topTile;
            _cornerBottomRight = view._cornerBottomRight;
            _cornerBottomleftt = view._cornerBottomleftt;
            _cornerUpRight = view._cornerUpRight;
            _cornerUpLeft = view._cornerUpLeft;
            _mapHeight = view._mapHeight;
            _mapWidth = view._mapWidth;
            _fillPercent = view._fillPercent;
            _smoothPercent = view._smoothPercent;
            _borders = view._borders;
            _map = new int[_mapWidth, _mapHeight];
        }
        public void Start()
        {
            FillMap();
            for (int i = 0; i < _smoothPercent; i++)
            {
                SmoothMap();
            }
            //DrawTiles();
            _marshController = new MarshingSquareController();
            _marshController.GenerateGrid(_map, 1);
            _marshController.DrawTiles(_tilemap, _tile);
        
        }

        public void FillMap()
        {
            for(int x = 0; x < _mapWidth; x++)
            {
                for( int y = 0; y < _mapHeight; y++)
                {
                    if (x == 0 || x == _mapWidth - 1 || y == 0 || y == _mapHeight -1)
                    {
                         if (_borders)
                         {
                            _map[x, y] = 1;
                         }
                    }
                    else
                    {
                        _map[x, y] = Random.Range(0, 100) < _fillPercent ? 1 : 0;
                    }
                }
            }
        }
        public void SmoothMap()
        {
            for(int x = 0; x < _mapWidth; x++)
            {
                for(int y = 0; y < _mapHeight; y++)
                {
                    int neighbour = GetNeighbour(x,y);
                    if (neighbour > 4) _map[x, y] = 1;
                    else if (neighbour < 4) _map[x, y] = 0;
                }
            }
        }
        public int GetNeighbour(int x, int y)
        {
            int neighbour = 0;
            for (int gridX = x-1; gridX <= x+1; gridX++)
            {
                for(int gridY = y-1; gridY <= y+1; gridY++)
                {
                    if (gridX >= 0 && gridX < _mapWidth && gridY >= 0 && gridY < _mapHeight)
                    {
                        if (gridX != x || gridY != y)
                        {
                            neighbour += _map[gridX, gridY];
                        }
                    }
                    else
                    {
                        neighbour++;
                    }
                }
            }
            return neighbour;
        }
        public void DrawTiles()
        {
            if (_map == null) return; 
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    if (_map[x,y] == 1)
                    {
                        Vector3Int tilePosition = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                        _tilemap.SetTile(tilePosition, _tile);
                    }
                }
            }
        }
        public void Clear()
        {
            _tilemap.ClearAllTiles();
            _snowTilemap.ClearAllTiles();
        }
        public void UseDecoratedTiles()
        {
            _tilemap.ClearAllTiles();
            _snowTilemap.ClearAllTiles();
            FillMap();
            for (int i = 0; i < _smoothPercent; i++)
            {
                SmoothMap();
            }
            for (int x = 1; x < _mapWidth -1; x++)
            {
                for (int y = 1; y < _mapHeight- 1; y++)
                {
                    if (_map[x, y] == 1)
                    {
                        if (_map[x + 1, y] == 1 && _map[x - 1, y] == 1 && _map[x, y - 1] == 1)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _tile);
                        }
                        else if (_map[x + 1, y] == 1 && _map[x - 1, y] == 1)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _topTile);
                        }
                        if (_map[x + 1, y] == 0 && _map[x, y + 1] == 1 && _map[x, y - 1] == 1)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _rightTile);
                        }
                        else if (_map[x + 1, y] == 0 && _map[x, y + 1] == 1 && _map[x, y - 1] == 0)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _cornerBottomRight);
                        }
                        else if (_map[x - 1, y] == 0 && _map[x, y + 1] == 1 && _map[x, y - 1] == 0)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _cornerBottomleftt);
                        }
                        else if (_map[x - 1, y] == 0 && _map[x, y + 1] == 1 && _map[x, y - 1] == 1)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _leftTile);
                        }
                        if (_map[x + 1, y] == 0 && _map[x, y + 1] == 0 && _map[x, y - 1] == 1)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _cornerUpRight);
                        }
                        else if (_map[x - 1, y] == 0 && _map[x, y + 1] == 0 && _map[x, y - 1] == 1)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _tilemap.SetTile(tilePos, _cornerUpLeft);
                        }
                        if (_map[x + 1, y] == 1 && _map[x - 1, y] == 1 && _map[x, y + 1] == 0)
                        {
                            Vector3Int tilePos = new Vector3Int(-_mapWidth / 2 + x, _mapHeight / 2 + y);
                            _snowTilemap.SetTile(tilePos, _snowTile);
                        }
                    }
                }
            }
        }
    }
}
