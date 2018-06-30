using System.Collections.Generic;

/*
  This is the algorithm to generate the map layout. Algo output is just a 2D array that store integers.
  The integers will determine what kind of room will fill the index.
 */
public abstract class GenerationAlgorithm {

    protected int columns = DungeonBuilder.COLS;
    protected int rows = DungeonBuilder.ROWS;

    public abstract List<List<RoomType>> GenerateMapLayout();

    protected List<List<RoomType>> CreateEmptyDungeon() {
        List<List<RoomType>> layout = new List<List<RoomType>>();

        for (int i = 0; i < columns; i++) {
            layout.Add(new List<RoomType>());
            for (int j = 0; j < rows; j++) {
                layout[i].Add(RoomType.NONE);
            }
        }

        return layout;
    }

}
