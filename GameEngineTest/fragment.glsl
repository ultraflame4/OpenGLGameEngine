#version 330 core
out vec4 FragColor;
in vec4 vertexColor;
in vec2 TexCoord;
uniform float test;
uniform sampler2D ourTexture;

void main()
{
    //FragColor = texture(ourTexture,TexCoord);
    FragColor = vec4(0.0, test, 1.0, 1.0);
} 