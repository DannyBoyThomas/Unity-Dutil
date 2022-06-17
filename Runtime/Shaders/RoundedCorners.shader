Shader "Dutil/RoundedCorners"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width ("Width",Float) = 1920
        _Height ("Height",Float) = 1080
        _Radius ("Radius",Float) = .1
        _Color ("Color",Color) = (1,1,1,1)
        
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
         Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Width;
            float _Height;
            float _Radius;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
          
            // Signed distance function for a rectangle with rounded corners.
            // Rectangle is located at (0,0)
            //  pos - point position
            //  ext - rectangle extents, (halfWidth, halfHeight)
            //  cr - corner radii, starting top left clockwise, (lt, rt, rb, lb)
            float sdRoundRect(float2 pos, float2 ext, float4 cr) {
            // select the radius according to the quadrant the point is in
            float2 s = step(pos, float2(0,0));
            float r = lerp(
                lerp(cr.y, cr.z, s.y),  
                lerp(cr.x, cr.w, s.y),
                s.x);
            return length(max(abs(pos) + float2(r,r) - ext, 0.0)) - r;
            }
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float relRadius = max(_Radius,0)*min(_Width,_Height);
                 float aspect = _Width/_Height;
                float relRadiusX = _Radius/_Width;
                float relRadiusY = _Radius/_Height*aspect;
               
               float2 size = float2(_Width,_Height);

              float2 pos = i.uv.xy * size;
              float2 boxPos = pos - 0.5*size;
                float2 boxExt = size/2;
                float4 cr = float4(relRadius,relRadius,relRadius,relRadius);
                float dist = sdRoundRect(boxPos,boxExt,cr);
                float4 color =_Color * float4(1,1,1,1)*step(dist,0);
             return color;

          
               /*  if(i.uv.x>relRadiusX && i.uv.x<1-relRadiusX && i.uv.y>relRadiusY && i.uv.y<1-relRadiusY && )
                {
              
                    return float4(1,0,0,1);
                } */
               
               
               /*  fixed4 col = float4(1,1,1,1);
                // apply fog
                return col; */
            }
            ENDCG
        }
    }
}
