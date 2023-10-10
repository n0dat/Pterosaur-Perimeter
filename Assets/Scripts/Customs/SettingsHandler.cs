using System;
using System.IO;
using IniParser;
using IniParser.Model;

public class SettingsHandler {
    private const string Filename = "settings.ini";
    private readonly IniData data;
    private readonly FileIniDataParser parser;

    public SettingsHandler() {
        createFile(Filename);
        
        parser = new FileIniDataParser();
        data = parser.ReadFile(Filename);
        
        if (!data.Sections.ContainsSection("Settings"))
            data.Sections.AddSection("Settings");
    }

    private static void createFile(string filePath) {
        if (!File.Exists(filePath))
            File.Create(filePath);
    }

    public void writeSettings(bool showFps, bool playAudio, int targetFps, int audioLevel) {
        writeShowFps(showFps);
        writePlayAudio(playAudio);
        writeTargetFps(targetFps);
        writeAudioLevel(audioLevel);
        
        if (File.Exists(Filename))
            parser.WriteFile(Filename, data);
    }

    private void writeShowFps(bool showFps) {
        if (!data["Settings"].ContainsKey("showFps"))
            data["Settings"].AddKey("showFps", (showFps ? 1 : 0).ToString());
        else
            data["Settings"]["showFps"] = (showFps ? 1 : 0).ToString();
        
        saveChanges();
    }

    private void writePlayAudio(bool playAudio) {
        if (!data["Settings"].ContainsKey("playAudio"))
            data["Settings"].AddKey("playAudio", (playAudio ? 1 : 0).ToString());
        else
            data["Settings"]["playAudio"] = (playAudio ? 1 : 0).ToString();
        
        saveChanges();
    }

    private void writeTargetFps(int targetFps) {
        if (!data["Settings"].ContainsKey("targetFps"))
            data["Settings"].AddKey("targetFps", targetFps.ToString());
        else
            data["Settings"]["targetFps"] = targetFps.ToString();
        
        saveChanges();
    }

    private void writeAudioLevel(int audioLevel) {
        if (!data["Settings"].ContainsKey("audioLevel"))
            data["Settings"].AddKey("audioLevel", audioLevel.ToString());
        else
            data["Settings"]["audioLevel"] = audioLevel.ToString();
        
        saveChanges();
    }

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

    private int readTargetFps() {
        if (!data["Settings"].ContainsKey("targetFps"))
            return 60;

        if (!int.TryParse(data["Settings"]["targetFps"], out var fps)) return 60;
        return fps is < 1 or > 60 ? 60 : fps;
    }

    private int readAudioLevel() {
        if (!data["Settings"].ContainsKey("audioLevel"))
            return 100;

        if (!int.TryParse(data["Settings"]["audioLevel"], out var audioLevel)) return 100;
        return audioLevel is < 0 or > 100 ? 100 : audioLevel;
    }
}