using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace Editor.Assets.Engine.Utilities
{
    class Engine_Time
    {
        internal string m_profile = "";

        internal static double m_time, m_delta;
        int m_fps, m_lastFPS;
        Stopwatch watch = new Stopwatch();
        DateTime now = DateTime.Now;

        internal void Update()
        {
            m_delta = watch.ElapsedMilliseconds * 0.001;
            m_time += m_delta;
            ++m_lastFPS;

            if (now.Second != DateTime.Now.Second)
            {
                m_fps = m_lastFPS;
                m_lastFPS = 0;
                now = DateTime.Now;
                m_profile = m_delta.ToString() + "\n" + m_fps.ToString();
            }


            watch.Restart();
        }
    }
}
