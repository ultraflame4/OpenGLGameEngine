﻿using GLFW;
using NLog;
using OpenGL;
using OpenGLGameEngine;
using OpenGLGameEngine.Graphics;
using OpenGLGameEngine.Inputs;
using OpenGLGameEngine.Utils;

namespace GameEngineTest;

public class Program
{
    static Logger logger = LogManager.GetCurrentClassLogger();

    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        Game.Create("Example Game", windowMode: WindowModes.Windowed);


        Shader shader = new Shader(new[]{
                ShaderUtils.LoadShaderFromPath("./vertex.glsl", ShaderType.VertexShader),
                ShaderUtils.LoadShaderFromPath("./fragment.glsl", ShaderType.FragmentShader)
        });

        float[] v1 = {// contains both position and color
                -1f, 1f, 0f, 1f, 0f, 0f,
                1f, 1f, 0f, 0f, 1f, 0f,
                1f, -1f, 0f, 0f, 0f, 1f,
                -1f, -1f, 0.0f, 0f, 0f, 1f
        };
        uint[] t1 = {
                0, 2, 1,
        };
        uint[] t2 = {
                0, 3, 2
        };
        
        var o = new VertexRenderObject(v1, 6, t1);
        o.SetVertexAttrib(0, 3, 0);
        o.SetVertexAttrib(1, 3, 3);
        var o2 = new VertexRenderObject(v1, 6, t2);
        o2.SetVertexAttrib(0, 3, 0);
        o2.SetVertexAttrib(1, 3, 3);

        
        Game.GameLoopDraw += () =>
        {
            shader.Use();
            o.Draw();
            o2.Draw();
        };
        Game.Run();
    }
}