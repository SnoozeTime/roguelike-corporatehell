public static class GeometryUtils {

    public static int compareWithOffset(float coord, float otherCoord, float offset) {

        if (coord < otherCoord - offset) {
            return -1;
        } else if (coord > otherCoord + offset) {
            return 1;
        }

        return 0;
    }
}
