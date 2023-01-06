using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders.src.Systems
{
    /// <summary>
    /// the death system handle removing entities with no health remaining 
    /// it gonna also handle sprite change and death sprite timer for a dead entity
    /// </summary>
    class DeathSystem : SuperSystem
    {
        private EntityManager entityManager;

        public DeathSystem(HashSet<Entity> allEntities, EntityManager entityManager, params string[] requirements)
            : base(allEntities, requirements)
        {
            this.entityManager = entityManager;
        }

        /// <summary>
        /// itereate among all eligible entities.
        /// if the current entity health is lesss than zero, the entity is removed from the entity manager 
        /// update the score of the player when a enemy die 
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                if (!NeedToRemoveEntity(entity))
                    continue;
                this.PlayEnemyDeathSound(entity);
                this.ActivateDeathSprite(entity);
                if (this.CanDie(entity)) // the moment when the enemy really dissappear from the game 
                {
                    this.entityManager.RemoveEntity(entity);
                    this.UpdatePlayerScore(entity);
                }
                this.HandleDeathSpriteTimer(entity);
            }
        }

        private void UpdatePlayerScore(Entity entity)
        {
            if (entity.HasComponents("BlockConstituentComponent"))
                Leaderboard.Instance.UpdateScore(20);
        }

        private void PlayEnemyDeathSound(Entity entity)
        {
            if (!entity.HasComponents("BlockConstituentComponent", "DeathSoundComponent"))
                return;
            DeathSoundComponent entityDeathSound = (DeathSoundComponent) entity.GetComponent("DeathSoundComponent");
            if (entityDeathSound.HasPlayed)
                return;
            Game.enemyDyingSoundPlayer.Play();
            entityDeathSound.HasPlayed = true; 
        }

        /// <summary>
        /// check if the entity can die,
        /// if the entity does not posess a death sprite, it can die now 
        /// if the entity posess a death sprite, chech if game have showed this sprite to the player 
        /// </summary>
        /// <param name="entity"> the current entity </param>
        /// <returns> true if the entity can be removed from the game else false </returns>
        private bool CanDie(Entity entity)
        {
            if (!entity.HasComponent("DeathSpriteComponent"))
                return true;
            DeathSpriteComponent entityDeathSpriteComponent = (DeathSpriteComponent) entity.GetComponent("DeathSpriteComponent");
            return entityDeathSpriteComponent.NeedToRemoveEntity;
        }

        /// <summary>
        /// Handle the death sprite timer, to show the death sprite for a period of time 
        /// </summary>
        /// <param name="entity"> current entity </param>
        private void HandleDeathSpriteTimer(Entity entity)
        {
            if (!entity.HasComponent("DeathSpriteComponent"))
                return;
            DeathSpriteComponent entityDeathSpriteComponent = (DeathSpriteComponent) entity.GetComponent("DeathSpriteComponent");
            if (!entityDeathSpriteComponent.NeedToShowSprite)
                return;
            if (entityDeathSpriteComponent.Timer >= 0)
                entityDeathSpriteComponent.Timer -= 1;
            else
                entityDeathSpriteComponent.NeedToRemoveEntity = true;
        }

        /// <summary>
        /// when the <c>entity</c> is dead, this will activate the sprite to the screen
        /// and starting the timer 
        /// </summary>
        /// <param name="entity"> the current entity we know is virtually dead </param>
        private void ActivateDeathSprite(Entity entity)
        {
            if (!entity.HasComponent("DeathSpriteComponent"))
                return;
            DeathSpriteComponent entityDeathSpriteComponent = (DeathSpriteComponent) entity.GetComponent("DeathSpriteComponent");
            if (entityDeathSpriteComponent.NeedToShowSprite)
                return;
            SpriteComponent entitySpriteComponent = (SpriteComponent) entity.GetComponent("SpriteComponent");
            entityDeathSpriteComponent.NeedToShowSprite = true;
            entitySpriteComponent.Sprite = Properties.Resources.explosion1; 
        }

        /// <summary>
        /// check if an entity is dead and need to be removed from the game 
        /// </summary>
        /// <param name="entity"> the current entity </param>
        /// <returns> true if the entity need to be removed from the game else false </returns>
        private bool NeedToRemoveEntity(Entity entity)
        {
            HealthComponent healthComponent = (HealthComponent) entity.GetComponent("HealthComponent");
            return healthComponent.Health <= 0;
        }
    }
}
