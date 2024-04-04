Shader "Unlit/S_PlaneMap"
{
    Properties
    {
		[Header(Main)][Space(5)]
		_Color ("Color", Color) = (1,1,1,1)
		_Lighting ("Lighting",  float) = 1
		_MainTex ("MainTexture", 2D) = "white" {}
		_Tex2 ("Texture2", 2D) = "white" {}
		_Tex3 ("Texture3", 2D) = "white" {}
		_Tex4 ("Texture4", 2D) = "white" {}
		_TexM ("Mask", 2D) = "white" {}
		[Header(Specular)][Space(5)]
		_SpecColor("Specular Color", color) = (1, 0.87, 0.68, 0.1)
        _SpecTex("Specular Texture", 2D) = "white" {}
		[Header(Advanced Options)][Space(5)]
		[Header(xyz_Direction     w_Gloss)][Space]
		_LightVector("Light Vector", vector) = (-1.91, -1.95, -2.82, 0.27)
		_LightMask("Light Mask", vector) = (4, 6, 2, 2.5)
		_UVPos("UV Position", vector) =(0,0,0,0) 
		_UVRotation("UV Rotation", Range(-3.14, 3.14)) = -0.785
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    SubShader
    {
		Tags { 
			"Queue" = "Transparent" 
			"IgnoreProjector" = "True" 
			//"RenderType" = "Transparent" 
			"PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
		}
        //LOD 100

        Pass
        {			
            Lighting Off
			//ZWrite Off
			ZTest LEqual
			Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile_instancing
            #pragma multi_compile _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
			#include "StarUnionFunction.cginc"
            //#include "UnityCG.cginc"


            float4 _MainTex_ST;
			sampler2D _Tex2;
			float4 _Tex2_ST;
			sampler2D _Tex3;
			float4 _Tex3_ST;
			sampler2D _Tex4;
			float4 _Tex4_ST;
			sampler2D _TexM;
			float4 _TexM_ST;
			float _Lighting;
			half4 _SpecColor;
            sampler2D _SpecTex;
            half4 _SpecTex_ST;
            half4 _LightVector;
			half4 _LightMask;
			float4 _UVPos;
			float _UVRotation;

            float param[676];

            struct a2v_mine
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float3 normal : NORMAL;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f_mine
            {
                float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;				
				float4 uv2 : TEXCOORD1;
				float4 uv3 : TEXCOORD2;
				float4 uv4 : TEXCOORD3;
				float2 uvM : TEXCOORD4;
				float4 color : COLOR;
				half3 worldPos : TEXCOORD5;
                half3 normalDir : TEXCOORD6;
				UNITY_VERTEX_OUTPUT_STEREO
                
            };



            v2f_mine vert (a2v_mine IN, uint vid : SV_VertexID)// 顶点 ID，必须为 uint)
            {
                v2f_mine OUT;
				UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

            #ifdef UNITY_INSTANCING_ENABLED
                IN.vertex.xy *= _Flip;
            #endif
                OUT.uv.xy = TRANSFORM_TEX(IN.texcoord0, _MainTex)+_UVPos.xy;
				OUT.uv.zw = TRANSFORM_TEX(IN.texcoord0, _SpecTex);
				//OUT.uv.zw = TRANSFORM_TEX(IN.texcoord0, _MainTex);
				OUT.uv2.xy = TRANSFORM_TEX(IN.texcoord0, _Tex2)+_UVPos.xy;
				OUT.uv2.zw = TRANSFORM_TEX(IN.texcoord0, _SpecTex);
				//OUT.uv2.zw = TRANSFORM_TEX(IN.texcoord0, _Tex2);
				OUT.uv3.xy = TRANSFORM_TEX(IN.texcoord0, _Tex3)+_UVPos.xy;
				OUT.uv3.zw = TRANSFORM_TEX(IN.texcoord0, _SpecTex);
				//OUT.uv3.zw = TRANSFORM_TEX(IN.texcoord0, _Tex3);
				OUT.uv4.xy = TRANSFORM_TEX(IN.texcoord0, _Tex4)+_UVPos.xy;
				OUT.uv4.zw = TRANSFORM_TEX(IN.texcoord0, _SpecTex);
				//OUT.uv4.zw = TRANSFORM_TEX(IN.texcoord0, _Tex4);
				OUT.uvM.xy = TRANSFORM_TEX(IN.texcoord0, _TexM)+_UVPos.xy/2;
				IN.vertex.y = IN.vertex.y + param[vid];
				OUT.pos = UnityObjectToClipPos(IN.vertex);
				OUT.worldPos = mul(unity_ObjectToWorld, IN.vertex).xyz;
                OUT.normalDir = UnityObjectToWorldNormal(IN.normal);
				OUT.color = IN.color * _RendererColor;



            #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
			#endif
                //UNITY_TRANSFER_FOG(o,o.vertex);

                //取出传递过来的数据
                //vector data = param[vid];
                //o.vertex = ObjietToClipPos(data);
                return OUT;
            }

            float4 frag (v2f_mine IN) : COLOR //SV_Target,
            {
				//main
                float4 col1 = tex2D(_MainTex, RotateUV(IN.uv.xy, _UVRotation));
				float4 col2 = tex2D(_Tex2, RotateUV(IN.uv2.xy, _UVRotation));
				float4 col3 = tex2D(_Tex3, RotateUV(IN.uv3.xy, _UVRotation));
				float4 col4 = tex2D(_Tex4, RotateUV(IN.uv4.xy, _UVRotation));
			    float4 mask = tex2D(_TexM, RotateUV(IN.uvM.xy, _UVRotation));
				//mask
				float3 Lmask= lerp(lerp(lerp(_LightMask.x,_LightMask.y,mask.r),_LightMask.z,mask.g),_LightMask.w,mask.b);
				float4 col = lerp(lerp(lerp(col1,col2,mask.r),col3,mask.g),col4,mask.b) * IN.color;
				//float4 maskHl = (Lmask.r+Lmask.g+Lmask.b)/3;//mask去色
				float3 Highlight = dot(col.rgb,float3(0.33,0.33,0.34));//去色的另一种写法
				Highlight = pow(Highlight,3) * Lmask;
				half3 lightDir = normalize(-_LightVector.xyz);
                half3 viewDir = normalize(_WorldSpaceCameraPos.xyz - IN.worldPos);
                half3 h = normalize(lightDir + viewDir);
				fixed nh = saturate(dot(normalize(IN.normalDir), h));

				//specular
                //half3 specTex = tex2D(_SpecTex, RotateUV(IN.uv.zw, _UVRotation)).rgb;
				fixed specRange = exp2(_LightVector.w * 10.0 + 1.0);
				half3 specular = pow(max(0, nh), specRange) * (_SpecColor.a * 5) * _SpecColor.rgb * Highlight.rgb;

				//final
				float4 final;
				final.rgb = col.rgb * _Color.rgb + specular;
				final.rgb *= _Lighting;
				final.a = col.a * _Color.a;

				return final;
            }
            ENDCG
        }
    }
}