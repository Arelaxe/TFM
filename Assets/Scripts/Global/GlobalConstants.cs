using System.Collections.Generic;
public static class GlobalConstants
{
    // Tags
    public static string TagPlayer = "Player";
    public static string TagPlayerInit = "PlayerInit";
    public static string TagPassage = "Passage";
    public static string TagVirtualCamera = "VCam";
    public static string TagMusicScene = "MusicScene";
    public static string TagDeviceManager = "DeviceManager";

    // Layers
    public static int LayerIntTerrenal = 6;
    public static int LayerIntSpiritual = 7;
    public static string LayerInteractable = "Interactable";

    // Sorting layers
    public static string SortingLayerForeground = "Foreground";

    // Paths
    public static string PathBoundaries = "/Level Structure/Boundaries";
    public static string PathDynamicObjectsRoot = "/Level Structure/Dynamic Objects";

    // Folders
    public static string ResourcesBaseDataFolder = "Data";
    public static string ResourcesItemDataFolder = "Inventory";

    // Action maps
    public static string ActionMapPlayer = "Player";

    // Control Schemes
    public static string ControlSchemeKeyboard = "Keyboard&Mouse";
    public static string ControlSchemeGamepad = "Gamepad";
    
    // Cannot Save Scenes
    public static List<string> CannotSaveScenes = new() { "TutorialScene", "IntroScene", "Ogawa Boss", "PreEndingScene" };
}
