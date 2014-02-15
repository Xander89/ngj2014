#ifndef BEND_INCLUDED
#define BEND_INCLUDED

float _Bend = -0.002;

#define BEND_AMOUNT -0.002

inline float4 bend_ObjectToClip(float4 vertex)
{
	float4 pos = mul (UNITY_MATRIX_MVP, vertex);
	float z = pos.w;
//	pos.y = pos.y + z * z * (BEND_AMOUNT);// _Bend;
	pos.y = pos.y + z * z * _Bend;
	return pos;
}	

#endif