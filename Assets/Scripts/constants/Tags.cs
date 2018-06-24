using System.ComponentModel;

public enum Tags {
    // This will contain the bullet we shoot. useful to avoid putting too much
    // stuff in the main object hierarchy
    [DescriptionAttribute("BulletContainer")]
    BULLET_CONTAINER = 0,

    [DescriptionAttribute("Player")]
    PLAYER = 1,

    // The generated map. Can be needed to detect what doors are opened...
    [DescriptionAttribute("Dungeon")]
    DUNGEON = 2,
}
