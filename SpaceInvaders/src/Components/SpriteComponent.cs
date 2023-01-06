using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Sprite component can be added to an entity 
    /// an entity implementing this component gonna render his <c>Bitmap sprite</c>
    /// </summary>
    class SpriteComponent
    {
        private Bitmap sprite;
        public Bitmap Sprite
        {
            get => this.sprite;
            set => this.sprite = value;
        }

        public SpriteComponent(Bitmap sprite)
        {
            this.sprite = sprite;
        }
    }
}
