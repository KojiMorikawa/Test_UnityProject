// Shader created with Shader Forge Beta 0.34 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.34;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,rpth:0,hqsc:True,hqlp:False,blpr:0,bsrc:0,bdst:0,culm:0,dpts:2,wrdp:True,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:1,x:32600,y:32612|diff-14-OUT;n:type:ShaderForge.SFN_Tex2dAsset,id:9,x:33351,y:32601,ptlb:node_9,ptin:_node_9,glob:False,tex:b66bceaf0cc0ace4e9bdc92f14bba709;n:type:ShaderForge.SFN_Multiply,id:14,x:32868,y:32612|A-15-RGB,B-16-RGB;n:type:ShaderForge.SFN_Tex2d,id:15,x:33117,y:32533,tex:b66bceaf0cc0ace4e9bdc92f14bba709,ntxv:0,isnm:False|UVIN-40-UVOUT,TEX-9-TEX;n:type:ShaderForge.SFN_Color,id:16,x:33151,y:32734,ptlb:node_16,ptin:_node_16,glob:False,c1:0.8235294,c2:0.8175158,c3:0.6055363,c4:1;n:type:ShaderForge.SFN_TexCoord,id:34,x:33627,y:32379,uv:0;n:type:ShaderForge.SFN_Rotator,id:40,x:33351,y:32390|UVIN-34-UVOUT,ANG-52-OUT;n:type:ShaderForge.SFN_Time,id:51,x:33762,y:32636;n:type:ShaderForge.SFN_Multiply,id:52,x:33554,y:32547|A-58-OUT,B-51-T;n:type:ShaderForge.SFN_Vector1,id:58,x:33762,y:32559,v1:0.05;proporder:9-16;pass:END;sub:END;*/

Shader "Shader Forge/TestFloorShader" {
    Properties {
        _node_9 ("node_9", 2D) = "white" {}
        _node_16 ("node_16", Color) = (0.8235294,0.8175158,0.6055363,1)
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers d3d11 xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _node_9; uniform float4 _node_9_ST;
            uniform float4 _node_16;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb*2;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_51 = _Time + _TimeEditor;
                float node_40_ang = (0.05*node_51.g);
                float node_40_spd = 1.0;
                float node_40_cos = cos(node_40_spd*node_40_ang);
                float node_40_sin = sin(node_40_spd*node_40_ang);
                float2 node_40_piv = float2(0.5,0.5);
                float2 node_40 = (mul(i.uv0.rg-node_40_piv,float2x2( node_40_cos, -node_40_sin, node_40_sin, node_40_cos))+node_40_piv);
                finalColor += diffuseLight * (tex2D(_node_9,TRANSFORM_TEX(node_40, _node_9)).rgb*_node_16.rgb);
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers d3d11 xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform float4 _TimeEditor;
            uniform sampler2D _node_9; uniform float4 _node_9_ST;
            uniform float4 _node_16;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(float4(v.normal,0), _World2Object).xyz;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float3 diffuse = max( 0.0, NdotL) * attenColor;
                float3 finalColor = 0;
                float3 diffuseLight = diffuse;
                float4 node_51 = _Time + _TimeEditor;
                float node_40_ang = (0.05*node_51.g);
                float node_40_spd = 1.0;
                float node_40_cos = cos(node_40_spd*node_40_ang);
                float node_40_sin = sin(node_40_spd*node_40_ang);
                float2 node_40_piv = float2(0.5,0.5);
                float2 node_40 = (mul(i.uv0.rg-node_40_piv,float2x2( node_40_cos, -node_40_sin, node_40_sin, node_40_cos))+node_40_piv);
                finalColor += diffuseLight * (tex2D(_node_9,TRANSFORM_TEX(node_40, _node_9)).rgb*_node_16.rgb);
/// Final Color:
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
