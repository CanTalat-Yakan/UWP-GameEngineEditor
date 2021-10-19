using Editor.Assets.Engine.Editor;
using Editor.Assets.Engine.Utilities;
using Windows.UI.Xaml.Controls;

namespace Editor.Assets.Engine
{
    class Engine_Main
    {
        public Engine_Input m_input;
        internal Engine_Time m_time;
        internal Engine_Scene m_scene;
        internal Engine_Renderer m_render;
        internal Engine_ImGui m_gui;

        internal Engine_Main(SwapChainPanel _swapChainPanel, TextBlock _textBlock)
        {
            m_render = new Engine_Renderer(_swapChainPanel);
            m_input = new Engine_Input();
            m_time = new Engine_Time();
            m_scene = new Engine_Scene();
            m_gui = new Engine_ImGui();

            m_scene.Awake();
            m_scene.Start();

            Windows.UI.Xaml.Media.CompositionTarget.Rendering += (s, e) =>
            {
                m_render.Clear();

                m_input.Update();

                m_scene.Update();
                m_scene.LateUpdate();

                m_input.LateUpdate();

                m_time.Update();

                m_render.SetSolid();
                m_scene.Render();
                m_render.SetWireframe();
                m_scene.Render();

                m_gui.Draw();

                m_render.Present();

                _textBlock.Text = m_time.m_profile;
                _textBlock.Text += "\n" + m_render.m_profile;
                _textBlock.Text += "\n" + m_scene.m_profile;
            };
        }
    }
}