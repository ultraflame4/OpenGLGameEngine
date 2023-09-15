# OpenGLGameEngine V2
An attempt at a "simple" game engine by me, so that i can learn about game engine and opengl :) .

This is the second 2 of the engine, which will include major changes into how objects in scene are handled.
The entire ECS system will be removed and replaced with a better one that makes more sense and hopefully easier to use.

This repository consists of 2 main projects:

1. [GameEngineTest](GameEngineTest) - This project is a test project used to test the engine and some of its features. Click on hyperlink for more info
2. [OpenGLGameEngine](OpenGLGameEngine) - The main engine that is being developed
   - The OpenGLGameEngine project is further splitted into many sub-projects
    1. [OpenGLGameEngine.Inputs](OpenGLGameEngine.Inputs) - The input system used by the engine<br/>
    2. [OpenGLGameEngine.Core](OpenGLGameEngine.Core) - The engine core, which deals with low <br/>
      level stuff like glfw and opengl.
   3. ~~[OpenGLGameEngine.ECS](OpenGLGameEngine.ECS) - The basic Entity Component System used by the engine<br/>~~ It has been removed and will be included with OpenGLGameEngine
   4. [OpenGLGameEngine](OpenGLGameEngine) - The main project itself. This combines the projects above and provides the user with a simple api to use the engine.

