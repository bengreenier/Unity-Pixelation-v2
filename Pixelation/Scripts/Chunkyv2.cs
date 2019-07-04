using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Assets.Pixelation.Scripts
{
    [Serializable]
    [PostProcess(typeof(Chunkyv2Renderer), PostProcessEvent.AfterStack, "Custom/Chunky")]
    public sealed class Chunkyv2 : PostProcessEffectSettings
    {
        public TextureParameter SprTex = new TextureParameter() { value = null };
        public ColorParameter Color = new ColorParameter { value = UnityEngine.Color.white };
    }

    public sealed class Chunkyv2Renderer : PostProcessEffectRenderer<Chunkyv2>
    {
        public override void Render(PostProcessRenderContext context)
        {
            if (settings.SprTex?.value == null)
            {
                return;
            }

            var textDimsOrDefault = new Vector2(settings.SprTex.value.width, settings.SprTex.value.height);
            var w = context.camera.pixelWidth;
            var h = context.camera.pixelHeight;
            var count = new Vector2(w / textDimsOrDefault.x, h / textDimsOrDefault.y);
            var size = new Vector2(1.0f / count.x, 1.0f / count.y);

            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Chunky"));

            sheet.properties.SetVector("BlockCount", count);
            sheet.properties.SetVector("BlockSize", size);
            sheet.properties.SetColor("_Color", settings.Color);
            sheet.properties.SetTexture("_SprTex", settings.SprTex);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
