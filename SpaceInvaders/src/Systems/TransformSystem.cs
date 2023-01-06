using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows;
using System.Xml;

namespace SpaceInvaders
{
    /// <summary>
    /// the transform system make entity move accordingly to their velocity and position
    /// </summary>
    class TransformSystem : SuperSystem
    {
        public TransformSystem(HashSet<Entity> allEntities, params string[] requirements)
        : base(allEntities, requirements)
        {}

        /// <summary>
        /// itereate among all eligible entities.
        /// updating their position 
        /// the formula is new position = old position + direction * velocity * OOBModifier
        /// direction: {0, 1}
        /// OOBModifier: {0, 1}
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                DirectionComponent directionComponentAssociated = (DirectionComponent) entity.GetComponent("DirectionComponent");
                VelocityComponent speedComponentAssociated = (VelocityComponent) entity.GetComponent("VelocityComponent");
                PositionComponent positionComponentAssociated = (PositionComponent) entity.GetComponent("PositionComponent");
                positionComponentAssociated.Position += directionComponentAssociated.Direction * speedComponentAssociated.Velocity * this.OutOfBoundModifier(entity) * Game.deltaT;
            }
        }

        /// <summary>
        /// determine if the current entity gonna be out of bound 
        /// that is out of the screen in the next game loop.
        /// </summary>
        /// <param name="entity"> the current entity </param>
        /// <returns> true if the entity gonna be out of bound else false </returns>
        private bool GonnaBeOutOfBound(Entity entity) {
            DirectionComponent directionComponent = (DirectionComponent) entity.GetComponent("DirectionComponent");
            PositionComponent positionComponent = (PositionComponent) entity.GetComponent("PositionComponent");
            HitboxComponent hitboxComponent = (HitboxComponent) entity.GetComponent("HitboxComponent");
            return
                (
                    positionComponent.Position.X <= 0 && directionComponent.Direction.X < 0
                    ||
                    positionComponent.Position.X + hitboxComponent.Hitbox.Width >= Game.windowSize.Width && directionComponent.Direction.X > 0
                );
        }

        /// <summary>
        /// calculate the out of bound modifier, making the position change 0 if the entity gonna be out of bounds
        /// </summary>
        /// <param name="entity"> the current entity </param>
        /// <returns> 0 if the entity is gonna be OOB else 1 </returns>
        private int OutOfBoundModifier(Entity entity)
        {
            return GonnaBeOutOfBound(entity) == true ? 0 : 1;
        }
    }
}
