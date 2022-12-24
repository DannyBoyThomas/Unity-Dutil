Shader "Unlit/Circle"
{
    Properties
    {
        //radius
        _Radius ("Radius", Float) = 0.5
        //thickness
        _Thickness ("Thickness", Float) = 0.1
        _Color ("Color (RGBA)", Color) = (1, 1, 1, 1)
        _Progress ("Progress", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
        
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                //radius
                float radius : TEXCOORD1;
                //thickness
                float thickness : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

          
            //radius
            float _Radius;
            //thickness
            float _Thickness;
            //color
            float4 _Color;
            //progress
            float _Progress;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                      
                float2 centre = float2(0.5,0.5);
               
             //get distance from center
                float dist = distance(i.uv,centre);
                //if distance is less than radius
                if(dist < _Radius)
                {
                    //if distance is less than radius - thickness
                    if(dist < _Radius - _Thickness)
                    {
                      
                            //return float 4 0,0,0,0
                        return fixed4(1,1,1,0);
                    }
                    else
                    {
                        //else set alpha to 1 - (distance - (radius - thickness)) / thickness
                        fixed4 col = fixed4(_Color.xyz,1);
                         return col;
                    }
                }
                else
                {
                    //else set alpha to 0
                   return fixed4(1,1,1,0);
                }
                

               
            }
            ENDCG
        }
    }
}
