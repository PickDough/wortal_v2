shader_type spatial;
render_mode cull_disabled, depth_test_disabled;

group_uniforms Border;
uniform float border_thickness : hint_range(0.01, 0.1) = 0.05;
uniform float opacity : hint_range(0.0, 1.0) = 1.0;
uniform vec3 line_color : source_color = vec3(1.0, 1.0, 1.0);

group_uniforms Wave;
uniform float speed : hint_range(0.0, 2.0) = 1.0;
uniform float height : hint_range(0.0, 100.0, 0.1) = 35.;
uniform float spread : hint_range(0.0, 100.0, 0.1) = 20.;

void vertex() {
	//MODELVIEW_MATRIX = VIEW_MATRIX * mat4(INV_VIEW_MATRIX[0],INV_VIEW_MATRIX[1],INV_VIEW_MATRIX[2],MODEL_MATRIX[3]);
}

void fragment() {

    float time_offset = -TIME * speed;

	float sy_up = cos(spread* (UV.x + time_offset)) / height;
	float sy_down = cos(spread* (UV.x - time_offset)) / height;
	float sx_right = cos(spread* (UV.y + time_offset)) / height;
	float sx_left = cos(spread* (UV.y - time_offset)) / height;

	if (UV.y < border_thickness + sy_up) {
        ALBEDO = line_color;
        ALPHA = opacity;
	} else if (1.0 - UV.y < border_thickness + sy_down) {
		ALBEDO = line_color;
        ALPHA = opacity;
	}
	else if (1.0 - UV.x < border_thickness + sx_right) {
  		ALBEDO = line_color;
        ALPHA = opacity;
	}
	else if (UV.x < border_thickness + sx_left) {
  		ALBEDO = line_color;
        ALPHA = opacity;
	} else {
		discard;
	}
}