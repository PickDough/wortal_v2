using Godot;

namespace wortal_v2.ui.screens.game_over;

public partial class GameOverScreen : Control
{
    private async void RestartPressed()
    {
        var screenManager = ScreenManager.Instance(this);
        screenManager.ClearScreen();
        
        screenManager.GetTree().ReloadCurrentScene();
    }
    
    private void QuitPressed()
    {
        GetTree().Quit();
    }
}