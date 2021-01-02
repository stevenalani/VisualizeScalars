#version 400

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;
 

out vec3 apos;
out vec3 anormal;
out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;


void main()
{
    texCoord = aTexCoord;
    apos = (model * vec4(aPos, 1.0)).xyz;
    anormal = mat3(transpose(inverse(model))) * aNormal;  
    gl_Position = projection * view * model * vec4(aPos, 1.0);
}