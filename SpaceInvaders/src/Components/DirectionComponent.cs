using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{

    /// <summary>
    /// The direction component can be added to an entity 
    /// an entity implementing this component can then move in several direction
    /// </summary>
    class DirectionComponent
    {
        private Vec2 direction;
        public Vec2 Direction
        {
            get => this.direction;
            set => this.direction = value;
        }
        
        public DirectionComponent(Vec2 direction)
        {
            this.direction = direction;
        }
    }
}
