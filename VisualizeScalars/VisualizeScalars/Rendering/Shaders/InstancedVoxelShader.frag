#version 430 


out vec4 FragColor;
in vec3 apos;
in vec3 anormal;
flat in int instanceID;

layout(std430, binding = 2) buffer layer0
{
    vec3 data1[];
};
layout(std430, binding = 3) buffer layer1
{
    vec3 data2[];
};
layout(std430, binding = 4) buffer layer2
{
    vec3 data3[];
};
uniform float BufferCnt[3];
uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform float diffuseStrength;
uniform float specularStrength;
uniform vec3 viewpos;
uniform vec4 LayerColor[3];
uniform mat4 model;
uniform float radius;

float distanceFunc(int x, int y, float rad )
{

    float val = 0;
    for(int i = 0; i < BufferCnt[instanceID-1]; i++){
        vec3 indexesValue;
        if(instanceID == 1){
            indexesValue = data1[i];
        }else if(instanceID == 2){
            indexesValue = data2[i];
        }else if(instanceID == 3){
            indexesValue = data3[i];
        }
        
        if(indexesValue.z > 0){
            float diff = abs(length(vec2(x,y) - indexesValue.xy));
            if(diff <= rad){
                val = 1-clamp(diff/rad,0.0,1.0)*indexesValue.z;
                break;
            }
        }
    }
    return val;
}


void main()
{
    //float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;
  	
    // diffuse 
    vec3 norm = normalize((inverse(model) * vec4(anormal,1.0)).xyz);
    vec3 pos = (model * vec4(apos, 1.0)).xyz;
    vec3 lightDir = normalize(lightPos - pos);
    float diff = dot(norm, lightDir);
    vec3 diffuse = diffuseStrength*diff * lightColor;
    
    vec4 layerColor = LayerColor[instanceID];
    // specular
    vec3 viewDir = normalize(viewpos - pos);
    vec3 reflectDir = reflect(-lightDir, norm);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), specularStrength);
    vec3 specular = spec * lightColor;  
        
    vec3 result = (ambient + diffuse + specular) * layerColor.xyz;
    if(instanceID == 0){
        FragColor = vec4(result, layerColor.w );
    }else{
        float dist = distanceFunc(int(apos.x),int(apos.z),radius);
        FragColor = vec4(result,dist * layerColor.w );
    }
    
}

