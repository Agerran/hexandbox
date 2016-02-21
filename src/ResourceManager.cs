using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace hexandbox
{
    public static class ResourceManager
    {
        private static TmxMap _tmxMap = new TmxMap("..\\data\\map.tmx");
        public static TmxMap Map { get { return _tmxMap; } }

        public static TmxLayerTile GetTile(int x, int y, int layer)
        {
            return _tmxMap.Layers[layer].Tiles[x + y * _tmxMap.Width];
        }

        public static SpriteTextureDTO GetTexture(int gId)
        {
            int tileId = gId - 1;
            var tileset = _tmxMap.Tilesets.First();

            var x = tileId % tileset.Columns.Value;
            var y = tileId / tileset.Columns.Value;

            var imageHeight = tileset.Image.Height.Value;
            var imageWidth = tileset.Image.Width.Value;

            var tileHeight = (float)tileset.TileHeight;
            var tileWidth = (float)tileset.TileWidth;

            var x0 = (x * tileWidth) / imageWidth;
            var y0 = (y * tileHeight) / imageHeight;
            var x1 = ((x + 1) * tileWidth ) / imageWidth;
            var y1 = ((y + 1) * tileHeight) / imageHeight;

            return new SpriteTextureDTO(x0, y0, x1, y1);
        }

    }
}
