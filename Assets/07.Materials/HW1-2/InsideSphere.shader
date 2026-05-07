Shader "Custom/InsideSphere"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Lighting Off
        Cull Front
        ZWrite On
        Pass
        {
            SetTexture [_MainTex] { combine texture }
        }
    }
}