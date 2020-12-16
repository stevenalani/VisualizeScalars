#version 430 


out vec4 FragColor;

uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform float diffuseStrength;
uniform float specularStrength;
uniform vec3 viewpos;
uniform vec4 LayerColor[6];
uniform mat4 model;

in vec3 apos;
in vec3 anormal;
flat in int instanceID;



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
    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewpos - apos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32);
    vec3 specular = specularStrength * spec * lightColor;  
        
    vec3 result = (ambient + diffuse + specular) * vec3(1,0,0);
    FragColor = vec4(result, 1.0);
}

