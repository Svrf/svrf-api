Shader "Svrf/Occluder"
{
    Properties
    {
    }
    SubShader{
        Tags { "RenderType"="Transparent" "Queue" = "Geometry-1" }

        Pass {
            ZWrite On
            ColorMask 0
        }
    }
}
