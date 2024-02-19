Shader "Unlit/testUV"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2g
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float2 posWs : TEXCOORD1;
            };

            struct g2f
            {
                float2 uv : TEXCOORD0;
                float2 uvTriPos : TEXCOORD1;
                float3 uvScale : TEXCOORD3;
                uint triangleID : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            // 最高2位表示分支
            // 2~4位表示旋转
            // 4~22 表示河道宽度
            StructuredBuffer<uint> TriTypeBuffer;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2g vert(appdata v)
            {
                v2g o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.posWs = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            [maxvertexcount(3)]
            void geom(triangle v2g input[3], uint triangleID : SV_PrimitiveID, inout TriangleStream<g2f> OutputStream)
            {
                float3 scale = 0;
                int offset = (TriTypeBuffer[triangleID] >> 18) % 4;


                for (int _ = 0; _ < 2 - offset; _++)
                {
                    v2g temp = input[0];
                    input[0] = input[2];
                    input[2] = input[1];
                    input[1] = temp;
                }

                scale.x = length(input[0].posWs - input[1].posWs);
                scale.y = length(input[1].posWs - input[2].posWs);
                scale.z = length(input[2].posWs - input[0].posWs);


                for (int i = 0; i < 3; ++i)
                {
                    g2f o;
                    o.vertex = input[i].vertex;
                    o.uv = input[i].uv;
                    o.triangleID = triangleID;
                    o.uvTriPos = float2(i / 2, i % 2);
                    o.uvScale = scale;
                    OutputStream.Append(o);
                }
            }


            // x -> 0,1 y -> 0,1 z = 1 1
            fixed4 frag(g2f i) : SV_Target
            {
                int buffer = TriTypeBuffer[i.triangleID];
                int riverMask = (TriTypeBuffer[i.triangleID] >> 20);
                float2 riverMaskData = float2(riverMask % 2, riverMask / 2);
                float curWidth = (((buffer >> 12) & 63) + 10) / 256.0;
                float leftWidth = (((buffer >> 0) & 63) + 10) / 256.0;
                float rightWidth = (((buffer >> 6) & 63) + 10) / 256.0;


                return float4(pow(smoothstep(i.uvTriPos.xx , 1, .5f ),2), 0, 1);

                float3 scale = 1 / i.uvScale * .01;
                fixed4 res = 0;
                if (riverMaskData.g > 0)
                {
                    float rate = i.uvTriPos.y * 2;
                    float width = lerp(curWidth, leftWidth, rate);
                    if (abs(i.uvTriPos.x - .5) < width)
                    {
                        return 1;
                    }
                }

                if (riverMaskData.r > 0)
                {
                    float rate = i.uvTriPos.y * 2;
                    float width = lerp(curWidth, rightWidth, rate);
                    if (abs(i.uvTriPos.x + i.uvTriPos.y - .5) < width)
                    {
                        return 1;
                    }
                }

                return res;
            }
            ENDCG
        }
    }
}