using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace cky.PngTo3D.Builder
{
    public static class GetPixelTextures
    {
        public static Color[,] ReadTextureMap(Texture2D textureMap, int _readPixelStep, Vector3 brickDistance)
        {
            Vector3 readPixelStep;
            Color[,] pixels;
            readPixelStep = 1 * _readPixelStep * brickDistance;

            pixels = new Color[textureMap.width / (int)readPixelStep.x, textureMap.height / (int)readPixelStep.y];
            for (int y = 0; y < (textureMap.height / (int)readPixelStep.y) * (int)readPixelStep.y; y += (int)readPixelStep.y)
            {
                for (int x = 0; x < (textureMap.width / (int)readPixelStep.x) * (int)readPixelStep.x; x += (int)readPixelStep.x)
                {
                    int _x = x / (int)readPixelStep.x;
                    int _y = y / (int)readPixelStep.y;
                    pixels[_x, _y] = textureMap.GetPixel(x, y);
                }
            }
            return pixels;
        }

    }
}