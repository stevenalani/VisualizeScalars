#version 430

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout(location = 2) in vec2 aTexCoord;

out vec2 texCoord;
out vec3 apos;
out vec3 anormal;
out vec4 fragPosLightSpace;
flat out int instanceID;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float yOffset;
uniform mat4 lightSpaceMatrix;

void main()
{
    fragPosLightSpace = lightSpaceMatrix * vec4(aPos, 1.0);
    anormal = mat3(transpose(inverse(model))) * aNormal;  
    apos = aPos + vec3(0.0,float(gl_InstanceID * yOffset),0.0); 
    instanceID = gl_InstanceID;
    gl_Position = projection * view *model * vec4(apos, 1.0);
}