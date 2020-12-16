#version 400
precision highp int;
precision highp float;

uniform highp sampler3D volume;
uniform vec3 voldimensions;
uniform float samplingrate;

in vec3 raydir;
flat in vec3 transviewpos;
out vec4 color;

vec2 intersect_box(vec3 orig, vec3 dir) {
	const vec3 box_min = vec3(0);
	const vec3 box_max = vec3(1);
	vec3 inv_dir = 1.0 / dir;
	vec3 tmin_tmp = (box_min - orig) * inv_dir;
	vec3 tmax_tmp = (box_max - orig) * inv_dir;
	vec3 tmin = min(tmin_tmp, tmax_tmp);
	vec3 tmax = max(tmin_tmp, tmax_tmp);
	float t0 = max(tmin.x, max(tmin.y, tmin.z));
	float t1 = min(tmax.x, min(tmax.y, tmax.z));
	return vec2(t0, t1);
}
// Pseudo-random number gen from
// http://www.reedbeta.com/blog/quick-and-easy-gpu-random-numbers-in-d3d11/
// with some tweaks for the range of values
float wang_hash(int seed) {
	seed = (seed ^ 61) ^ (seed >> 16);
	seed *= 9;
	seed = seed ^ (seed >> 4);
	seed *= 0x27d4eb2d;
	seed = seed ^ (seed >> 15);
	return float(seed % 2147483647) / float(2147483647);
}
void main(void) {
	vec3 ray_dir = normalize(raydir);
	vec2 t_hit = intersect_box(transviewpos, ray_dir);
	if (t_hit.x > t_hit.y) {
		discard;
	}
	t_hit.x = max(t_hit.x, 0.0);
	vec3 dt_vec = 1.0 / (vec3(voldimensions) * abs(ray_dir));
	float dt = samplingrate * min(dt_vec.x, min(dt_vec.y, dt_vec.z));
	float offset = wang_hash(int(gl_FragCoord.x + 600.0 * gl_FragCoord.y));
	vec3 p = transviewpos + (t_hit.x /*+ offset*/ * dt) * ray_dir;
	for (float t = t_hit.x; t < t_hit.y; t += dt) {
		vec4 val_color = texture(volume, p);
		// Opacity correction
		//val_color.w = 1.0 - pow(1.0 - val_color.w, samplingrate);
		color.xyz += (1.0 - color.w) * val_color.w * val_color.xyz;
		color.w += (1.0 - color.w) * val_color.w;
		if (color.a >= 0.95) {
			break;
		}
		p += ray_dir * dt;
	}
	///color = vec4(t_hit.x,t_hit.y,0,1);
}