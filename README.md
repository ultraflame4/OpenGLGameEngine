# OpenGLGameEngine
An attempt at a "simple" game engine by me, so that i can learn about game engine and opengl :) .

This repository consists of 2 main projects:

1. [GameEngineTest](GameEngineTest) - This project is a test game (well not really a game) used to test the engine
2. [OpenGLGameEngine](OpenGLGameEngine) - The main engine that is being developed
   - The OpenGLGameEngine project is further splitted into many sub-projects
    2. [OpenGLGameEngine.Inputs](OpenGLGameEngine.Inputs) - The input system used by the engine<br/>
    1. [OpenGLGameEngine.Core](OpenGLGameEngine.Core) - The engine core, which deals with low <br/>
      level stuff like glfw and opengl.
   3. [OpenGLGameEngine.ECS](OpenGLGameEngine.ECS) - The basic Entity Component System used by the engine<br/>
   4. [OpenGLGameEngine](OpenGLGameEngine) - The main package itself. This combines the packages above and provides the user with a simple api to use the engine.
