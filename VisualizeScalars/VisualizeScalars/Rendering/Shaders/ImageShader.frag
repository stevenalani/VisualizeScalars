#version 400
out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform float diffuseStrength;
uniform float specularStrength;
uniform sampler2D tex;
uniform vec3 viewpos;


in vec3 apos;
in vec3 anormal;
in vec2 texCoord;

void main()
{
    //float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;
  	
    // diffuse 
    vec3 norm = normalize(anormal);
    vec3 lightDir = normalize(lightPos - apos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;
    
    // specular
    //float specularStrength = 0.5;
    vec3 viewDir = normalize(viewpos - apos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    
    
    vec3 result = (ambient + diffuse) * texture(tex,texCoord).xyz;
    FragColor = vec4(result, 1.0);
}
