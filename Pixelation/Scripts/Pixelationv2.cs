using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Assets.Pixelation.Scripts
{
    [Serializable]
    [PostProcess(typeof(Pixelationv2Renderer), PostProcessEvent.AfterStack, "Custom/Pixelation")]
    public sealed class Pixelationv2 : PostProcessEffectSettings
    {
        [Range(64.0f, 512.0f)]
        public FloatParameter BlockCount = new FloatParameter { value = 128 };
    }

    public sealed class Pixelationv2Renderer : PostProcessEffectRenderer<Pixelationv2>
    {
        public override void Render(PostProcessRenderContext context)
        {
            float k = context.camera.aspect;
            var blockCount = settings.BlockCount;
            var count = new Vector2(blockCount, blockCount / k);
            var size = new Vector2(1.0f / count.x, 1.0f / count.y);
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Pixelation"));

            sheet.properties.SetVector("BlockCount", count);
            sheet.properties.SetVector("BlockSize", size);
            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
}
