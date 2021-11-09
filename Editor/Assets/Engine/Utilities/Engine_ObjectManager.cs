using System;
using System.Collections.Generic;
using Editor.Assets.Engine.Components;
using Editor.Assets.Engine.Editor;
using Editor.Assets.Engine.Helper;
using Vector2 = SharpDX.Vector2;
using Vector3 = SharpDX.Vector3;


namespace Editor.Assets.Engine.Utilities
{
    public enum EPrimitiveTypes { CUBE, SPHERE, PLANE, CYLINDER, CAPSULE }
    class MyList<T> : List<T>
    {
        public event EventHandler OnAdd;
        public new void Add(T item) // "new" to avoid compiler-warnings, because we're hiding a method from base-class
        {
            if (null != OnAdd) OnAdd(this, null);
            base.Add(item);
        }
    }
    internal class Engine_ObjectManager
    {
        internal MyList<Engine_Object> m_list = new MyList<Engine_Object>();
        internal Engine_Object m_sky;

        Engine_Material m_materialDefault;
        Engine_Material m_materialSky;
        Engine_Mesh m_meshSphere;
        Engine_Mesh m_meshCube;

        static readonly string SHADER_LIT = @"Assets//Engine//Resources//Lit.hlsl";
        static readonly string SHADER_UNLIT = @"Assets//Engine//Resources//Unlit.hlsl";
        static readonly string IMAGE_DEFAULT = @"Assets//Engine//Resources//Default.png";
        static readonly string IMAGE_SKY = @"Assets//Engine//Resources//SkyGradient.png";
        static readonly string OBJ_CUBE = @"Assets//Engine//Resources//Cube.obj";
        static readonly string OBJ_SPHERE = @"Assets//Engine//Resources//Sphere.obj";

        internal Engine_ObjectManager()
        {
            m_materialDefault = new Engine_Material(SHADER_LIT, IMAGE_DEFAULT);
            m_materialSky = new Engine_Material(SHADER_UNLIT, IMAGE_SKY);

            m_meshCube = new Engine_Mesh(Engine_ObjLoader.LoadFilePro(OBJ_CUBE));
            m_meshSphere = new Engine_Mesh(Engine_ObjLoader.LoadFilePro(OBJ_SPHERE));
        }


        internal Engine_Object CreateEmpty(string _name = "Entity")
        {
            Engine_Object gObject = new Engine_Object()
            {
                m_name = _name,
                m_material = m_materialDefault,
            };

            m_list.Add(gObject);
            return gObject;
        }

        internal Engine_Object CreatePrimitive(EPrimitiveTypes _type)
        {
            Engine_Object gObject = new Engine_Object();
            gObject.m_material = m_materialDefault;

            switch (_type)
            {
                case EPrimitiveTypes.CUBE:
                    gObject.m_mesh = m_meshCube;
                    gObject.m_name = "Cube";
                    break;
                case EPrimitiveTypes.SPHERE:
                    gObject.m_mesh = m_meshSphere;
                    gObject.m_name = "Sphere";
                    break;
                case EPrimitiveTypes.PLANE:
                    break;
                case EPrimitiveTypes.CYLINDER:
                    break;
                case EPrimitiveTypes.CAPSULE:
                    break;
                default:
                    break;
            }

            m_list.Add(gObject);
            return gObject;
        }
        internal Engine_Object CreatePrimitive(EPrimitiveTypes _type, Engine_Object _parent)
        {
            var gObject = CreatePrimitive(_type);
            gObject.m_parent = _parent;

            return gObject;
        }

        internal void CreateSky()
        {
            m_sky = new Engine_Object()
            {
                m_name = "Sky",
                m_mesh = m_meshSphere,
                m_material = m_materialSky,
            };

            m_sky.m_transform.m_scale = new Vector3(-1000, -1000, -1000);
        }


        internal void Render()
        {
        }
    }
}
