using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SpaceInvaders.src.Systems
{
    /// <summary>
    /// the hit system handle entity health change 
    /// </summary>
    class HitSystem : SuperSystem
    {
        public HitSystem(HashSet<Entity> allEntities, params string[] requirements)
            : base(allEntities, requirements) {}

        /// <summary>
        /// itereate among all eligible entities.
        /// if an entity is collidable and a collision with another entity occur
        /// update their health accordingly.
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                PositionComponent positionComponentAssociated = (PositionComponent) entity.GetComponent("PositionComponent");
                HealthComponent healthComponentAssociated = (HealthComponent) entity.GetComponent("HealthComponent");
                this.HandleHealthRemoval(entity);
                if (IsOutOfBoundVertical(positionComponentAssociated))
                    healthComponentAssociated.Health = 0;
            }
        }

        /// <summary>
        /// this function is called for every entity but we are only gonna execute it if the entity is a missile: why?
        /// because no other life removing collisions can occur except for missile / missile, missile / bunker, missile / spaceship
        /// to simplify things we are gonna perform the HealthRemoval function from the point of view of the missile colliding with something else 
        /// </summary>
        /// <param name="entity"> the entity currently checked </param>
        private void HandleHealthRemoval(Entity entity)
        {
            if (!entity.HasComponent("OwnerComponent"))
                return;
            CollidableComponent collidableComponent = (CollidableComponent) entity.GetComponent("CollidableComponent");
            if (!collidableComponent.HasCollision)
                return;
            HealthComponent healthComponent = (HealthComponent) entity.GetComponent("HealthComponent");
            Entity collidingWith = collidableComponent.CollideWith;
            /*
             */
            if (collidingWith.HasComponents("HealthComponent") && !collidingWith.HasComponent("OwnerComponent")) // the collision occur with a spaceship
            {
                HealthComponent collideWithHealthComponent = (HealthComponent) collidingWith.GetComponent("HealthComponent");
                int valueToRemove = Math.Min(collideWithHealthComponent.Health, healthComponent.Health);
                healthComponent.Health -= valueToRemove;
                collideWithHealthComponent.Health -= valueToRemove;
            }
            else if (collidingWith.HasComponent("OwnerComponent")) // the collision is with another missile 
            {
                healthComponent.Health = 0;
            } else // the collision occur with a bunker 
            {
                healthComponent.Health -= collidableComponent.NumberOfPixelColliding;
            }
        }

        /// <summary>
        /// determine if an entity is currently out of bound
        /// that is higher or lower than the camera
        /// </summary>
        /// <param name="positionComponent"> the entity <c>positionComponent</c> </param>
        /// <returns></returns>
        private bool IsOutOfBoundVertical(PositionComponent positionComponent)
        {
            return (positionComponent.Position.Y < 0 || positionComponent.Position.Y >= Game.windowSize.Height);
        }
    }
}
