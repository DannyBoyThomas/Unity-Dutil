Shader "Dutil/Gradient"
{
    Properties
    {
        _ColOne ("Start Color", Color) = (0, .91, .99,1) 
        _ColTwo ("End Color", Color) = (.13, .44, .95,1) 
        _Direction ("Direction", Range(0, 360)) = 0
        
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
            Float  _Direction;

            fixed4 frag (v2f i) : SV_Target
            {              
             //0 = up
             //x=0, y=1

                i.uv -=.5;
                //0 =up, 90 right
                float rad = _Direction * 0.0174532925;
                float x = cos(rad); //90 -> 0
                float y = sin(rad); //90 -> 1
              
                float2x2 rotationMatrix = float2x2( x, -y, y, x);
                rotationMatrix *=0.5;
                rotationMatrix +=0.5;
                rotationMatrix = rotationMatrix * 2-1;
                i.uv = mul ( i.uv.xy, rotationMatrix );
                i.uv +=.5;
               
                 fixed4 col = lerp(_ColTwo,_ColOne,i.uv.y);  
                  
                return col;
            }
            ENDCG
        }
    }
}
