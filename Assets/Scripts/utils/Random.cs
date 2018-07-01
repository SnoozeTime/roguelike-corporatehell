/*
  Gather all the random functions used in the game here
 */
using System.Collections.Generic;

namespace GameUtils {

    public class Random {

        private static System.Random rnd = new System.Random();

        /*
          return a random integer in the given range. Lower bound is inclusive,
          upper bound is exclusive.
         */
        public static int Randint(int lowerInclusive, int upperExclusive) {
            return rnd.Next(lowerInclusive, upperExclusive);
        }

        /*
          Return a random element from the list
         */
        public static E Choice<E>(IList<E> list) {
            int index = rnd.Next(list.Count);
            return list[index];
        }
    }

}
