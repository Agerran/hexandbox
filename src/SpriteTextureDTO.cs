using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hexandbox
{
    public class SpriteTextureDTO
    {
        public float x0 { get; set; }
        public float x1 { get; set; }
        public float y0 { get; set; }
        public float y1 { get; set; }

        public SpriteTextureDTO() { }

        public SpriteTextureDTO(float _x0, float _y0, float _x1, float _y1)
        {
            x0 = _x0;
            x1 = _x1;
            y0 = _y0;
            y1 = _y1;
        }
    }
}
