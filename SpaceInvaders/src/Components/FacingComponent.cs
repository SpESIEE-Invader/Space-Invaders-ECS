using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Facing component can be added to an entity 
    /// an entity implementing this component is facing a direction,
    /// this component is used to tell which direction an entity is facing 
    /// an entity can face two different direction:
    ///     north: <c>this.facing = -1</c>.
    ///     south: <c>this.facing = 1</c>
    /// </summary>
    class FacingComponent
    {
        private int facing; 

        public FacingComponent(int facing)
        {
            this.facing = facing;
        }

        public int Facing
        {
            get => this.facing;
            set => this.facing = value;
        }
    }
}
