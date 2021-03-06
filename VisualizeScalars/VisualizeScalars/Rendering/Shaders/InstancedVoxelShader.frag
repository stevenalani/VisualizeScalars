#version 430 

out vec4 FragColor;
in vec3 apos;
in vec3 anormal;
in vec4 fragPosLightSpace;
flat in int instanceID;

uniform highp sampler3D textures;
uniform highp sampler2D shadowMap;
uniform vec3 texDimensions;


uniform vec3 lightPos;
uniform vec3 lightColor;
uniform float ambientStrength;
uniform float diffuseStrength;
uniform float specularStrength;
uniform vec3 viewpos;
uniform vec4 layer0Color;
uniform mat4 model;

float ShadowCalculation(vec4 fragPosLightSpace)
{
    // perform perspective divide
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    // transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;
    // get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
    float closestDepth = texture(shadowMap, projCoords.xy).r; 
    // get depth of current fragment from light's perspective
    float currentDepth = projCoords.z;
    // calculate bias (based on depth map resolution and slope)
    vec3 normal = normalize(anormal);
    vec3 lightDir = normalize(lightPos - vec3(model * vec4(apos, 1.0)));
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    // check whether current frag pos is in shadow
    // float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;
    // PCF
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth  ? 1.0 : 0.0;        
        }    
    }
    shadow /= 9.0;
    
    // keep the shadow at 0.0 when outside the far_plane region of the light's frustum.
    if(projCoords.z > 1.0)
        shadow = 0.0;
        
    return shadow;
}



void main()
{
    vec3 ambient = ambientStrength * lightColor;
  	

    vec3 pos = (model * vec4(apos, 1.0)).xyz;
    vec3 lightDir = normalize(lightPos - pos);
    float diff = dot(anormal, lightDir);
    vec3 diffuse = diffuseStrength*diff * lightColor;
    
    vec4 layerColor = layer0Color;

    vec3 viewDir = normalize(viewpos - pos);
    vec3 reflectDir = reflect(-lightDir, anormal);  
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), specularStrength);
    vec3 specular = spec * lightColor;  
    float shadow = ShadowCalculation(fragPosLightSpace); 
    vec3 result = (ambient + (1.0 - shadow) * (diffuse + specular)) * layerColor.xyz;
    if(instanceID == 0){
        FragColor = vec4(result, layerColor.w );
    }else{
        FragColor = texture(textures,vec3(apos.x/texDimensions.x,apos.z/texDimensions.y,(1 + 2*(instanceID-1))/(2*texDimensions.z)));
    }
    
}

