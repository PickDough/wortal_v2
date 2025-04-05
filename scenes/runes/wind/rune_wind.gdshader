// Author's github: cptchuckles/tornado-particles.gdshader
shader_type particles;

uniform float tangent_velocity = 0.1;
uniform float upward_speed = 1.0;
uniform float base_radius = 0.3;
uniform sampler2D profile : repeat_disable;

const vec3 flatten = vec3(1.0, 0.0, 1.0);

uint hash(uint x) {
	x = ((x >> uint(16)) ^ x) * uint(73244475);
	x = ((x >> uint(16)) ^ x) * uint(73244475);
	x = (x >> uint(16)) ^ x;
	return x;
}

void start() {
	if (RESTART_POSITION) {
		uint seed = hash(NUMBER + uint(1) + RANDOM_SEED);
		float theta = mod(float(seed), PI*2.0);

		// Create initial position in local space
		vec3 local_pos = vec3(cos(theta), 0, sin(theta)) * base_radius;

		// Transform to world space
		mat3 emission_basis = mat3(EMISSION_TRANSFORM[0].xyz, EMISSION_TRANSFORM[1].xyz, EMISSION_TRANSFORM[2].xyz);
		vec3 world_pos = emission_basis * local_pos;

		TRANSFORM[3].xyz = EMISSION_TRANSFORM[3].xyz + world_pos;
	}
	if (RESTART_CUSTOM) {
		CUSTOM = vec4(0.0);
		CUSTOM.xyz = EMISSION_TRANSFORM[3].xyz;
	}
}

void process() {
	CUSTOM.w += DELTA;
	float t = CUSTOM.w;
	float s = texture(profile, vec2(.8*t/LIFETIME, 2.0)).r;

	// Convert current position to local space
	mat3 emission_basis = mat3(EMISSION_TRANSFORM[0].xyz, EMISSION_TRANSFORM[1].xyz, EMISSION_TRANSFORM[2].xyz);
	vec3 local_pos = inverse(emission_basis) * (TRANSFORM[3].xyz - CUSTOM.xyz);

	// Calculate movement in local space
	float v = tangent_velocity * DELTA / (s*2.0);
	float current_angle = atan(local_pos.z, local_pos.x);
	float new_angle = mod(current_angle + v, 2.0*PI);

	// Keep the original radius but scale it with the profile
	float current_radius = length(local_pos * flatten);
	float target_radius = base_radius * s;

	// Create new position in local space
	vec3 new_local_pos = vec3(
		cos(new_angle) * target_radius,
		local_pos.y + upward_speed * DELTA,
		sin(new_angle) * target_radius
	);

	// Transform back to world space
	vec3 world_pos = emission_basis * new_local_pos;
	TRANSFORM[3].xyz = CUSTOM.xyz + world_pos;
}