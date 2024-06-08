using NFramework;

namespace TenCrush
{
    public class GameSound : SingletonMono<GameSound>
    {
        public void PlayButtonClickSFX() => SoundManager.I.PlaySFXResource(Define.SoundName.SFX_BUTTON_CLICK);

        public void PlayBGM() => SoundManager.I.PlayMusicResource(Define.SoundName.BGM_MUSIC_1, loop: true);

        public void PlaySFX(string audioClipPath) => SoundManager.I.PlaySFXResource(audioClipPath);
    }
}
