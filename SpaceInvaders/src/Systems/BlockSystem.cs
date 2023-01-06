using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// the BlockSystem is handling interaction with the block's enemies
    /// </summary>
    class BlockSystem: SuperSystem
    {
        private double previousBlockDirectionX;
        public BlockSystem(HashSet<Entity> allEntities, params string[] requirements)
            : base(allEntities, requirements)
        {
            this.previousBlockDirectionX = 1;
        }

        /// <summary>
        /// itereate among all eligible entities.
        /// check if one entity of the block is hitting a wall.
        /// if so, making the whole block changing direction and go faster 
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                this.HandleBlockCollision(entity);
                this.HandleTimer(entity);
                this.CheckForPlayerLoose(entity);
            } 
        }

        /// <summary>
        /// check if one entity of the block is hitting a wall.
        /// if so, making the whole block changing direction and go faster 
        /// </summary>
        /// <param name="entity"> the current entity </param>
        private void HandleBlockCollision(Entity entity)
        {
            if (this.GonnaBeOutOfBound(entity))
            {
                foreach (Entity otherEntity in base.entitesWithRequiredComponents)
                {
                    BlockConstituentComponent otherBlockConstituentComponent = (BlockConstituentComponent) otherEntity.GetComponent("BlockConstituentComponent");
                    otherBlockConstituentComponent.GoingDown = true;
                    this.UpdateDirectionTimerStarted(otherEntity);
                }
            }
        }

        /// <summary>
        /// the block is going down in a smooth way,
        /// update component to make this smooth transition happen 
        /// </summary>
        /// <param name="entity"> the current entity </param>
        private void HandleTimer(Entity entity)
        {
            BlockConstituentComponent blockConstituentComponent = (BlockConstituentComponent) entity.GetComponent("BlockConstituentComponent");
            if (!blockConstituentComponent.GoingDown)
                return;
            if (blockConstituentComponent.GoingDownTimer > 0) // the transition is occuring now
            {
                blockConstituentComponent.GoingDownTimer -= 1;
            }
            else // the transition is finished 
            {
                blockConstituentComponent.GoingDown = false;
                blockConstituentComponent.GoingDownTimer = blockConstituentComponent.GoingDownInitialTimer;
                this.UpdateVelocityDirectionTimerOver(entity);
            }
        }

        private void UpdateDirectionTimerStarted(Entity entity)
        {
            DirectionComponent directionComponent = (DirectionComponent) entity.GetComponent("DirectionComponent");
            this.previousBlockDirectionX = directionComponent.Direction.X;
            directionComponent.Direction.X = 0;
            directionComponent.Direction.Y = 1;

        }
        private void UpdateVelocityDirectionTimerOver(Entity entity)
        {
            DirectionComponent directionComponent = (DirectionComponent) entity.GetComponent("DirectionComponent");
            VelocityComponent velocityComponent = (VelocityComponent) entity.GetComponent("VelocityComponent");
            directionComponent.Direction.Y = 0;
            directionComponent.Direction.X = this.previousBlockDirectionX * -1;
            velocityComponent.Velocity += Config.BLOCK_VELOCITY_SPEED_INCREASE;
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
        /// if the block of enemies is to low on the screen, make the player loose the game 
        /// </summary>
        /// <param name="entity"> the enemy entity </param>
        private void CheckForPlayerLoose(Entity entity)
        {
            PositionComponent entityPositionComponent = (PositionComponent) entity.GetComponent("PositionComponent");
            if (entityPositionComponent.Position.Y >= Game.windowSize.Height - 180)
                Game.State = src.GameState.Lost;
        }
    }
}
