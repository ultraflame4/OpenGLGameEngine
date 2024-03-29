﻿using System.Drawing;
using System.Numerics;
using GameEngineTest;
using NLog;
using OpenGLGameEngine;
using OpenGLGameEngine.Actors;
using OpenGLGameEngine.Core.Windowing;
using OpenGLGameEngine.Graphics.LowLevel;
using OpenGLGameEngine.Graphics.Rendering;
using OpenGLGameEngine.UI;
using OpenGLGameEngine.Universe;

var logger = LogManager.GetCurrentClassLogger();
Console.WriteLine("Hello World!");
Game.Init("Example Game", windowMode: WindowModes.Windowed, windowSize: (720, 720));

// GameWorld.GlobalShader = new Shader(new[] {
//         ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
//         ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
// });
Texture AddTestRenderTarget(World world)
{
        
    var renderTexture = Texture.CreateEmpty(720, 720, new TextureConfig());
    var testRenderTarget = new RenderTarget(renderTexture);
    testRenderTarget.ClearColor = GlColor.FromColor(Color.Red);
    world.AddActor(new TestCameraController() {
            renderTarget = testRenderTarget
    });
    return renderTexture;
}
var world = new World();
var renderTexture = AddTestRenderTarget(world);

world.AddActor(new TestCameraController());
var parent = world.AddActor(new TestObject(renderTexture));

var cube = PrimitiveShape.CreateCube();
cube.Mesh.SetTexture(Texture.FromFile("./CheckerboardMap.png", new TextureConfig()));
cube.transform.position = new Vector3(0, 2, 0);
world.AddActor(cube, parent.transform);


var canvasUi = world.AddActor(new CanvasRenderer());




WorldManager.LoadWorld(world);
Game.Start();

