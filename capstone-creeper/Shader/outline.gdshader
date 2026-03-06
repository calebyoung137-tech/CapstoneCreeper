shader_type canvas_item;

uniform vec4 outline_color : source_color = vec4(1.0, 1.0, 0.0, 1.0);
uniform float outline_size = 1.0;

void fragment()
{
    vec4 tex = texture(TEXTURE, UV);
    float alpha = tex.a;

    float outline = 0.0;

    outline += texture(TEXTURE, UV + vec2( outline_size * TEXTURE_PIXEL_SIZE.x, 0)).a;
    outline += texture(TEXTURE, UV + vec2(-outline_size * TEXTURE_PIXEL_SIZE.x, 0)).a;
    outline += texture(TEXTURE, UV + vec2(0,  outline_size * TEXTURE_PIXEL_SIZE.y)).a;
    outline += texture(TEXTURE, UV + vec2(0, -outline_size * TEXTURE_PIXEL_SIZE.y)).a;

    outline += texture(TEXTURE, UV + vec2( outline_size * TEXTURE_PIXEL_SIZE.x,  outline_size * TEXTURE_PIXEL_SIZE.y)).a;
    outline += texture(TEXTURE, UV + vec2(-outline_size * TEXTURE_PIXEL_SIZE.x,  outline_size * TEXTURE_PIXEL_SIZE.y)).a;
    outline += texture(TEXTURE, UV + vec2( outline_size * TEXTURE_PIXEL_SIZE.x, -outline_size * TEXTURE_PIXEL_SIZE.y)).a;
    outline += texture(TEXTURE, UV + vec2(-outline_size * TEXTURE_PIXEL_SIZE.x, -outline_size * TEXTURE_PIXEL_SIZE.y)).a;

    if (alpha == 0.0 && outline > 0.0)
    {
        COLOR = outline_color;
    }
    else
    {
        COLOR = tex;
    }
}