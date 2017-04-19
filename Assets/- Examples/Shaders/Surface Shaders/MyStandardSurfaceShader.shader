Shader "Custom/MyStandardShader" {

//##################//
// PROPERTIES BLOCK //
//##################//

//----------------------------------------------------------------------------------------------
//For more information: https://docs.unity3d.com/Manual/SL-Properties.html
//"Shaders can define a list of parameters to be set by artists in Unity's material inspector.
//The properties block in the shader file defines them"
//----------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------
// How properties are declared:
//----------------------------------------------------------------------------------------------
// _nameOfProperty ("Name in inspector", PropertyType) = defaultValue
// Note how there is no ";" at the end of each line for properties. A new line = a new property
//----------------------------------------------------------------------------------------------

//----------------------------------------------------------------------------------------------
// Type of properties:
//----------------------------------------------------------------------------------------------

// Numbers & Sliders
// _propertyName ("display name", Range(min, max)) = number
// _propertyName ("display name", Float) = number
// _propertyName ("display name", Int) = number
// The Range form makes the number to be displayed as a slider between min and max ranges

// Colors and Vectors
// _propertyName ("display name", Color) = (number, number, number, number)
// _propertyName ("display name", Vector) = (number, number, number, number)
// Defines a color property with default value of given RGBA components, or a 4D vector

// Textures
// _propertyName ("display name", 2D) = "" {}
// _propertyName ("display name", Cube) = "" {}
// _propertyName ("display name", 3D) = ""  {}
// Defines a 2D texture, a cubemap or a 3D Volume texture
// 3D textures are used for advanced cases, such as 3D Color Correction (Such as the Color LUT example in this project)
// The default value of 2D textures can be either "" {}, "white" {}, "black" {}, "gray" {}, "bump" {}, "red" {}

//----------------------------------------------------------------------------------------------

	Properties {
		_Color ("Albedo Color Blend", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "" {}

		_NormalMap ("Normal Map", 2D) = "bump" {}
		_NormalBoost ("Normal Boost", Range(0,1)) = 0.0

		_EmissionColor ("Emission Color", Color) = (0,0,0,0)
		_EmissionMap ("Emission Map", 2D) = "white" {}

		_MetallicMap ("Metallic Map", 2D) = "white" {}
		_MetallicLevel ("Metallic Level", Range(0,1)) = 0.0

		_SmoothnessMap ("Smoothness Map", 2D) = "white" {}
		_SmoothnessLevel ("Smoothness Level", Range(0,1)) = 0.5

		_OcclusionMap ("Occlusion Map", 2D) = "white" {}
		_OcclusionLevel ("Occlusion Level", Range(0,1)) = 1.0
	}

//#################################################################//
// SUBSHADER BLOCK (SURFACE SHADERS SPECIFICALLY FOR THIS EXAMPLE) //
//#################################################################//

//------------------------------------------------------------------------------------------------------
//For more information: https://docs.unity3d.com/Manual/SL-SurfaceShaders.html
//"Surface Shaders in Unity is a code generation approach that makes it much easier to write lit shaders
//than using low level vertex/pixel shader programs."
//------------------------------------------------------------------------------------------------------

	SubShader {

	//############//
	// TAGS BLOCK //
	//############//

	//---------------------------------------------------------------------------------------------
	//For more information: https://docs.unity3d.com/Manual/SL-SubShaderTags.html
	//"Subshaders use tags to tell how and when they expect to be rendered to the rendering engine.
	//than using low level vertex/pixel shader programs."
	//---------------------------------------------------------------------------------------------

	//---------------------------------------------------------------------------------------------
	// Tag types:
	//---------------------------------------------------------------------------------------------

	// Queue Tag
	// Determines in which render queue objects belong to. Important to differentiate transparent objects from opaque ones.
	// Possible values:
	// Background - this render queue is rendered before any others. You’d typically use this for things that really need to be in the background. (Such as skyboxes)
	// Geometry (default) - this is used for most objects. Opaque geometry uses this queue.
	// AlphaTest -It’s a separate queue from Geometry one since it’s more efficient to render alpha-tested objects after all solid ones are drawn.
	// Transparent - this render queue is rendered after Geometry and AlphaTest, in back-to-front order. Anything alpha-blended (i.e. shaders that don’t write to depth buffer) should go here (glass, particle effects).
	// Overlay - this render queue is meant for overlay effects. Anything rendered last should go here (e.g. lens flares).
	// ex: "Queue"="Geometry"

	// RenderType tag
	// Categorizes shaders into several predefined groups, e.g. is is an opaque shader, or an alpha-tested shader etc.
	// Possible values:
	// Opaque: most of the shaders (Normal, Self Illuminated, Reflective, terrain shaders).
	// Transparent: most semitransparent shaders (Transparent, Particle, Font, terrain additive pass shaders).
	// TransparentCutout: masked transparency shaders (Transparent Cutout, two pass vegetation shaders).
	// Background: Skybox shaders.
	// Overlay: GUITexture, Halo, Flare shaders.
	// ex: "RenderType"="Opaque"

	// DisableBatching tag
	// Some shaders (mostly ones that do object-space vertex deformations) do not work when Draw Call Batching is used.
	// Possible values:
	// "True"
	// "False"
	// "LODFading" (disable batching when LOD fading is active; mostly used on trees)
	// ex: "DisableBatching"="False"

	// ForceNoShadowCasting tag
	// Determines if an object rendered using this subshader will cast shadows or not.
	// Possible values:
	// "True"
	// "False"
	// ex: "ForceNoShadowCasting"="True"

	// IgnoreProjector tag
	// Determines if an object rendered using this subshader will be affected by projectors.
	// Possible values:
	// "True"
	// "False"
	// ex: "IgnoreProjector"="True"

	//CanUseSpriteAtlas tag
	// Set CanUseSpriteAtlas tag to “False” if the shader is meant for sprites, and will not work when they are packed into atlases.
	// Possible values:
	// "True"
	// "False"
	// ex: "CanUseSpriteAtlas"="True"

	//PreviewType tag
	// Indicates how the material inspector preview should display the material. If not specified, will default to sphere
	// Possible values:
	// "Plane"
	// "Skybox"

		Tags { 
			"Queue"="Geometry"
			"RenderType"="Opaque"
			"DisableBatching"="False"
			"ForceNoShadowCasting"="False"
			"IgnoreProjector"="False"
		}
		//#####//
		// LOD //
		//#####//
		//--------------------------------------------------
		// https://docs.unity3d.com/Manual/SL-ShaderLOD.html
		// Shader Level of Detail (LOD) works by only using shaders or subshaders that have their LOD value less than a given number.
		//--------------------------------------------------
		LOD 200

		//##########//
		// CULLLING //
		//##########//
		//--------------------------------------------------------------
		// https://docs.unity3d.com/Manual/SL-CullAndDepth.html
		// Controls which sides of polygons should be culled (not drawn)
		//--------------------------------------------------------------
		// Possible Values:
		// Back - Don’t render polygons facing away from the viewer (default).
		// Front - Don’t render polygons facing towards the viewer. Used for turning objects inside-out.
		// Off - Disables culling - all faces are drawn. Used for special effects.

		Cull Back

		//########//
		// ZWrite //
		//########//
		//--------------------------------------------------------------
		// https://docs.unity3d.com/Manual/SL-CullAndDepth.html
		// Controls whether pixels from this object are written to the depth buffer (default is On).
		// If you’re drawng solid objects, leave this on. If you’re drawing semitransparent effects, switch to ZWrite Off.
		//--------------------------------------------------------------
		// Possible Values: On(Default), Off
		ZWrite On

		//#######//
		// ZTest //
		//#######//
		//--------------------------------------------------------------
		// https://docs.unity3d.com/Manual/SL-CullAndDepth.html
		// How should depth testing be performed. 
		// Default is LEqual (draw objects in from or at the distance as existing objects; hide objects behind them).
		//--------------------------------------------------------------
		//Possible Values: Less, Greater, LEqual (Default), GEqual, Equal, NotEqual, Always
		Ztest LEqual

		//##########//
		// Blending //
		//##########//
		//--------------------------------------------------------------
		// https://docs.unity3d.com/Manual/SL-Blend.html
		// Determines how they are pixels are combined with what is already there is controlled by the Blend command.
		// Allow for different types of transparency
		//--------------------------------------------------------------
		//Possible Values: Refer to documentation
		Blend Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		fixed4 _Color;
		sampler2D _MainTex;

		sampler2D _NormalMap;
		half _NormalBoost;

		fixed4 _EmissionColor;
		sampler2D _EmissionMap;

		sampler2D _MetallicMap;
		half _MetallicLevel;

		sampler2D _SmoothnessMap;
		half _SmoothnessLevel;

		sampler2D _OcclusionMap;
		half _OcclusionLevel;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalMap;
			float2 uv_EmissionMap;
			float2 uv_MetallicMap;
			float2 uv_SmoothnessMap;
			float2 uv_OcclusionMap;
		};


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			//Normal Map Blending + Normal map boosting
			o.Normal = UnpackNormal(tex2D(_NormalMap,IN.uv_NormalMap)) * float3(_NormalBoost, _NormalBoost, 1);
			o.Normal = normalize(o.Normal);

			//Emission
			o.Emission = tex2D(_EmissionMap, IN.uv_EmissionMap) * _EmissionColor * _EmissionColor.a;

			//Metallic blending
			o.Metallic = tex2D(_MetallicMap, IN.uv_MetallicMap) * _MetallicLevel;

			//Smoothness Blending
			o.Smoothness = tex2D(_SmoothnessMap, IN.uv_SmoothnessMap) * _SmoothnessLevel;

			//Occlusion Blending
			o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap) * _OcclusionLevel;

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
