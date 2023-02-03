using UnityEngine;
using UnityEngine.Tilemaps;

namespace PlatformerMVC
{
    public class MarshingSquareController
    {
        private Tilemap _tilemap;
        private Tile _tile;
        private SquareGrid _grid;

        public void GenerateGrid(int[,] map, float squareSize)
        {
            _grid = new SquareGrid(map, squareSize);
        }
        public void DrawTile(bool active, Vector3 pos)
        {
            if (active)
            {
                Vector3Int tilePos = new Vector3Int((int)pos.x, (int)pos.y, 0);
                _tilemap.SetTile(tilePos, _tile);
            }
        }
        public void DrawTiles(Tilemap tilemapG, Tile ground)
        {
            if (_grid == null) return;
            _tile = ground;
            _tilemap = tilemapG;

            for (int x = 0; x < _grid.Squares.GetLength(0); x++)
            {
                for (int y = 0; y < _grid.Squares.GetLength(1); y++)
                {
                    DrawTile(_grid.Squares[x, y].TL.Active, _grid.Squares[x, y].TL.Position);
                    DrawTile(_grid.Squares[x, y].TR.Active, _grid.Squares[x, y].TR.Position);
                    DrawTile(_grid.Squares[x, y].BL.Active, _grid.Squares[x, y].BL.Position);
                    DrawTile(_grid.Squares[x, y].BR.Active, _grid.Squares[x, y].BR.Position);
                }
            }
        }
    }
    public class Node
    {
        public Vector3 Position;
        public Node(Vector3 position)
        {
            Position = position;
        }
    }
    public class ControlNode: Node
    {
        public bool Active;
        public ControlNode(Vector3 pos, bool active): base(pos)
        {
            Active = active;
        }
    }
    public class Square
    {
        public ControlNode TL, TR, BL, BR;
        public Square(ControlNode tl, ControlNode tr, ControlNode bl, ControlNode br)
        {
            TL = tl;
            TR = tr;
            BL = bl;
            BR = br;
        }
    }
    public class SquareGrid
    {
        public Square[,] Squares;
        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);

            float mapWidth = nodeCountX * squareSize;
            float mapHeight = nodeCountY * squareSize;

            float size = squareSize / 2;

            float mapMidWidth = -mapWidth / 2;
            float mapMidHeight = -mapHeight / 2;
            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 pos = new Vector3(mapMidWidth + x * squareSize + size, mapMidHeight + y * squareSize + size, 0);
                    controlNodes[x, y] = new ControlNode(pos, map[x, y] == 1);
                }
            }
            Squares = new Square[nodeCountX-1, nodeCountY-1];
           
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    Squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y], controlNodes[x + 1, y + 1], controlNodes[x, y]);
                }
            }
        }
    }
}
