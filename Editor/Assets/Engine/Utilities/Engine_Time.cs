using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Time
    {
        internal string m_profile = "";

        internal static double m_time, m_delta;
        internal static Stopwatch m_watch = new Stopwatch();
        int m_fps, m_lastFPS;
        DateTime m_now = DateTime.Now;

        internal void Update()
        {
            m_delta = m_watch.ElapsedMilliseconds * 0.001;
            m_time += m_delta;
            ++m_lastFPS;

            if (m_now.Second != DateTime.Now.Second)
            {
                m_fps = m_lastFPS;
                m_lastFPS = 0;
                m_now = DateTime.Now;
                m_profile = m_delta.ToString() + "ms" + "\n" + m_fps.ToString() + "FPS";
            }


            m_watch.Restart();
        }
    }
}
