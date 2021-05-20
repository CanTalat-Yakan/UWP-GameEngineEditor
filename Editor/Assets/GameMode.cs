using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Assets.Control
{
    enum EGameMode
    {
        NONE,
        PLAYING,
        PAUSED
    }
    class GameMode
    {
        internal EGameMode m_playMode;
    }
}
