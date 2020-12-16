#version 400

layout (location = 0) in vec3 pos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform vec3 viewpos;
uniform vec3 volscale;

out vec3 raydir;
flat out vec3 transviewpos;

void main()
{
	vec3 volume_translation = vec3(0.5) - volscale * 0.5;
    gl_Position = projection * view * vec4(pos * volscale + volume_translation, 1);
	transviewpos = (viewpos - volume_translation) / volscale;
	raydir = pos - transviewpos;
} 