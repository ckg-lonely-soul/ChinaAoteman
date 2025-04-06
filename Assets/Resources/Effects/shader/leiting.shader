// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.35 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.35;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:33862,y:33209,varname:node_3138,prsc:2|emission-1802-OUT,custl-3577-OUT;n:type:ShaderForge.SFN_Fresnel,id:9627,x:32073,y:33351,varname:node_9627,prsc:2;n:type:ShaderForge.SFN_Power,id:928,x:32600,y:33297,varname:node_928,prsc:2|VAL-9627-OUT,EXP-766-OUT;n:type:ShaderForge.SFN_Vector1,id:766,x:32181,y:33422,varname:node_766,prsc:2,v1:1.25;n:type:ShaderForge.SFN_Multiply,id:5280,x:32719,y:33066,varname:node_5280,prsc:2|A-7359-OUT,B-2108-OUT;n:type:ShaderForge.SFN_Vector3,id:7359,x:32146,y:33008,varname:node_7359,prsc:2,v1:0.1,v2:0.2,v3:1;n:type:ShaderForge.SFN_Multiply,id:9207,x:32886,y:33247,varname:node_9207,prsc:2|A-7359-OUT,B-928-OUT,C-475-OUT;n:type:ShaderForge.SFN_Add,id:1802,x:33247,y:33178,varname:node_1802,prsc:2|A-5280-OUT,B-9207-OUT,C-1486-OUT;n:type:ShaderForge.SFN_Vector1,id:920,x:32227,y:33475,varname:node_920,prsc:2,v1:5;n:type:ShaderForge.SFN_Power,id:3965,x:32713,y:33448,varname:node_3965,prsc:2|VAL-9627-OUT,EXP-920-OUT;n:type:ShaderForge.SFN_Vector1,id:475,x:32845,y:33367,varname:node_475,prsc:2,v1:0.8;n:type:ShaderForge.SFN_Vector1,id:9088,x:32181,y:33227,varname:node_9088,prsc:2,v1:0.75;n:type:ShaderForge.SFN_Power,id:2108,x:32146,y:33101,varname:node_2108,prsc:2|VAL-9627-OUT,EXP-9088-OUT;n:type:ShaderForge.SFN_Vector1,id:7495,x:33829,y:33907,varname:node_7495,prsc:2,v1:0.00075;n:type:ShaderForge.SFN_Tex2d,id:1529,x:32524,y:33678,ptovrint:False,ptlb:node_1529,ptin:_node_1529,varname:node_1529,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:98a232da793f88f4f880ffce2298d6b8,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:9797,x:33851,y:33748,varname:node_9797,prsc:2|A-1402-OUT,B-7495-OUT;n:type:ShaderForge.SFN_OneMinus,id:1402,x:33080,y:34125,varname:node_1402,prsc:2|IN-1529-A;n:type:ShaderForge.SFN_Multiply,id:114,x:33098,y:33543,varname:node_114,prsc:2|A-7359-OUT,B-1529-A;n:type:ShaderForge.SFN_Multiply,id:1486,x:33113,y:33387,varname:node_1486,prsc:2|A-3965-OUT,B-6618-OUT;n:type:ShaderForge.SFN_Vector1,id:6618,x:32713,y:33595,varname:node_6618,prsc:2,v1:1.25;n:type:ShaderForge.SFN_Panner,id:5460,x:32631,y:33871,varname:node_5460,prsc:2,spu:-0.4,spv:0|UVIN-9011-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:9011,x:31996,y:33796,varname:node_9011,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Power,id:3964,x:33068,y:33701,varname:node_3964,prsc:2|VAL-1529-A,EXP-8447-OUT;n:type:ShaderForge.SFN_Vector1,id:8447,x:32980,y:33844,varname:node_8447,prsc:2,v1:7;n:type:ShaderForge.SFN_Add,id:8199,x:33378,y:33544,varname:node_8199,prsc:2|A-114-OUT,B-3964-OUT;n:type:ShaderForge.SFN_Multiply,id:3577,x:33613,y:33626,varname:node_3577,prsc:2|A-8199-OUT,B-2564-RGB;n:type:ShaderForge.SFN_Tex2d,id:2564,x:33032,y:33937,ptovrint:False,ptlb:node_2564,ptin:_node_2564,varname:node_2564,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:2db098b6043a67046ae40b2b3ec8b638,ntxv:0,isnm:False|UVIN-5460-UVOUT;proporder:1529-2564;pass:END;sub:END;*/

Shader "Shader Forge/leiting" {
    Properties {
        _node_1529 ("node_1529", 2D) = "white" {}
        _node_2564 ("node_2564", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x xboxone ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform sampler2D _node_1529; uniform float4 _node_1529_ST;
            uniform sampler2D _node_2564; uniform float4 _node_2564_ST;
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
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float3 node_7359 = float3(0.1,0.2,1);
                float node_9627 = (1.0-max(0,dot(normalDirection, viewDirection)));
                float3 emissive = ((node_7359*pow(node_9627,0.75))+(node_7359*pow(node_9627,1.25)*0.8)+(pow(node_9627,5.0)*1.25));
                float4 _node_1529_var = tex2D(_node_1529,TRANSFORM_TEX(i.uv0, _node_1529));
                float4 node_7533 = _Time + _TimeEditor;
                float2 node_5460 = (i.uv0+node_7533.g*float2(-0.4,0));
                float4 _node_2564_var = tex2D(_node_2564,TRANSFORM_TEX(node_5460, _node_2564));
                float3 finalColor = emissive + (((node_7359*_node_1529_var.a)+pow(_node_1529_var.a,7.0))*_node_2564_var.rgb);
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
