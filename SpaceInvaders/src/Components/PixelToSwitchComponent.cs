using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The PixelToSwitch component can be added to an entity 
    /// an entity implementing this component gonna have his sprite alter by missile
    /// whenever a missile hit the entity implementing this component (two pixel enter in collision)
    /// a list of pixel in collision is gonna be set and gonna dispear from the sprite 
    /// </summary>
    class PixelToSwitchComponent
    {
        private HashSet<Vec2> pixelPositions;

        public PixelToSwitchComponent()
        {
            this.pixelPositions = new HashSet<Vec2>();
        }
        
        public HashSet<Vec2> PixelPositions
        {
            get => this.pixelPositions;
            set => this.pixelPositions = value;
        }
    }
}
