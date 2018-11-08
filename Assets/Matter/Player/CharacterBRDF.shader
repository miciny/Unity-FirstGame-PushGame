Shader "Gaea/CharacterBRDF" {
Properties {
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "grey" {}
 _AlphaTex ("Alpha Tex", 2D) = "black" {}
 _BRDFTex ("NdotL NdotH (RGBA)", 2D) = "white" {}
 _FlowLight ("Flow Light", 2D) = "black" {}
 _LightMask ("Light Mask", 2D) = "black" {}
 _LightSpeedX ("Speed X", Float) = 0
 _LightSpeedY ("Speed Y", Float) = 0
 _EmisColor ("Emissive Color", Color) = (1,1,1,1)
 _EmisRange ("Emissive Range", Range(0,2)) = 0
 _GlobalScale ("Global Scale", Float) = 1
}
SubShader { 
 LOD 200
 Tags { "QUEUE"="Transparent+10" "RenderType"="Opaque" }
 Pass {
  Name "FORWARD"
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "QUEUE"="Transparent+10" "RenderType"="Opaque" }
  Fog { Mode Off }
  Stencil {
   Ref 2
   Pass Replace
   ZFail Replace
  }
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  tmpvar_5 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = glstate_lightmodel_ambient.xyz;
  tmpvar_4 = tmpvar_9;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform sampler2D _BRDFTex;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_AlphaTex, xlv_TEXCOORD0.zw);
  highp vec2 tmpvar_6;
  tmpvar_6.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_6.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture2D (_FlowLight, tmpvar_6) * texture2D (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_8;
  lowp vec4 c_9;
  lowp vec2 tmpvar_10;
  tmpvar_10.x = ((dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz) * 0.5) + 0.5);
  tmpvar_10.y = dot (xlv_TEXCOORD2, normalize((_WorldSpaceLightPos0.xyz + xlv_TEXCOORD4)));
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_BRDFTex, tmpvar_10);
  highp vec3 tmpvar_12;
  tmpvar_12 = ((tmpvar_2 * (tmpvar_11.xyz + (tmpvar_5.x * tmpvar_11.w))) * _GlobalScale);
  c_9.xyz = tmpvar_12;
  c_9.w = tmpvar_5.x;
  c_1.w = c_9.w;
  c_1.xyz = (c_9.xyz + (tmpvar_2 * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  tmpvar_5 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = glstate_lightmodel_ambient.xyz;
  tmpvar_4 = tmpvar_9;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform sampler2D _BRDFTex;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_AlphaTex, xlv_TEXCOORD0.zw);
  highp vec2 tmpvar_6;
  tmpvar_6.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_6.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture (_FlowLight, tmpvar_6) * texture (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_8;
  lowp vec4 c_9;
  lowp vec2 tmpvar_10;
  tmpvar_10.x = ((dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz) * 0.5) + 0.5);
  tmpvar_10.y = dot (xlv_TEXCOORD2, normalize((_WorldSpaceLightPos0.xyz + xlv_TEXCOORD4)));
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_BRDFTex, tmpvar_10);
  highp vec3 tmpvar_12;
  tmpvar_12 = ((tmpvar_2 * (tmpvar_11.xyz + (tmpvar_5.x * tmpvar_11.w))) * _GlobalScale);
  c_9.xyz = tmpvar_12;
  c_9.w = tmpvar_5.x;
  c_1.w = c_9.w;
  c_1.xyz = (c_9.xyz + (tmpvar_2 * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
uniform sampler2D unity_Lightmap;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp vec2 tmpvar_5;
  tmpvar_5.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_5.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture2D (_FlowLight, tmpvar_5) * texture2D (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_7;
  c_1.xyz = (tmpvar_2 * (2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz));
  c_1.w = texture2D (_AlphaTex, xlv_TEXCOORD0.zw).x;
  c_1.xyz = (c_1.xyz + tmpvar_3);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
uniform sampler2D unity_Lightmap;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  highp vec2 tmpvar_5;
  tmpvar_5.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_5.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture (_FlowLight, tmpvar_5) * texture (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_7;
  c_1.xyz = (tmpvar_2 * (2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz));
  c_1.w = texture (_AlphaTex, xlv_TEXCOORD0.zw).x;
  c_1.xyz = (c_1.xyz + tmpvar_3);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  tmpvar_5 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = glstate_lightmodel_ambient.xyz;
  tmpvar_4 = tmpvar_9;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform sampler2D _BRDFTex;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_AlphaTex, xlv_TEXCOORD0.zw);
  highp vec2 tmpvar_6;
  tmpvar_6.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_6.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture2D (_FlowLight, tmpvar_6) * texture2D (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_8;
  lowp vec4 c_9;
  lowp vec2 tmpvar_10;
  tmpvar_10.x = ((dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz) * 0.5) + 0.5);
  tmpvar_10.y = dot (xlv_TEXCOORD2, normalize((_WorldSpaceLightPos0.xyz + xlv_TEXCOORD4)));
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_BRDFTex, tmpvar_10);
  highp vec3 tmpvar_12;
  tmpvar_12 = ((tmpvar_2 * (tmpvar_11.xyz + (tmpvar_5.x * tmpvar_11.w))) * _GlobalScale);
  c_9.xyz = tmpvar_12;
  c_9.w = tmpvar_5.x;
  c_1.w = c_9.w;
  c_1.xyz = (c_9.xyz + (tmpvar_2 * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  tmpvar_5 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = glstate_lightmodel_ambient.xyz;
  tmpvar_4 = tmpvar_9;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform sampler2D _BRDFTex;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_AlphaTex, xlv_TEXCOORD0.zw);
  highp vec2 tmpvar_6;
  tmpvar_6.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_6.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture (_FlowLight, tmpvar_6) * texture (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_8;
  lowp vec4 c_9;
  lowp vec2 tmpvar_10;
  tmpvar_10.x = ((dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz) * 0.5) + 0.5);
  tmpvar_10.y = dot (xlv_TEXCOORD2, normalize((_WorldSpaceLightPos0.xyz + xlv_TEXCOORD4)));
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture (_BRDFTex, tmpvar_10);
  highp vec3 tmpvar_12;
  tmpvar_12 = ((tmpvar_2 * (tmpvar_11.xyz + (tmpvar_5.x * tmpvar_11.w))) * _GlobalScale);
  c_9.xyz = tmpvar_12;
  c_9.w = tmpvar_5.x;
  c_1.w = c_9.w;
  c_1.xyz = (c_9.xyz + (tmpvar_2 * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

uniform highp vec4 _Time;
uniform highp vec4 _LightShadowData;
uniform sampler2D _ShadowMapTexture;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
uniform sampler2D unity_Lightmap;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp vec2 tmpvar_5;
  tmpvar_5.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_5.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture2D (_FlowLight, tmpvar_5) * texture2D (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_7;
  lowp float tmpvar_8;
  mediump float lightShadowDataX_9;
  highp float dist_10;
  lowp float tmpvar_11;
  tmpvar_11 = texture2DProj (_ShadowMapTexture, xlv_TEXCOORD3).x;
  dist_10 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = _LightShadowData.x;
  lightShadowDataX_9 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = max (float((dist_10 > (xlv_TEXCOORD3.z / xlv_TEXCOORD3.w))), lightShadowDataX_9);
  tmpvar_8 = tmpvar_13;
  c_1.xyz = (tmpvar_2 * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz), vec3((tmpvar_8 * 2.0))));
  c_1.w = texture2D (_AlphaTex, xlv_TEXCOORD0.zw).x;
  c_1.xyz = (c_1.xyz + tmpvar_3);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform highp vec4 _LightShadowData;
uniform sampler2D _ShadowMapTexture;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
uniform sampler2D unity_Lightmap;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  highp vec2 tmpvar_5;
  tmpvar_5.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_5.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture (_FlowLight, tmpvar_5) * texture (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_7;
  lowp float tmpvar_8;
  mediump float lightShadowDataX_9;
  highp float dist_10;
  lowp float tmpvar_11;
  tmpvar_11 = textureProj (_ShadowMapTexture, xlv_TEXCOORD3).x;
  dist_10 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = _LightShadowData.x;
  lightShadowDataX_9 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = max (float((dist_10 > (xlv_TEXCOORD3.z / xlv_TEXCOORD3.w))), lightShadowDataX_9);
  tmpvar_8 = tmpvar_13;
  c_1.xyz = (tmpvar_2 * min ((2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz), vec3((tmpvar_8 * 2.0))));
  c_1.w = texture (_AlphaTex, xlv_TEXCOORD0.zw).x;
  c_1.xyz = (c_1.xyz + tmpvar_3);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
varying highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  tmpvar_5 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = glstate_lightmodel_ambient.xyz;
  tmpvar_4 = tmpvar_9;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _Time;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform sampler2D _BRDFTex;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec3 xlv_TEXCOORD2;
varying lowp vec3 xlv_TEXCOORD3;
varying lowp vec3 xlv_TEXCOORD4;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_AlphaTex, xlv_TEXCOORD0.zw);
  highp vec2 tmpvar_6;
  tmpvar_6.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_6.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture2D (_FlowLight, tmpvar_6) * texture2D (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_8;
  lowp vec4 c_9;
  lowp vec2 tmpvar_10;
  tmpvar_10.x = ((dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz) * 0.5) + 0.5);
  tmpvar_10.y = dot (xlv_TEXCOORD2, normalize((_WorldSpaceLightPos0.xyz + xlv_TEXCOORD4)));
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_BRDFTex, tmpvar_10);
  highp vec3 tmpvar_12;
  tmpvar_12 = ((tmpvar_2 * (tmpvar_11.xyz + (tmpvar_5.x * tmpvar_11.w))) * _GlobalScale);
  c_9.xyz = tmpvar_12;
  c_9.w = tmpvar_5.x;
  c_1.w = c_9.w;
  c_1.xyz = (c_9.xyz + (tmpvar_2 * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
in vec4 _glesMultiTexCoord0;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_Scale;
uniform highp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out lowp vec3 xlv_TEXCOORD2;
out lowp vec3 xlv_TEXCOORD3;
out lowp vec3 xlv_TEXCOORD4;
out highp vec4 xlv_TEXCOORD5;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec3 tmpvar_4;
  lowp vec3 tmpvar_5;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  mat3 tmpvar_6;
  tmpvar_6[0] = _Object2World[0].xyz;
  tmpvar_6[1] = _Object2World[1].xyz;
  tmpvar_6[2] = _Object2World[2].xyz;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_6 * (normalize(_glesNormal) * unity_Scale.w));
  tmpvar_3 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = normalize((_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz));
  tmpvar_5 = tmpvar_8;
  highp vec3 tmpvar_9;
  tmpvar_9 = glstate_lightmodel_ambient.xyz;
  tmpvar_4 = tmpvar_9;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = tmpvar_3;
  xlv_TEXCOORD3 = tmpvar_4;
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_TEXCOORD5 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform lowp vec4 _WorldSpaceLightPos0;
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _BRDFTex;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in lowp vec3 xlv_TEXCOORD2;
in lowp vec3 xlv_TEXCOORD3;
in lowp vec3 xlv_TEXCOORD4;
in highp vec4 xlv_TEXCOORD5;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture (_AlphaTex, xlv_TEXCOORD0.zw);
  highp vec2 tmpvar_6;
  tmpvar_6.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_6.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture (_FlowLight, tmpvar_6) * texture (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_7;
  highp vec3 tmpvar_8;
  tmpvar_8 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_8;
  lowp float shadow_9;
  mediump float tmpvar_10;
  tmpvar_10 = texture (_ShadowMapTexture, xlv_TEXCOORD5.xyz);
  shadow_9 = tmpvar_10;
  highp float tmpvar_11;
  tmpvar_11 = (_LightShadowData.x + (shadow_9 * (1.0 - _LightShadowData.x)));
  shadow_9 = tmpvar_11;
  lowp vec4 c_12;
  lowp vec2 tmpvar_13;
  tmpvar_13.x = ((dot (xlv_TEXCOORD2, _WorldSpaceLightPos0.xyz) * 0.5) + 0.5);
  tmpvar_13.y = dot (xlv_TEXCOORD2, normalize((_WorldSpaceLightPos0.xyz + xlv_TEXCOORD4)));
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture (_BRDFTex, tmpvar_13);
  highp vec3 tmpvar_15;
  tmpvar_15 = ((tmpvar_2 * (tmpvar_14.xyz + (tmpvar_5.x * tmpvar_14.w))) * _GlobalScale);
  c_12.xyz = tmpvar_15;
  c_12.w = tmpvar_5.x;
  c_1.w = c_12.w;
  c_1.xyz = (c_12.xyz + (tmpvar_2 * xlv_TEXCOORD3));
  c_1.xyz = (c_1.xyz + tmpvar_3);
  _glesFragData[0] = c_1;
}



#endif"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES


#ifdef VERTEX

#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _Time;
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
uniform sampler2D unity_Lightmap;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_MainTex, xlv_TEXCOORD0.xy);
  highp vec2 tmpvar_5;
  tmpvar_5.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_5.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture2D (_FlowLight, tmpvar_5) * texture2D (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_7;
  lowp float shadow_8;
  lowp float tmpvar_9;
  tmpvar_9 = shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  highp float tmpvar_10;
  tmpvar_10 = (_LightShadowData.x + (tmpvar_9 * (1.0 - _LightShadowData.x)));
  shadow_8 = tmpvar_10;
  c_1.xyz = (tmpvar_2 * min ((2.0 * texture2D (unity_Lightmap, xlv_TEXCOORD2).xyz), vec3((shadow_8 * 2.0))));
  c_1.w = texture2D (_AlphaTex, xlv_TEXCOORD0.zw).x;
  c_1.xyz = (c_1.xyz + tmpvar_3);
  gl_FragData[0] = c_1;
}



#endif"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec4 _glesMultiTexCoord0;
in vec4 _glesMultiTexCoord1;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform highp vec4 _FlowLight_ST;
uniform highp vec4 _LightMask_ST;
out highp vec4 xlv_TEXCOORD0;
out highp vec4 xlv_TEXCOORD1;
out highp vec2 xlv_TEXCOORD2;
out highp vec4 xlv_TEXCOORD3;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_1.xy = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_1.zw = ((_glesMultiTexCoord0.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  tmpvar_2.xy = ((_glesMultiTexCoord0.xy * _FlowLight_ST.xy) + _FlowLight_ST.zw);
  tmpvar_2.zw = ((_glesMultiTexCoord0.xy * _LightMask_ST.xy) + _LightMask_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD3 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
uniform highp vec4 _Time;
uniform highp vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp float _GlobalScale;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _FlowLight;
uniform sampler2D _LightMask;
uniform lowp float _LightSpeedX;
uniform lowp float _LightSpeedY;
uniform highp vec3 _EmisColor;
uniform highp float _EmisRange;
uniform sampler2D unity_Lightmap;
in highp vec4 xlv_TEXCOORD0;
in highp vec4 xlv_TEXCOORD1;
in highp vec2 xlv_TEXCOORD2;
in highp vec4 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 c_1;
  lowp vec3 tmpvar_2;
  lowp vec3 tmpvar_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture (_MainTex, xlv_TEXCOORD0.xy);
  highp vec2 tmpvar_5;
  tmpvar_5.x = (xlv_TEXCOORD1.x + (_Time.x * _LightSpeedX));
  tmpvar_5.y = (xlv_TEXCOORD1.y + (_Time.x * _LightSpeedY));
  tmpvar_2 = (tmpvar_4.xyz + ((texture (_FlowLight, tmpvar_5) * texture (_LightMask, xlv_TEXCOORD1.zw)) * 2.0).xyz);
  highp vec3 tmpvar_6;
  tmpvar_6 = (tmpvar_2 * _GlobalScale);
  tmpvar_2 = tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = ((_EmisColor * _EmisRange) * tmpvar_4.w);
  tmpvar_3 = tmpvar_7;
  lowp float shadow_8;
  mediump float tmpvar_9;
  tmpvar_9 = texture (_ShadowMapTexture, xlv_TEXCOORD3.xyz);
  shadow_8 = tmpvar_9;
  highp float tmpvar_10;
  tmpvar_10 = (_LightShadowData.x + (shadow_8 * (1.0 - _LightShadowData.x)));
  shadow_8 = tmpvar_10;
  c_1.xyz = (tmpvar_2 * min ((2.0 * texture (unity_Lightmap, xlv_TEXCOORD2).xyz), vec3((shadow_8 * 2.0))));
  c_1.w = texture (_AlphaTex, xlv_TEXCOORD0.zw).x;
  c_1.xyz = (c_1.xyz + tmpvar_3);
  _glesFragData[0] = c_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES"
}
SubProgram "gles3 " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_ON" "DIRLIGHTMAP_OFF" }
"!!GLES3"
}
}
 }
 Pass {
  Tags { "QUEUE"="Transparent+10" "RenderType"="Opaque" }
  Fog { Mode Off }
  Stencil {
   Ref 2
   Comp NotEqual
   Pass Replace
   ZFail Replace
  }
Program "vp" {
SubProgram "gles " {
"!!GLES


#ifdef VERTEX

attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 glstate_matrix_projection;
varying highp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.zw = tmpvar_2.zw;
  mat3 tmpvar_3;
  tmpvar_3[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_3[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_3[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  mat2 tmpvar_4;
  tmpvar_4[0] = glstate_matrix_projection[0].xy;
  tmpvar_4[1] = glstate_matrix_projection[1].xy;
  tmpvar_1.xy = (tmpvar_2.xy + (((tmpvar_4 * (tmpvar_3 * normalize(_glesNormal)).xy) * tmpvar_2.z) * 0.002));
  gl_Position = tmpvar_1;
  xlv_COLOR = vec4(0.2, 0.2, 0.2, 1.0);
}



#endif
#ifdef FRAGMENT

varying highp vec4 xlv_COLOR;
void main ()
{
  mediump vec4 tmpvar_1;
  tmpvar_1 = xlv_COLOR;
  gl_FragData[0] = tmpvar_1;
}



#endif"
}
SubProgram "gles3 " {
"!!GLES3#version 300 es


#ifdef VERTEX

in vec4 _glesVertex;
in vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 glstate_matrix_projection;
out highp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = (glstate_matrix_mvp * _glesVertex);
  tmpvar_1.zw = tmpvar_2.zw;
  mat3 tmpvar_3;
  tmpvar_3[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_3[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_3[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  mat2 tmpvar_4;
  tmpvar_4[0] = glstate_matrix_projection[0].xy;
  tmpvar_4[1] = glstate_matrix_projection[1].xy;
  tmpvar_1.xy = (tmpvar_2.xy + (((tmpvar_4 * (tmpvar_3 * normalize(_glesNormal)).xy) * tmpvar_2.z) * 0.002));
  gl_Position = tmpvar_1;
  xlv_COLOR = vec4(0.2, 0.2, 0.2, 1.0);
}



#endif
#ifdef FRAGMENT

out mediump vec4 _glesFragData[4];
in highp vec4 xlv_COLOR;
void main ()
{
  mediump vec4 tmpvar_1;
  tmpvar_1 = xlv_COLOR;
  _glesFragData[0] = tmpvar_1;
}



#endif"
}
}
Program "fp" {
SubProgram "gles " {
"!!GLES"
}
SubProgram "gles3 " {
"!!GLES3"
}
}
 }
}
Fallback "Mobile/Diffuse"
}