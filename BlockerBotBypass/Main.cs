using GDWeave;
using GDWeave.Godot.Variants;
using GDWeave.Godot;
using GDWeave.Modding;

namespace GDWeave.Sample;

public class Mod : IMod {
    public Config Config;

    
    public Mod(IModInterface modInterface) {
        this.Config = modInterface.ReadConfig<Config>();
        modInterface.RegisterScriptMod(new BlockerBotBypass());
        modInterface.Logger.Information("[Blocker Bot Bypass] Loaded!");

    }

    public void Dispose() {
        // Cleanup anything you do here
    }
}
