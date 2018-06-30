using System.Collections.Generic;

/*
  my first algo :')

  First, will place the rooms of interest (entrance, chest, exit...)
  Then, will fill the rooms to link them
  At last, will randomly add some rooms inbetween
*/
public class NaiveAlgorithm: GenerationAlgorithm {

    public override List<List<RoomType>> GenerateMapLayout() {
        List<List<RoomType>> layout = CreateEmptyDungeon();

        // Should be on the same row.
        layout[0][0] = RoomType.NORMAL;
        layout[0][1] = RoomType.NORMAL;
        layout[0][2] = RoomType.ENTRANCE;

        return layout;
    }


}
