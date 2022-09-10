Shader "Unlit/yabai"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            #define PI 3.14159265358979323846

            float2 rotate2D(float2 _st, float _angle){
                _st -= 0.5;
                _st =  mul(_st,float2x2(cos(_angle),-sin(_angle),
                            sin(_angle),cos(_angle)));
                _st += 0.5;
                return _st;
            }

            float2 tile(float2 _st, float _zoom){
                _st *= _zoom;
                return frac(_st);
            }

            float box(float2 _st, float2 _size, float _smoothEdges){
                _size = float2(0.5,0.5)-_size*0.5;
                float2 aa = _smoothEdges*0.5;
                float2 uv = smoothstep(_size,_size+aa,_st);
                uv *= smoothstep(_size,_size+aa,1.0-_st);
                return uv.x*uv.y;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                _Time.y += 1000;
                float2 st = i.uv;
                float3 color = 0.0;

                // Divide the space in 4
                st = tile(st,floor(sin(_Time.y + cos(_Time.y*PI))*4.));

                // Use a matrix to rotate the space 45 degrees
                st = rotate2D(st,pow(PI*0.25,sin(_Time.y*3.14)));

                // Draw a square
                st+=frac(st.x*_Time.y);
                color = box(st,float2(0.7,0.7),0.01);
               
                color *= float3(sin(_Time.y*3.14159)+0.1,sin(_Time.y*1.716)+0.1,cos(_Time.y/2.)*1.5+0.1);
               
                // color = float3(st,0.0);

                return float4(color,1.0);
            }


            
            ENDCG
        }
    }
}
