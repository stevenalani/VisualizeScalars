#version 430

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;

out vec3 apos;
out vec3 anormal;
flat out int instanceID;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    anormal = aNormal;  
    apos = aPos + vec3(0.0,float(gl_InstanceID*10),0.0); 
    instanceID = gl_InstanceID;
    gl_Position = projection * view *model * vec4(apos, 1.0);
}