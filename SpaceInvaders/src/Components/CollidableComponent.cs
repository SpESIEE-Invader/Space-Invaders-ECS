using SpaceInvaders.src;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    
    /// <summary>
    /// The Collidable component can be added to an entity 
    /// an entity implementing this component is collidable,
    /// this component is used to tell if the entity
    /// implementing it is currently intersecting with another entity
    /// </summary>
    class CollidableComponent
    {
        private bool hasCollision;
        private int numberOfPixelColliding; 
        private Entity collideWith;
        public CollidableComponent()
        {
            this.hasCollision = false;
            this.collideWith = null;
            this.numberOfPixelColliding = 0;
        }

        public bool HasCollision
        {
            get => this.hasCollision;
            set => this.hasCollision = value;
        }

        public Entity CollideWith
        {
            get => this.collideWith;
            set => this.collideWith = value;
        }

        public int NumberOfPixelColliding
        {
            get => this.numberOfPixelColliding;
            set => this.numberOfPixelColliding = value;
        }
    }
}