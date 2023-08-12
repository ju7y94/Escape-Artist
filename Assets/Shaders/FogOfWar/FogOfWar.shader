Shader "Custom/FogOfWar" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
        _MaskTex ("Mask Texture", 2D) = "white" {}
        _FogRadius ("FogRadius", Float) = 15.0
        _FogMaxRadius ("FogMaxRadius", Float) = 1.5
        _Player1_Pos ("_Player1_Pos", Vector) = (0,0,0,1)
        _Player2_Pos ("_Player2_Pos", Vector) = (0,0,0,1)
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4   _Color;
            float    _FogRadius;
            float    _FogMaxRadius;
            float4   _Player1_Pos;
            float4   _Player2_Pos;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float2 location : TEXCOORD1;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
            };

            float powerForPos(float4 pos, float2 nearVertex);

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv = v.uv;
                o.location = posWorld.xz;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 baseColor = tex2D(_MainTex, i.uv) * _Color;

                float alpha = (1.0 - (baseColor.a + powerForPos(_Player1_Pos, i.location) + powerForPos(_Player2_Pos, i.location)));

                return fixed4(baseColor.rgb, alpha);
            }

            //return 0 if (pos-nearVertex) > _FogRadius
            float powerForPos(float4 pos, float2 nearVertex)
            {
                float atten = clamp(_FogRadius - length(pos.xz - nearVertex.xy), 0.0, _FogRadius);
                return (1.0/_FogMaxRadius)*atten/_FogRadius;
            }

            ENDCG
        }
    }

    Fallback "Transparent/VertexLit"
}