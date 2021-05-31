using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Editor.Assets.Control
{
    internal enum EGameMode
    {
        NONE,
        PLAYING,
        PAUSED
    }
    internal class Control_GameMode
    {
        internal EGameMode m_playMode;

        View_Main m_main;
        AppBarToggleButton m_play;
        AppBarToggleButton m_pause;
        AppBarButton m_forward;
        AppBarButton m_repeat;
        AppBarButton m_kill;
        TextBlock m_status;
        Control_Output m_output;

        internal Control_GameMode(View_Main main, AppBarToggleButton play, AppBarToggleButton pause, AppBarButton forward, AppBarButton repeat, AppBarButton kill, TextBlock status, Control_Output _output)
        {
            m_main = main;
            m_play = play;
            m_pause = pause;
            m_forward = forward;
            m_repeat = repeat;
            m_kill = kill;
            m_status = status;
            m_output = _output;
        }


        void SetStatusAppBarButtons(bool _b)
        {
            m_playMode = _b ? EGameMode.PLAYING : EGameMode.NONE;

            m_pause.IsEnabled = _b;
            m_forward.IsEnabled = _b;
            m_repeat.IsEnabled = _b;
            m_kill.IsEnabled = _b;
            m_pause.IsChecked = false;

            m_play.Label = _b ? "Stop" : "Play";
            m_play.Icon = _b ? new SymbolIcon(Symbol.Stop) : new SymbolIcon(Symbol.Play);
        }
        void SetStatus(string _s)
        {
            m_status.Text = _s;
        }

        internal void Play()
        {
            if (m_playMode == EGameMode.NONE)
                if (m_output.m_ClearPlay.IsChecked.Value)
                    m_output.ClearOutput();

            SetStatusAppBarButtons(m_play.IsChecked.Value);

            SetStatus(m_play.IsChecked.Value ? "Now Playing..." : "Stopped Gamemode");
        }
        internal void Pause()
        {
            m_playMode = m_pause.IsChecked.Value ? EGameMode.PAUSED : EGameMode.PLAYING;

            SetStatus(m_pause.IsChecked.Value ? "Paused Gamemode" : "Continued Gamemode");
        }
        internal void Forward()
        {
            if (m_playMode != EGameMode.PAUSED)
                return;

            m_output.Log("Stepped Forward..");

            SetStatus("Stepped Forward");
        }
        internal void Repeat()
        {
            if (m_output.m_ClearPlay.IsChecked.Value)
                m_output.ClearOutput();

            m_playMode = EGameMode.PLAYING;

            m_pause.IsChecked = false;
            m_play.IsChecked = true;

            m_play.Label = "Stop";
            m_play.Icon = new SymbolIcon(Symbol.Stop);

            SetStatus("Restarted Gamemode");
        }
        internal void Kill()
        {
            m_play.IsChecked = false;

            SetStatusAppBarButtons(false);

            SetStatus("Killed Process of GameInstance!");
        }

    }
}
