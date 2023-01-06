using SpaceInvaders.src;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace SpaceInvaders
{
    /// <summary>
    /// the shoot system listen for the shootComponent of an entity 
    /// and make the entity fire a missile if it can 
    /// </summary>
    class ShootSystem : SuperSystem
    {
        private EntityManager entityManager; // this system need to have acces to the coordinator instance of the entityManager to spawn missiles 

        public ShootSystem(HashSet<Entity> allEntities, EntityManager entityManager, params string[] requirements)
            : base(allEntities, requirements)
        {
            this.entityManager = entityManager;
        }

        /// <summary>
        /// itereate among all eligible entities.
        /// update the inputComponent, indicate that the player can shoot, change the player direction if needed
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                ShootComponent shootComponentAssociated = (ShootComponent) entity.GetComponent("ShootComponent");
                if (!this.entityManager.Entities.Contains(shootComponentAssociated.Missile))
                    shootComponentAssociated.Missile = null;
                if (shootComponentAssociated.NeedToShoot && shootComponentAssociated.Missile == null)
                {
                    Entity missile = CreateMissile(entity);
                    shootComponentAssociated.Missile = missile;
                    this.entityManager.AddEntity(missile);
                }
                shootComponentAssociated.NeedToShoot = false;
            }
        }

        /// <summary>
        /// create the missile and make it spawn by adding it to the entityManager
        /// this missile can be fired by an enemy or the player 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private Entity CreateMissile(Entity entity)
        {
            FacingComponent entityFacingComponent = (FacingComponent) entity.GetComponent("FacingComponent");
            Entity missile = GetBasicMissileSkeleton();
            Bitmap missileSprite = Properties.Resources.shoot1;
            Vec2 missilePosition = GetMissilePosition(entity);
            Vec2 missileVelocity = GetMissileVelocity(entity);
            missile.RegisterComponents
                (
                    new PositionComponent(missilePosition),
                    new HitboxComponent(missilePosition, missileSprite.Width, missileSprite.Height),
                    new DirectionComponent
                    (
                        new Vec2(0, entityFacingComponent.Facing)
                    ),
                    new VelocityComponent(missileVelocity),
                    GetOwnerComponent(entity),
                    new HealthComponent(GetMissileHealth(entity))
                );
            this.entityManager.AddEntity(missile);
            if (entityFacingComponent.Facing == -1)
                Game.playerShootingSoundPlayer.Play();
            return missile;
        }

        /// <summary>
        /// get the health of the missile depending of which entity is firing it, entity in {player, enemy}
        /// </summary>
        /// <param name="entity"> the entity firing the missile </param>
        /// <returns> the amount of health for the missile </returns>
        private int GetMissileHealth(Entity entity)
        {
            return entity.HasComponent("BlockConstituentComponent") ? Config.ENEMY_MISSILE_LIVES : Config.PLAYER_MISSILE_LIVES;
        }

        /// <summary>
        /// create a basic missile skeleton,
        /// that is a missile which has only the components that do not depend on the entity that fires the missile 
        /// </summary>
        /// <returns> an Entity representing the missile </returns>
        private Entity GetBasicMissileSkeleton()
        {
            Entity missile = new Entity();
            missile.RegisterComponents
                (
                    new SpriteComponent(Properties.Resources.shoot1),
                    new CollidableComponent()
                );
            return missile;
        }

        private Vec2 GetMissileVelocity(Entity entity)
        {
            return entity.HasComponents("BlockConstituentComponent") ? Config.ENEMY_MISSILE_VELOCITY : Config.PLAYER_MISSILE_VELOCITY;
        }

        /// <summary>
        /// create a new OwnerComponent depending of the entity firing the missile
        /// </summary>
        /// <param name="entity"> the current entity firing </param>
        /// <returns> a new OwnerComponent with the good owner specified in it </returns>
        private OwnerComponent GetOwnerComponent(Entity entity)
        {
            string owner;
            if (entity.HasComponent("BlockConstituentComponent"))
                owner = "enemy";
            else
                owner = "player";
            return new OwnerComponent(owner);
        }

        /// <summary>
        /// calculate the missile position and returning it
        /// </summary>
        /// <param name="entity"></param>
        /// <returns> a new Vec2 depicting the missile position depending of the entity firing the missile </returns>
        private Vec2 GetMissilePosition(Entity entity)
        {
            FacingComponent facingComponent = (FacingComponent) entity.GetComponent("FacingComponent");
            PositionComponent positionComponent = (PositionComponent) entity.GetComponent("PositionComponent");
            HitboxComponent hitboxComponent = (HitboxComponent) entity.GetComponent("HitboxComponent");
            double y = facingComponent.Facing == 1 ? hitboxComponent.Hitbox.Height : -Properties.Resources.shoot1.Height - 1;
            return positionComponent.Position + new Vec2(hitboxComponent.Hitbox.Width / 2, y);
        }
    }
}
