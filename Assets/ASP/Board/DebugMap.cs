using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ASP
{
    public class DebugMap : Map2D
    {
        public DebugPixel PixelPrefab;
        private DebugPixel[,] map;
        [SerializeField] MapKey<Color> mapKey;
        public float PixelSpacing = 1.1f;

        protected override void clearTile(int x, int y)
        {
            Debug.Log($"clearTile({x},{y}) {map.GetUpperBound(0)},{map.GetUpperBound(1)}");
            if (map[x, y] != null) map[x, y].gameObject.SetActive(false);
        }
        override public void DisplayMap(Clingo.AnswerSet answerset)
        {
            DisplayMap(answerset, mapKey);
        }

        public void DisplayMap(Clingo.AnswerSet answerset, MapKey<Color> mapKey)
        {
            DisplayMap(answerset, mapKey.widthKey, mapKey.heightKey, mapKey.pixelKey, mapKey.xIndex, mapKey.yIndex, mapKey.pixelTypeIndex, mapKey.dict);
        }
        public void DisplayMap(Clingo.AnswerSet answerset, string widthKey, string heightKey, string pixelKey, int xIndex, int yIndex, int pixelTypeIndex, MapObjectKey<Color> colorDict)
        {

            map = setupMap(answerset, widthKey, heightKey, map);
            //ClearMap(new Vector2Int(minWidth - 1, minHeight - 1), new Vector2Int(maxWidth - 1, maxheight - 1));
            foreach (List<string> pixelASP in answerset.Value[pixelKey])
            {
                int x = int.Parse(pixelASP[xIndex]) - 1;
                int y = int.Parse(pixelASP[yIndex]) - 1;

                string pixelType = pixelASP[pixelTypeIndex];

                //Debug.Log($"pixelKey : {pixelKey} | pixelTypeIndex : {pixelTypeIndex}");
                if (map[x, y] == null)
                {
                    DebugPixel pixel = Instantiate(PixelPrefab, transform).GetComponent<DebugPixel>();
                    pixel.SetPixel(x * PixelSpacing, y * PixelSpacing, colorDict[pixelType]);
                    pixel.AddNote(pixelASP);
                    map[x, y] = pixel;
                }
                else
                {
                    map[x, y].SetPixel(x * PixelSpacing, y * PixelSpacing, colorDict[pixelType]);
                    map[x, y].AddNote(pixelASP);
                    map[x, y].gameObject.SetActive(true);
                }

            }
        }

        override public void AdjustCamera()
        {
            Debug.Log("Adjusting MapPixel");
            Camera cam = Camera.main;
            float aspect = cam.aspect;
            float size = cam.orthographicSize;

            float boardSizeHeight = height * PixelSpacing / 2 + (PixelSpacing - 1) / 2;
            float boardSizeWidth = width * PixelSpacing / 2 + (PixelSpacing - 1) / 2;

            float boardAspect = boardSizeWidth / boardSizeHeight;

            float boardSizeX = boardSizeWidth / aspect;
            float boardSize = aspect < boardAspect ? boardSizeX : boardSizeHeight;

            cam.orthographicSize = boardSize;

            float y = height / 2 * (1 + (PixelSpacing - 1));
            float x = width / 2 * (1 + (PixelSpacing - 1));
            if (width % 2 == 0) x -= (1 + (PixelSpacing - 1)) / 2;
            if (height % 2 == 0) y -= (1 + (PixelSpacing - 1)) / 2;



            cam.transform.position = new Vector3(x, y, cam.transform.position.z);
        }
    }

}

