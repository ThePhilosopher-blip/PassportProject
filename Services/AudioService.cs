using Plugin.Maui.Audio;

namespace PassportMD.Services;

public class AudioService
{
    private IAudioManager? _manager;
    private IAudioPlayer? _musicPlayer;

    private byte[]? _tapSound;
    private byte[]? _successSound;
    private byte[]? _warningSound;
    private byte[]? _saveSound;
    private byte[]? _deleteSound;
    private byte[]? _ambientMusic;

    public bool SoundEnabled { get; set; } = true;
    public bool MusicEnabled { get; set; } = true;

    private IAudioManager Manager => _manager ??= AudioManager.Current;

    private void EnsureSounds()
    {
        _tapSound ??= SoundGenerator.CreateTapSound();
        _successSound ??= SoundGenerator.CreateSuccessChime();
        _warningSound ??= SoundGenerator.CreateWarningTone();
        _saveSound ??= SoundGenerator.CreateSaveSound();
        _deleteSound ??= SoundGenerator.CreateDeleteSound();
    }

    private readonly List<IAudioPlayer> _activePlayers = new();

    private void PlaySound(byte[] wavData)
    {
        if (!SoundEnabled) return;
        try
        {
            _activePlayers.RemoveAll(p => !p.IsPlaying);

            var stream = new MemoryStream(wavData);
            var player = Manager.CreatePlayer(stream);
            _activePlayers.Add(player);
            player.Play();
        }
        catch { }
    }

    public void PlayTap() { EnsureSounds(); PlaySound(_tapSound!); }
    public void PlaySuccess() { EnsureSounds(); PlaySound(_successSound!); }
    public void PlayWarning() { EnsureSounds(); PlaySound(_warningSound!); }
    public void PlaySave() { EnsureSounds(); PlaySound(_saveSound!); }
    public void PlayDelete() { EnsureSounds(); PlaySound(_deleteSound!); }

    public void StartMusic()
    {
        if (!MusicEnabled || _musicPlayer != null) return;
        try
        {
            _ambientMusic ??= SoundGenerator.CreateAmbientLoop();
            var stream = new MemoryStream(_ambientMusic);
            _musicPlayer = Manager.CreatePlayer(stream);
            _musicPlayer.Loop = true;
            _musicPlayer.Volume = 0.12;
            _musicPlayer.Play();
        }
        catch { }
    }

    public void StopMusic()
    {
        try
        {
            _musicPlayer?.Stop();
            _musicPlayer?.Dispose();
            _musicPlayer = null;
        }
        catch { }
    }

    public void ToggleMusic()
    {
        if (_musicPlayer != null)
        {
            StopMusic();
            MusicEnabled = false;
        }
        else
        {
            MusicEnabled = true;
            StartMusic();
        }
    }
}
