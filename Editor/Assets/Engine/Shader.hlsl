cbuffer ConstantBuffer : register(b0)
{
	matrix view;
	matrix projection;
};

cbuffer PerModelConstantBuffer : register(b1)
{
	matrix world;
};

float4 VS(min16float3 inPos : POSITION) : SV_POSITION
{
	float4 pos = float4(inPos, 1.0f);

	pos = mul(pos, world);
	pos = mul(pos, view);
	pos = mul(pos, projection);

	return(pos);
}

float4 PS(float4 notUsed : SV_POSITION) : SV_TARGET
{
    return float4(0.2f, 0.2f, 0.2f, 1.0f);
}
