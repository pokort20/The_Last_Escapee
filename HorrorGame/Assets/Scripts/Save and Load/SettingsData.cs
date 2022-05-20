/// Settings data class
/**
    This class serves as a structure, encapsuling 
    the variables required for the game settings.
*/

[System.Serializable]
public class SettingsData
{
    public float volume;
    public float brightness;

    public SettingsData(float _volume, float _brightness)
    {
        brightness = _brightness;
        volume = _volume;
    }
}
