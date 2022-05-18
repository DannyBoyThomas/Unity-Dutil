Shader "Dutil/Gradient"
{
    Properties
    {
        _ColOne ("Start Color", Color) = (1,1,1,1) 
        _ColTwo ("End Color", Color) = (0,0,0,1) 
        _Sin ("Sine", Float) = 0
        _Cos ("Cosine", Float) = 0
        
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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            fixed4 _ColOne, _ColTwo;
            Float _Sin,_Cos;

            fixed4 frag (v2f i) : SV_Target
            {              
                float t = (i.uv.y-.5)*-_Sin + (i.uv.x-.5)*_Cos;
                fixed4 col = lerp(_ColTwo,_ColOne,t);  
               
                return col; 
            }
            ENDCG
        }
    }
}
