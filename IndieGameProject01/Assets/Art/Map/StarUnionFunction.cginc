
float2 RotateUV(float2 uv, float _a)
{
    float2 curUV = uv - 0.5;
    curUV = float2(curUV.x * cos(_a) - curUV.y * sin(_a), curUV.y * cos(_a) + curUV.x * sin(_a));
    curUV += 0.5;
    return curUV;
}