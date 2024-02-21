#version 330

layout(location = 0) in vec3 aPosition;
layout(location = 2) in vec2 aTextureCoord;
out vec2 TexCoord;

void main()
{
	TexCoord = aTextureCoord;

	gl_Position = vec4(aPosition,1.0);
}