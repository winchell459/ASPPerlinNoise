using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASP
{
    public static class Utility
    {
        public static int GetMaxInt(List<List<string>> set)
        {
            int max = int.MinValue;
            foreach (List<string> num in set)
            {
                if (max < int.Parse(num[0]))
                {
                    max = int.Parse(num[0]);
                }
            }

            return max;
        }




        public static List<T> GetSmallestRandomPermutation<T>(List<List<T>> permutations, bool remove)
        {

            int smallest = int.MaxValue;
            foreach (List<T> permutation in permutations)
            {
                if (smallest > permutation.Count) smallest = permutation.Count;
            }

            List<int> smallestIndices = new List<int>();
            for (int i = 0; i < permutations.Count; i += 1)
            {
                List<T> permutation = permutations[i];
                if (smallest == permutation.Count) smallestIndices.Add(i);
            }
            int rand = Random.Range(0, smallestIndices.Count);
            List<T> smallestPermutation = permutations[smallestIndices[rand]];
            if (remove) permutations.Remove(smallestPermutation);

            return smallestPermutation;
        }
        public static List<List<T>> GetPermutations<T>(List<T> indices)
        {
            return GetPermutations(indices.ToArray());
        }
        public static List<List<T>> GetPermutations<T>(T[] indices)
        {
            List<List<T>> permutations = new List<List<T>>();
            int permutationCount = (int)Mathf.Pow(2, indices.Length);
            for (int countI = 1; countI < permutationCount; countI += 1)
            {
                int count = countI;

                List<T> permutation = new List<T>();
                string binary = "";
                for (int i = 1; i < indices.Length + 1; i += 1)
                {
                    int power = (int)Mathf.Pow(2, indices.Length - i);

                    if (count / power == 1)
                    {
                        permutation.Add(indices[i - 1]);
                    }
                    binary += count / power;
                    count -= power * (count / power);
                }
                permutations.Add(permutation);
                //Debug.Log(binary);
            }
            return permutations;
        }
        public static Vector2Int GetRoomCoord(int roomID, int width, int height, int startID, bool invertY)
        {
            roomID -= startID;
            int x = roomID % width;
            int y = invertY ? height - roomID / width - 1 : roomID / width;

            return new Vector2Int(x, y);
        }
        public static Vector2Int GetRoomCoord(int roomID, int width, int startID)
        {
            roomID -= startID;
            int x = roomID % width;
            int y = roomID / width;
            return new Vector2Int(x, y);
        }

        public static Vector2Int GetRoomCoord(int roomID, int width)
        {

            return GetRoomCoord(roomID, width, 0);
        }
        

        public static int GetRoomID(Vector2Int coord, int width, int startID)
        {
            return coord.x + coord.y * width + startID;
        }
    }
}

