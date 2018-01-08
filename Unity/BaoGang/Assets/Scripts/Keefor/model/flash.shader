// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/flash" {  
    Properties{  
        _Color("Color", Color) = (1,1,1,1)  
        _MainTex("纹理", 2D) = "white" {}  
       _LightColor("灯颜色",Color) = (1,1,1,1)  
        _LightDir("灯方向",Vector) = (0,1,0,1)  
        _OutLine("描边颜色",Color) = (1,1,1,1)  
        _EdgeChange("描边大小",Range(0,.1)) = .05  
        _FlickerTime("闪烁时间，0为关闭",Range(0,2)) = 1  
    }  
    CGINCLUDE  
    #include "UnityCG.cginc"  
    #pragma vertex vert  
    #pragma fragment frag  
    #pragma target 3.0  
    ENDCG  
    SubShader{  
    Tags{ "RenderType" = "Transparent"  
    "Queue" = "Transparent"  
    "LightMode" = "ForwardBase" }  
    LOD 200  
    Pass  
    {  
        Cull Front  
        ZWrite Off  
        Blend SrcAlpha OneMinusSrcAlpha  
        CGPROGRAM  
        fixed4 _OutLine;  
        float _EdgeChange;  
        float _FlickerTime;  
    struct a2v  
    {  
        float4 vertex:POSITION;  
        float3 normal : NORMAL;  
    };  
    struct v2f  
    {  
        float4 pos : POSITION;  
    };  
    v2f vert(a2v v)  
    {  
        v2f o;  
        //参考博客 http://blog.csdn.net/candycat1992/article/details/45577749  
        o.pos = mul(UNITY_MATRIX_MV,v.vertex);  
        v.normal = mul((float3x3)UNITY_MATRIX_MV,v.normal);  
        v.normal.z = -.5;  
        o.pos.xyz += v.normal*_EdgeChange;  
        o.pos = mul(UNITY_MATRIX_P,o.pos);  
        //参考官方教程  
        //v.vertex.xyz += v.normal*_EdgeChange;  
        //o.pos = mul(UNITY_MATRIX_MVP, v.vertex);  
        return o;  
    }  
    fixed4 frag(v2f i) :COLOR  
    {  
        //闪烁的乒乓的速度，如果脚本传值可以把这段删掉  
        _OutLine.a = abs(cos(_Time.x*_FlickerTime));  
        return _OutLine;  
    }  
    ENDCG  
    }  
    Pass  
    {  
    CGPROGRAM  
    fixed4 _Color;  
    sampler2D _MainTex;  
    fixed4 _MainTex_ST;  
    fixed4 _LightColor;  
    fixed4 _LightDir;  
  
  
    struct a2v  
    {  
        float4 vertex:POSITION;  
        float3 normal : NORMAL;  
        float4 texcoord : TEXCOORD0;  
    };  
    struct v2f {  
        float4 pos : POSITION;  
        float2 uv:TEXCOORD0;  
        float3 normal:TEXCOORD1;  
        float3 viewDir:TEXCOORD2;  
        UNITY_FOG_COORDS(3)  
    };  
  
  
    v2f vert(a2v v)  
    {  
        v2f o;  
        o.pos = UnityObjectToClipPos(v.vertex);  
        float4 wPos = mul(unity_ObjectToWorld, v.vertex);  
        o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);  
        //不过分追求效果的情况下在这里normalize,如果追求效果，请在frag内normalize，否则插值会被归一。  
        o.normal = normalize(mul(v.normal, unity_WorldToObject).xyz);  
        //我们希望光是从摄像机对面过来的，所以就反减  
        o.viewDir = normalize(wPos.xyz - _WorldSpaceCameraPos);  
        //旋转矩阵  
        float3x3 rotaX = { 1,0,0,0,cos(_LightDir.x),sin(_LightDir.x),0,-sin(_LightDir.x),cos(_LightDir.x) };  
        float3x3 rotaY = { cos(_LightDir.y),0,sin(_LightDir.y),0,1,0,-sin(_LightDir.y),0,cos(_LightDir.y) };  
        float3x3 rotaZ = { cos(_LightDir.z),sin(_LightDir.z),0,-sin(_LightDir.z),cos(_LightDir.z),0,0,0,1 };  
        o.viewDir = mul(rotaX, o.viewDir);  
        o.viewDir = mul(rotaY, o.viewDir);  
        o.viewDir = mul(rotaZ, o.viewDir);  
        //矩阵相乘后结果不太对，可能写错了。  
        //float3x3 rota = { sin(_LightDir.y)*sin(_LightDir.z),sin(_LightDir.y)*cos(_LightDir.z),cos(_LightDir.y),  
        // -sin(_LightDir.y)*cos(_LightDir.x)*sin(_LightDir.z) - sin(_LightDir.x)*sin(_LightDir.z),-sin(_LightDir.y)*cos(_LightDir.x)*cos(_LightDir.z) + sin(_LightDir.x)*cos(_LightDir.z),cos(_LightDir.x)*cos(_LightDir.y),  
        // -sin(_LightDir.y)*cos(_LightDir.x)*sin(_LightDir.z) + sin(_LightDir.x)*sin(_LightDir.z),-sin(_LightDir.y)*cos(_LightDir.x)*cos(_LightDir.z) - sin(_LightDir.x)*cos(_LightDir.z),cos(_LightDir.x)*cos(_LightDir.y)  
        //};  
        //o.viewDir = mul(rota, o.viewDir);  
        return o;  
    }  
    fixed4 frag(v2f i) :COLOR  
    {  
        fixed4 col;  
        fixed4 tex = tex2D(_MainTex,i.uv);  
        //点乘normal和视线，判断相似度  
        float diff = max(0,dot(i.normal, i.viewDir));  
        col = tex*_Color + _LightColor*diff*_LightDir.w;  
        return col;  
    }  
    ENDCG  
    }  
    }  
    FallBack "Diffuse"  
}  