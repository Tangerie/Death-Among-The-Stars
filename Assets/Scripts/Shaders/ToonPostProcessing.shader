Shader "Hidden/ToonPostProcessing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _StepValue ("Step Value", Range(0.000000000000001, 0.2)) = 0.1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float3 Hue(float H)
            {
                float R = abs(H * 6 - 3) - 1;
                float G = 2 - abs(H * 6 - 2);
                float B = 2 - abs(H * 6 - 4);
                return saturate(float3(R,G,B));
            }

            float4 HSVtoRGB(in float3 HSV)
            {
                return float4(((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z,1);
            }

            float4 RGBtoHSV(in float3 RGB)
            {
                float3 HSV = 0;
                HSV.z = max(RGB.r, max(RGB.g, RGB.b));
                float M = min(RGB.r, min(RGB.g, RGB.b));
                float C = HSV.z - M;
                if (C != 0)
                {
                    HSV.y = C / HSV.z;
                    float3 Delta = (HSV.z - RGB) / C;
                    Delta.rgb -= Delta.brg;
                    Delta.rg += float2(2,4);
                    if (RGB.r >= HSV.z)
                        HSV.x = Delta.b;
                    else if (RGB.g >= HSV.z)
                        HSV.x = Delta.r;
                    else
                        HSV.x = Delta.g;
                    HSV.x = frac(HSV.x / 6);
                }
                return float4(HSV,1);
            }

            sampler2D _MainTex;
            float _StepValue;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                /*col.r = round(col.r / _StepValue) * _StepValue;
                col.g = round(col.g / _StepValue) * _StepValue;
                col.b = round(col.b / _StepValue) * _StepValue;*/

                //col = round(col / _StepValue) * _StepValue;

                float3 hsv = RGBtoHSV(col.rgb);
                hsv.z = round(hsv.z / _StepValue) * _StepValue;

                
                return HSVtoRGB(hsv);
            }
            ENDCG
        }
    }
}
