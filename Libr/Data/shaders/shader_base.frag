﻿#version 330

in vec2 TexCoord;
uniform sampler2D textureSampler;
out vec4 outColor;

void main()
{
	outColor = texture(textureSampler, TexCoord);
}
