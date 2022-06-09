using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASP
{
    public abstract class Map2D : Map
    {
        public float TileSpacing = 1.1f;
        public Vector2 TileOffset = Vector2.zero;
        public bool InvertY, InvertX;

        protected int maxWidth = 0;
        protected int maxheight = 0;
        protected int minWidth = int.MaxValue;
        protected int minHeight = int.MaxValue;

        protected abstract void clearTile(int x, int y);
        public virtual void ClearMap(Vector2Int min, Vector2Int max)
        {
            for (int x = min.x; x <= max.x; x += 1)
            {
                for (int y = min.y; y <= max.y; y += 1)
                {
                    //Debug.Log($"clearTile({x},{y})");
                    clearTile(x, y);
                }
            }
        }
        protected virtual T[,] setupMap<T>(Clingo.AnswerSet answerset, string widthKey, string heightKey, T[,] map)
        {
            maxWidth = 0;
            maxheight = 0;
            minWidth = int.MaxValue;
            minHeight = int.MaxValue;
            foreach (List<string> widths in answerset.Value[widthKey])
            {
                if (int.Parse(widths[0]) > maxWidth) maxWidth = int.Parse(widths[0]);
                if (int.Parse(widths[0]) < minWidth) minWidth = int.Parse(widths[0]);
            }
            foreach (List<string> h in answerset.Value[heightKey])
            {
                if (int.Parse(h[0]) > maxheight) maxheight = int.Parse(h[0]);
                if (int.Parse(h[0]) < minHeight) minHeight = int.Parse(h[0]);
            }
            if (map == null)
            {
                width = maxWidth;
                height = maxheight;
                map = new T[width, height];
            }
            else if (maxheight > height || maxWidth > width)
            {
                width = maxWidth > width ? maxWidth : width;
                height = maxheight > height ? maxheight : height;
                T[,] newMap = new T[width, height];
                for (int x = 0; x < map.GetUpperBound(0) + 1; x += 1)
                {
                    for (int y = 0; y < map.GetUpperBound(1) + 1; y += 1)
                    {
                        newMap[x, y] = map[x, y];
                    }
                }
                map = newMap;
                //ClearMap(new Vector2Int(minWidth - 1, minHeight - 1), new Vector2Int(maxWidth - 1, maxheight - 1));
            }

            return map;
        }

        protected void parseMapDimensions(Clingo.AnswerSet answerset, string widthKey, string heightKey)
        {
            foreach (List<string> widths in answerset.Value[widthKey])
            {
                if (int.Parse(widths[0]) > width) width = int.Parse(widths[0]);
            }
            foreach (List<string> h in answerset.Value[heightKey])
            {
                if (int.Parse(h[0]) > height) height = int.Parse(h[0]);
            }
        }

        override public void AdjustCamera()
        {
            Camera cam = Camera.main;
            float aspect = cam.aspect;
            float size = cam.orthographicSize;

            float tileSpacing = 1;
            float boardSizeHeight = height * tileSpacing / 2 + (tileSpacing - 1) / 2;
            float boardSizeWidth = width * tileSpacing / 2 + (tileSpacing - 1) / 2;

            float boardAspect = boardSizeWidth / boardSizeHeight;

            float boardSizeX = boardSizeWidth / aspect;
            float boardSize = aspect < boardAspect ? boardSizeX : boardSizeHeight;

            cam.orthographicSize = boardSize;

            float y = height / 2 * (1 + (tileSpacing - 1));
            float x = width / 2 * (1 + (tileSpacing - 1));
            if (width % 2 == 0) x -= (1 + (tileSpacing - 1)) / 2;
            if (height % 2 == 0) y -= (1 + (tileSpacing - 1)) / 2;



            cam.transform.position = new Vector3(x + TileOffset.x, y + TileOffset.y, cam.transform.position.z);
        }
    }
}

