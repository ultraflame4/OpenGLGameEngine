# OpenGLGameEngine

A attempt at a "simple" game engine by me.

## Features

- [ ] input controls system, one that allows for key remapping. example:

```csharp
var example = Game.createInputSystem(name:"Keyboard") // you can create your own input systems to group controls together
example.active=false // you can disable systems to disable groups of controls at the same time. 
                            // Eg. inventory is opened so u disable the walking control system.
 
var walkInput = example.addControl(
                                    name:"walkForward", // Name of control. (display name?)
                                    default_key:InputKeys.W, // The default key (or combinations of them using enums)
                                    type:InputType.Held // Type of control, Held, Pressed, Released.
                                                        //determines when the control will be active
                                    invert:false // Whether to invert to control/active state. Eg, if type is Held,
                                                 // control will be constantly active until input is pressed
                                    )

walk.IsActive() // returns true if active
walk.OnActive // An event, fired when active. Eg. when key is pressed and type is Held, event will constantly fire
```

- [x] fullscreen
- [x] keyboard input