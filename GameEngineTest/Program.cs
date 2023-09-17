﻿using System.Drawing;
using System.Numerics;
using NLog;
using OpenGLGameEngine;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Core.Drawing;
using OpenGLGameEngine.Core.Windowing;
using OpenGLGameEngine.Graphics.Camera;
using OpenGLGameEngine.Graphics.Mesh;
using OpenGLGameEngine.Universe;

var logger = LogManager.GetCurrentClassLogger();
Console.WriteLine("Hello World!");
Game.CreateMainWindow("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));

// GameWorld.GlobalShader = new Shader(new[] {
//         ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
//         ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
// });

var world = new World();
world.AddActor(new TestCameraController());
world.AddActor(new TestObject());
world.AddActor(PrimitiveShape.CreateCube());
WorldManager.LoadWorld(world);
Game.Start();


public class TestObject : MeshRenderer
{
    
    
    public override void Start()
    {
        Mesh = new Mesh();
        var texture = Texture.FromBitmap(new Bitmap("./CheckerboardMap.png"),new ());
        Mesh.SetVertices(
            new MeshVertex(new Vector3(-1f, 1f, 0f), Color.Red, new Vector2(0f, 1f)),
            new MeshVertex(new Vector3(1f, 1f, 0f), Color.Lime, new Vector2(1f, 1f)),
            new MeshVertex(new Vector3(1f, -1f, 0f), Color.Blue, new Vector2(1f, 0f)),
            new MeshVertex(new Vector3(-1f, -1f, 0f), Color.Blue, new Vector2(0f, 0f))
        );

        Mesh.SetTriangles(0, 2, 1, 0, 3, 2);
        Mesh.SetTexture(texture);

        // transform.scale = new Vector3(0.5f);
    }
    
}