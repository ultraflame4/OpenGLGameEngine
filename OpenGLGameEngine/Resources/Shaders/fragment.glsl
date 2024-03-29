﻿#version 330 core
out vec4 FragColor;
in vec4 vertexColor;
in vec2 TexCoord;

uniform sampler2D ourTexture;

void main()
{
    FragColor = texture(ourTexture,TexCoord) * vertexColor;
    if (FragColor.a == 0) discard;
    //FragColor = vec4(0.0, test, 1.0, 1.0);
} 