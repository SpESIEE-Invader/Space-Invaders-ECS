using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.src.Systems
{
    /// <summary>
    /// the Collidable handle collision between collidable entity 
    /// </summary>
    class CollidableSystem : SuperSystem
    {
        public HashSet<Entity> allEntities;
        public CollidableSystem(HashSet<Entity> allEntities, params string[] requirements)
            : base(allEntities, requirements)
        {
            this.allEntities = allEntities;
        }

        /// <summary>
        /// itereate among all eligible entities.
        /// perform various check to see if the entity is colliding with another entity
        /// update their component in conseqeuence 
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                CollidableComponent collidableComponentAssociated = (CollidableComponent) entity.GetComponent("CollidableComponent");
                HitboxComponent hitboxComponentAssociated = (HitboxComponent) entity.GetComponent("HitboxComponent");
                SpriteComponent spriteComponentAssociated = (SpriteComponent) entity.GetComponent("SpriteComponent");
                collidableComponentAssociated.HasCollision = false;
                foreach (Entity otherEntity in this.allEntities) 
                {
                    if (otherEntity != entity && otherEntity.HasComponents("CollidableComponent", "HitboxComponent"))
                    {
                        HitboxComponent otherHitboxComponent = (HitboxComponent) otherEntity.GetComponent("HitboxComponent");
                        Rectangle unionRectangle = Rectangle.Intersect(hitboxComponentAssociated.Hitbox, otherHitboxComponent.Hitbox);
                        if (GiveUpCollisions(entity, otherEntity))
                            continue;
                        if (unionRectangle.IsEmpty)
                            continue;
                        int numberOfPixelColliding = GetNumberOfPixelColliding(entity, otherEntity);
                        if (numberOfPixelColliding == 0)
                            continue;
                        collidableComponentAssociated.HasCollision = true;
                        collidableComponentAssociated.CollideWith = otherEntity;
                        this.UpdateNumberOfPixelColliding(numberOfPixelColliding, entity, otherEntity);
                    }
                }
            }
        }

        /// <summary>
        /// chagne the Collidable component properties for the entity and the other entity we are checking the collision with
        /// </summary>
        /// <param name="numberOfPixelColliding"> the number of pixel colliding for the current collision between the two entities </param>
        /// <param name="entity"> the entity </param>
        /// <param name="otherEntity"> the other entity </param>
        private void UpdateNumberOfPixelColliding(int numberOfPixelColliding, Entity entity, Entity otherEntity)
        {
            CollidableComponent otherEntityCollidableComponent = (CollidableComponent) otherEntity.GetComponent("CollidableComponent");
            CollidableComponent collidableComponent = (CollidableComponent) entity.GetComponent("CollidableComponent");
            otherEntityCollidableComponent.NumberOfPixelColliding = numberOfPixelColliding;
            collidableComponent.NumberOfPixelColliding = numberOfPixelColliding;
        }

        /// <summary>
        /// if two hitbox intersect, it's time for a In Depth Check
        /// InDepth check is a pixex by pixel check to see if two entities are collising
        /// </summary>
        /// <param name="unionRectangle"> the intersection between the two colliding hitbox </param>
        /// <param name="entity"> the current entity </param>
        /// <param name="otherEntity"> the other entity we are checking the collision with </param>
        /// <returns> true if one pixel of the first entity is colliding with a pixel of the second entity else false </returns>
        private int GetNumberOfPixelColliding(Entity entity, Entity otherEntity)
        {
            int result = 0;
            if (entity.HasComponent("OwnerComponent")) // the entity is a missile 
            {
                HitboxComponent missileHitboxComponent = (HitboxComponent) entity.GetComponent("HitboxComponent");
                SpriteComponent otherObjectSpriteComponent = (SpriteComponent) otherEntity.GetComponent("SpriteComponent");
                for (int row = 0; row < missileHitboxComponent.Hitbox.Height; row++)
                {
                    for (int col = 0; col < missileHitboxComponent.Hitbox.Width; col++)
                    {
                        Vec2 missilePointOtherObjectReference = ReferenceChange(entity, otherEntity, row, col);
                        if (IsPixelInsideOtherReference(missilePointOtherObjectReference, otherEntity) && !IsShowingDeathSprite(otherEntity))
                        {
                            if (otherObjectSpriteComponent.Sprite.GetPixel(
                                (int) Math.Round(missilePointOtherObjectReference.X),
                                (int) Math.Round(missilePointOtherObjectReference.Y)
                                ).ToArgb() == Color.Black.ToArgb())
                            {
                                AddPixelToBeSwitch(otherEntity, missilePointOtherObjectReference);
                                result++;       
                            }
                        }
                    }
                }
            } 
            return result;
        }

        /// <summary>
        /// this function is to avoid crash when checking for collision,
        /// the death sprite of a enemy is smaller than the enemy sprite,
        /// we need to avoid the collision if the enemy is virtually dead but not yet removed from the entity manager 
        /// </summary>
        /// <param name="entity"> the current entity </param>
        /// <returns></returns>
        private bool IsShowingDeathSprite(Entity entity)
        {
            if (!entity.HasComponent("DeathSpriteComponent"))
                return false;
            DeathSpriteComponent entityDeathSpriteComponent = (DeathSpriteComponent)entity.GetComponent("DeathSpriteComponent");
            return entityDeathSpriteComponent.NeedToShowSprite;
        }

        /// <summary>
        /// this function will perform two reference change: 
        ///     1: MissilePoint (Missile Reference) -> MissilePoint (Screen Reference) 
        ///     2: MissilePoint (Screen Reference) -> MissilePoint (other Object Reference)
        /// </summary>
        /// <param name="entity"> the missile entity </param>
        /// <param name="otherEntity"> the other object entity </param>
        /// <param name="pixelMissileRow"> the row of the pixel checked in the missile reference </param>
        /// <param name="pixelMissileCol"> the col of the pixel checked in the missile reference </param>
        /// <returns></returns>
        private Vec2 ReferenceChange(Entity entity, Entity otherEntity, int pixelMissileRow, int pixelMissileCol)
        {
            PositionComponent missilePositionScreenReferenceComponent = (PositionComponent) entity.GetComponent("PositionComponent");
            PositionComponent otherEntityScreenReferencePositionComponent = (PositionComponent) otherEntity.GetComponent("PositionComponent");
            Vec2 missilePositionScreenReference = missilePositionScreenReferenceComponent.Position;
            Vec2 missilePointMissileReference = new Vec2(pixelMissileCol, pixelMissileRow);
            Vec2 missilePointScreenReference = missilePositionScreenReference + missilePointMissileReference;
            Vec2 missilePointBunkerReference =  missilePointScreenReference - otherEntityScreenReferencePositionComponent.Position;
            return missilePointBunkerReference;
        }

        /// <summary>
        /// to avoid Exception when performing an in Depth check for collisions 
        /// we need to ensure that the pixel whe check is inside the other Object Reference
        /// </summary>
        /// <param name="pointPositionOtherReference"> the pixel to check in the other object reference </param>
        /// <param name="otherEntity"> the other object entity </param>
        /// <returns></returns>
        private bool IsPixelInsideOtherReference(Vec2 pointPositionOtherReference, Entity otherEntity)
        {
            int pointPositionOtherReferenceX = Convert.ToInt32(pointPositionOtherReference.X);
            int pointPositionOtherReferenceY = Convert.ToInt32(pointPositionOtherReference.Y);
            HitboxComponent otherEntityHitboxComponent = (HitboxComponent) otherEntity.GetComponent("HitboxComponent");
            if
                (
                        pointPositionOtherReferenceY >= 0
                    &&  pointPositionOtherReferenceX < otherEntityHitboxComponent.Hitbox.Width - 1
                    &&  pointPositionOtherReferenceY < otherEntityHitboxComponent.Hitbox.Height
                    &&  pointPositionOtherReferenceX >= 1
                )
                return true;
            return false;
        }

        /// <summary>
        /// add a pixel to be switched to the other entity (the entity colliding with a missile)
        /// </summary>
        /// <param name="otherEntity"> the other entity (the entity colliding with a missile) </param>
        /// <param name="pointToSwitch"> the Vec2(x,y) pixel position </param>
        private void AddPixelToBeSwitch(Entity otherEntity, Vec2 pointToSwitch)
        {
            if (otherEntity.HasComponent("PixelToSwitchComponent"))
            {
                PixelToSwitchComponent otherObjectPixelToSwitchComponent = (PixelToSwitchComponent) otherEntity.GetComponent("PixelToSwitchComponent");
                otherObjectPixelToSwitchComponent.PixelPositions.Add(pointToSwitch.Clone());
            }
        }

        /// <summary>
        /// sometimes in the game, we need to avoid a collision
        /// for exemple, to prevent for friendly fire among enemies
        /// that is, when a enemy missile hit another enemy
        /// when this is the case, the collision check need to be ignored and we skip the entity
        ///
        /// we need to give up the collision in the following situations
        ///     1: the current entity checked is a missile, owned by a enemy and colliding with a enemy
        ///     2: the current entity is a enemy, colliding with a missile owned by another enemy
        /// </summary>
        /// <param name="entity"> the current entity </param>
        /// <param name="otherEntity"> the other entity we are checking the collsion with </param>
        /// <returns> true if we need to abort the collision and skip the entity else false </returns>
        private bool GiveUpCollisions(Entity entity, Entity otherEntity)
        {
            // 1 case 
            if (entity.HasComponent("OwnerComponent")) // the entity is a missile
            {
                OwnerComponent ownerComponent = (OwnerComponent) entity.GetComponent("OwnerComponent");
                if (otherEntity.HasComponent("BlockConstituentComponent") && ownerComponent.Owner.Equals("enemy"))
                    return true; 
            } else // 2 case 
            {
                if (otherEntity.HasComponent("OwnerComponent") && entity.HasComponent("BlockConstituentComponent"))
                {
                    OwnerComponent otherOwnerComponent = (OwnerComponent) otherEntity.GetComponent("OwnerComponent");
                    if (otherOwnerComponent.Owner.Equals("enemy"))
                        return true;
                }
            }
            return false;
        }
    }
}
