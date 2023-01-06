using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// the ChangeColorPixelSystem is responsible for updating 
    /// entity pixels. this system have no knowledge of hitbox, collision and so one
    /// </summary>
    class ChangeColorPixelSystem : SuperSystem
    {
        public ChangeColorPixelSystem(HashSet<Entity> allEntities, params string[] requirements)
            : base(allEntities, requirements) { }

        /// <summary>
        /// itereate among all eligible entities.
        /// if the instruction is received by the component 
        /// change the color of the assocciated pixels to white (making it dissapear) 
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                SpriteComponent spriteComponentAssociated = (SpriteComponent) entity.GetComponent("SpriteComponent");
                PixelToSwitchComponent pixelToSwitchComponentAssociated = (PixelToSwitchComponent) entity.GetComponent("PixelToSwitchComponent");
                foreach (Vec2 pixelToSwitch in pixelToSwitchComponentAssociated.PixelPositions)
                {
                    if (pixelToSwitch.X >= 0 && pixelToSwitch.Y >= 0)
                        spriteComponentAssociated.Sprite.SetPixel(Convert.ToInt32(pixelToSwitch.X), Convert.ToInt32(pixelToSwitch.Y), Color.White);
                }
                pixelToSwitchComponentAssociated.PixelPositions.Clear(); // all the pixels have been switched
            }
        }
    }
}
