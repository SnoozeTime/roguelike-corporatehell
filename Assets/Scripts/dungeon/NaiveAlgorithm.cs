using System.Collections.Generic;
using UnityEngine;

/*
  my first algo :')

  First, will place the rooms of interest (entrance, chest, exit...)
  Then, will fill the rooms to link them
  At last, will randomly add some rooms inbetween
*/
public class NaiveAlgorithm: GenerationAlgorithm {

    private int minimumDistance = 1;
    private int maximumDistance = 3;
    private int numberOfRooms = 5;

    public override List<List<RoomType>> GenerateMapLayout() {
        List<List<RoomType>> layout = CreateEmptyDungeon();

        // Entrance is always at the center :)
        Vector2Int entranceIndex = new Vector2Int(
            (int) Mathf.Round(columns/2),
            (int) Mathf.Round(rows/2)
            );
        layout[entranceIndex.x][entranceIndex.y] = RoomType.ENTRANCE;

        // Place the exit
        Vector2Int exitIndex = PickAvailableIndex(layout, entranceIndex);
        layout[exitIndex.x][exitIndex.y] = RoomType.EXIT;

        Vector2Int chestIndex = PickAvailableIndex(layout, entranceIndex);
        layout[chestIndex.x][chestIndex.y] = RoomType.CHEST;

        Vector2Int shopIndex = PickAvailableIndex(layout, entranceIndex);
        layout[shopIndex.x][shopIndex.y] = RoomType.SHOP;

        // Fill blanks.
        FillRoomsInBetween(layout, entranceIndex, exitIndex);
        FillRoomsInBetween(layout, entranceIndex, chestIndex);
        FillRoomsInBetween(layout, entranceIndex, shopIndex);

        // Add some randomness
        AddRoomsRandomly(layout);

        return layout;
    }

    private void FillRoomsInBetween(List<List<RoomType>> layout,
                                    Vector2Int startIndex,
                                    Vector2Int endIndex) {
        int minX = (int) Mathf.Min(startIndex.x, endIndex.x);
        int maxX = (int) Mathf.Max(startIndex.x, endIndex.x);
        int minY = (int) Mathf.Min(startIndex.y, endIndex.y);
        int maxY = (int) Mathf.Max(startIndex.y, endIndex.y);

        for (int x = minX; x <= maxX; x++) {
            if (layout[x][startIndex.y] == RoomType.NONE) {
                layout[x][startIndex.y] = RoomType.NORMAL;
            }
        }

        for (int y = minY; y <= maxY; y++) {
            if (layout[endIndex.x][y] == RoomType.NONE) {
                layout[endIndex.x][y] = RoomType.NORMAL;
            }
        }

    }

    private void AddRoomsRandomly(List<List<RoomType>> layout) {
        List<Vector2Int> currentRoomIndices = new List<Vector2Int>();

        for (int x = 0; x < layout.Count; x++) {
            for (int y = 0; y < layout[x].Count; y++) {
                if (layout[x][y] != RoomType.NONE) {
                    currentRoomIndices.Add(new Vector2Int(x, y));
                }
            }
        }

        int totalNumberOfRooms = numberOfRooms + currentRoomIndices.Count;
        while (currentRoomIndices.Count < totalNumberOfRooms) {
            Vector2Int fromIndex = GameUtils.Random.Choice(currentRoomIndices);

            List<Vector2Int> availableIndices = new List<Vector2Int>();
            if (fromIndex.x > 0) {
                availableIndices.Add(new Vector2Int(fromIndex.x-1, fromIndex.y));
            }

            if (fromIndex.x < DungeonBuilder.COLS - 1) {
                availableIndices.Add(new Vector2Int(fromIndex.x+1, fromIndex.y));
            }

            if (fromIndex.y > 0) {
                availableIndices.Add(new Vector2Int(fromIndex.x, fromIndex.y-1));
            }

            if (fromIndex.y < DungeonBuilder.ROWS - 1) {
                availableIndices.Add(new Vector2Int(fromIndex.x, fromIndex.y+1));
            }

            currentRoomIndices.Add(GameUtils.Random.Choice(availableIndices));
        }
    }

    /*
      Pick a random index in the current layout. The index will be a given distance
      farther from a given point
     */
    private Vector2Int PickIndexRandomly(Vector2Int referenceIndex) {
        int distance = GameUtils.Random.Randint(minimumDistance, maximumDistance);
        int distanceX = GameUtils.Random.Randint(-distance, distance);

        int distanceYSquare = distance * distance - distanceX * distanceX;
        int distanceY;
        if (GameUtils.Random.Randint(0, 1) == 0) {
            distanceY = (int) Mathf.Round(Mathf.Sqrt(distanceYSquare));
        } else {
            distanceY = -(int) Mathf.Round(Mathf.Sqrt(distanceYSquare));
        }

        return (referenceIndex - new Vector2Int(distanceX, distanceY));
    }

    private Vector2Int PickAvailableIndex(List<List<RoomType>> layout, Vector2Int referenceIndex) {
        int retry = 0;
        int maxAvailableRoom = DungeonBuilder.COLS * DungeonBuilder.ROWS;
        // Add bounds condition
        while (retry < maxAvailableRoom) {

            Vector2Int potentialIndex = PickIndexRandomly(referenceIndex);

            if (potentialIndex.x >= 0 && potentialIndex.x < DungeonBuilder.COLS) {
                if (potentialIndex.y >= 0 && potentialIndex.y < DungeonBuilder.ROWS) {
                    if (layout[potentialIndex.x][potentialIndex.y] == RoomType.NONE) {
                        return potentialIndex;
                    }
                }
            }
            retry++;
        }

        // Maybe we picked the same wrong index a lot of time but anyway, if we go there
        // it means that the empty map is too small or that we want to create too many
        // rooms.
        throw new System.ArgumentException("There is no available index. Check the code.");
    }
}
