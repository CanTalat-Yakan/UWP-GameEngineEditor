using System;
using System.Diagnostics;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Time
    {
        double m_time, m_delta;
        int m_frames, m_fps, m_lastFPS;
        Stopwatch watch = new Stopwatch();
        DateTime now = DateTime.Now;

        internal void Update()
        {
            Windows.UI.Xaml.Media.CompositionTarget.Rendering += (s, e) =>
            {
                m_time += watch.ElapsedMilliseconds;
                ++m_lastFPS;

                if (now.Second != DateTime.Now.Second)
                {
                    m_fps = m_lastFPS;
                    m_lastFPS = 0;
                    now = DateTime.Now;
                }

                if (m_frames % 24 == 0)
                    m_delta = Math.Floor(watch.Elapsed.TotalMilliseconds);


                watch.Restart();
            };
        }
    }
}
