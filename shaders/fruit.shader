HEADER
{
	Description = "Fruit Shader";
}

FEATURES
{
    #include "common/features.hlsl"
}

MODES
{
	VrForward();
	ToolsVis( S_MODE_TOOLS_VIS );
	Depth( S_MODE_DEPTH );
}

COMMON
{
	#include "common/shared.hlsl"
}

struct VertexInput
{
	#include "common/vertexinput.hlsl"
};

struct PixelInput
{
	#include "common/pixelinput.hlsl"
};

VS
{
	#include "common/vertex.hlsl"

	PixelInput MainVs( VertexInput i )
	{
		PixelInput o = ProcessVertex( i );
		return FinalizeVertex( o );
	}
}

PS
{ 
    StaticCombo( S_MODE_DEPTH, 0..1, Sys(ALL) );

    #define DEPTH_STATE_ALREADY_SET
    RenderState( DepthWriteEnable, false );
    RenderState( DepthEnable, false );

    #include "vr_common_ps_code.fxc"

    #define BLEND_MODE_ALREADY_SET
	RenderState( BlendEnable, true );
	RenderState( SrcBlend, SRC_ALPHA );
    RenderState( DstBlend, INV_SRC_ALPHA );

	CreateTexture2D( g_tFruitTexture ) < Attribute( "FruitTexture" ); SrgbRead( true ); Filter( POINT ); AddressU( CLAMP ); AddressV( CLAMP ); >;
    
    BoolAttribute(translucent, true);
	RenderState( CullMode, NONE );


	float4 MainPs( PixelInput i ) : SV_Target0
	{
		float2 uv = i.vTextureCoords.xy;
        float4 result = Tex2D( g_tFruitTexture, uv.xy ).rgba;

		return result.rgba;
	}
}