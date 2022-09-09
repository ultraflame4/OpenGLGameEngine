# OpenGLGameEngine
A attempt at a "simple" game engine by me.

## Features
- [ ] input controls system, one that allows for key remapping. example:
```csharp
var exampleInputSys = InputSystems.createSystem("Keyboard") // you can create your own input systems to group controls together
exampleInputSys.active=false // you can disable systems to disable groups of controls at the same time. 
                            // Eg. inventory is opened so u disable the walking control system.

//                                 Name of control    The default key (combinations) Type of control. 
var walkInput = CreateInputControl(name:"walkForward",default_key:InputKeys.W,type:InputType.Held)
walk.IsActive()
walk.OnActive
```
- [x] fullscreen
- [x] keyboard input