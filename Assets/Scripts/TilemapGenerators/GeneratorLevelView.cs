using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlatformerMVC
{
    public class GeneratorLevelView : MonoBehaviour
    {
        public Tilemap _tilemap;
        public Tilemap _snowTilemap;
        public Tile _snowTile;
        public Tile _commonTile;
        public Tile _rightTile;
        public Tile _leftTile;
        public Tile _topTile;
        public Tile _cornerBottomRight;
        public Tile _cornerBottomleftt;
        public Tile _cornerUpRight;
        public Tile _cornerUpLeft;
        public int _mapHeight;
        public int _mapWidth;

        [Range(0,100)] public int _fillPercent;
        [Range(0,50)] public int _smoothPercent;

        public bool _borders;
        public int[,] _map;
    }
}
