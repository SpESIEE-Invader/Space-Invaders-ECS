using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Speed component can be added to an entity 
    /// an entity implementing this component has a velocity
    /// without a velocity the entity is fix on the screen
    /// </summary>
    class VelocityComponent
    {
        private Vec2 velocity;
        public Vec2 Velocity
        {
            get => this.velocity;
            set => this.velocity = value; 
        }

        public VelocityComponent(Vec2 velocity) 
        {
            this.velocity = velocity;
        }
    }
    
}
