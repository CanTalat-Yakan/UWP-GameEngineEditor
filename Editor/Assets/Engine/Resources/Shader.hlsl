cbuffer PerModelConstantBuffer : register(b0)
{
    matrix World;
};
cbuffer ViewConstantsBuffer : register(b1)
{
    matrix VP;
    float3 WCP;
};

struct appdata
{
    float3 vertex : POSITION;
    float2 uv : TEXCOORD;
    float3 normal : NORMAL;
};

Texture2D ObjTexture : register(t0);
SamplerState ObjSamplerState : register(s0);

struct VS_OUTPUT
{
    float4 pos : SV_POSITION;
    float3 worldPos : POSITION;
    float3 camPos : POSITION1;
    float2 uv : TEXCOORD;
    float3 normal : NORMAL;
};

VS_OUTPUT VS(appdata v)
{
    VS_OUTPUT o;

    o.pos = mul(float4(v.vertex, 1), mul(World, VP));
    o.normal = mul(float4(v.normal, 0), World);
    o.worldPos = mul(float4(v.vertex, 1), World);
    o.camPos = WCP;
    o.uv = v.uv;

    return o;
}

float4 PS(VS_OUTPUT i) : SV_TARGET
{
    float4 col = ObjTexture.Sample(ObjSamplerState, i.uv);
    
    return col + float4(i.worldPos, 1);
}
