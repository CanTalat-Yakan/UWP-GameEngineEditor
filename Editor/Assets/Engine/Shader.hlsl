cbuffer ConstantBuffer : register(b0)
{
    matrix VP;
    float3 WCP;
};
cbuffer PerModelConstantBuffer : register(b1)
{
    matrix World;
};

struct appdata
{
    float3 vertex : POSITION;
    float2 uv : TEXCOORD;
    float3 normal : NORMAL;
};

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

    o.pos = mul(mul(VP, World), float4(v.vertex, 1));
    o.normal = mul(float4(v.normal, 0), World);
    o.worldPos = mul(float4(v.vertex, 1), World);
    o.camPos = WCP;
    o.uv = v.uv;

    return o;
}

float4 PS(VS_OUTPUT i) : SV_TARGET
{
    return float4(0, 0, 0, 1);
}
