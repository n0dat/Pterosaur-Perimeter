using System;
using System.IO;
using IniParser;
using IniParser.Model;

// UNUSED

public class SettingsHandler {
    private const string Filename = "settings.ini";
    private readonly IniData data;
    private readonly FileIniDataParser parser;
    // Sets the setting by create a file and parsing that data
    public SettingsHandler() {
        createFile(Filename);
        
        parser = new FileIniDataParser();
        data = parser.ReadFile(Filename);
        
        if (!data.Sections.ContainsSection("Settings"))
            data.Sections.AddSection("Settings");
    }
    // creates a file 
    private static void createFile(string filePath) {
        if (!File.Exists(filePath))
            File.Create(filePath);
    }
    // Writes the setting 
    public void writeSettings(bool showFps, bool playAudio, int targetFps, int audioLevel) {
        writeShowFps(showFps);
        writePlayAudio(playAudio);
        writeTargetFps(targetFps);
        writeAudioLevel(audioLevel);
        
        if (File.Exists(Filename))
            parser.WriteFile(Filename, data);
    }
    //Writes and shows the value of Fps and then saves those string values to a file 
    private void writeShowFps(bool showFps) {
        if (!data["Settings"].ContainsKey("showFps"))
            data["Settings"].AddKey("showFps", (showFps ? 1 : 0).ToString());
        else
            data["Settings"]["showFps"] = (showFps ? 1 : 0).ToString();
        
        saveChanges();
    }
    // Writes the play audios file to a value and then save those string values to a file
    private void writePlayAudio(bool playAudio) {
        if (!data["Settings"].ContainsKey("playAudio"))
            data["Settings"].AddKey("playAudio", (playAudio ? 1 : 0).ToString());
        else
            data["Settings"]["playAudio"] = (playAudio ? 1 : 0).ToString();
        
        saveChanges();
    }
    // writes the target values for Frams Per Second and saves those values
    private void writeTargetFps(int targetFps) {
        if (!data["Settings"].ContainsKey("targetFps"))
            data["Settings"].AddKey("targetFps", targetFps.ToString());
        else
            data["Settings"]["targetFps"] = targetFps.ToString();
        
        saveChanges();
    }
    // Writes the audio level and saves those values to the file
    private void writeAudioLevel(int audioLevel) {
        if (!data["Settings"].ContainsKey("audioLevel"))
            data["Settings"].AddKey("audioLevel", audioLevel.ToString());
        else
            data["Settings"]["audioLevel"] = audioLevel.ToString();
        
        saveChanges();
    }
    // saves the changes of the file
    private void saveChanges() {
        if (File.Exists(Filename))
            parser.WriteFile(Filename, data);
    }

    public Tuple<bool, bool, int, int> readSettings() {
        return new Tuple<bool, bool, int, int>(readShowFps(), readPlayAudio(), readTargetFps(), readAudioLevel());
    }

    private bool readShowFps() {
        if (!data["Settings"].ContainsKey("showFps"))
            return false;
        
        return string.CompareOrdinal(data["Settings"]["showFps"], "1") == 0;
    }
     
    private bool readPlayAudio() {
        if (!data["Settings"].ContainsKey("playAudio"))
            return false;
        
        return string.CompareOrdinal(data["Settings"]["playAudio"], "1") == 0;
    }
    // return an value of 1 - 60 for frams per seconds
    private int readTargetFps() {
        if (!data["Settings"].ContainsKey("targetFps"))
            return 60;

        if (!int.TryParse(data["Settings"]["targetFps"], out var fps)) return 60;
        return fps is < 1 or > 60 ? 60 : fps;
    }
    // return a value of 0 - 100 for the audio sound
    private int readAudioLevel() {
        if (!data["Settings"].ContainsKey("audioLevel"))
            return 100;

        if (!int.TryParse(data["Settings"]["audioLevel"], out var audioLevel)) return 100;
        return audioLevel is < 0 or > 100 ? 100 : audioLevel;
    }
}
