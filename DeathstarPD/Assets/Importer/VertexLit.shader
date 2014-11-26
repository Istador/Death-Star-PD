Shader "Custom/VertexLit" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_TColor ("Team Color", Color) = (0.5,0.5,0.5,1)


	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp


// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	
	
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb  * (atten * 2);
	c.a = 0;
	return c;
}



float4 _Color;
float4 _TColor;

struct Input {
	float2 uv_MainTex : TEXCOORD0;
	float4 color : COLOR;
};

void surf (Input IN, inout SurfaceOutput o) {
	half4 c;
	if(IN.color[1] == 1 && IN.color[2] == 0.0f){
		c = _TColor * _Color;
	}else{
		c = IN.color * _Color;
	}
	o.Albedo = c.rgb;
	o.Alpha = c.a;
}
ENDCG

	} 

	Fallback "Diffuse"
} 
