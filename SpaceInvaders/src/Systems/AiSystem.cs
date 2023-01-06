using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// the AiSystem is responsible for making enemy fire missile
    /// </summary>
    class AiSystem : SuperSystem
    {
        private Random pseudoRandomClient;
        public AiSystem(HashSet<Entity> allEntities, params string[] requirements)
            : base(allEntities, requirements)
        {
            this.pseudoRandomClient = new Random();
        }
    
        /// <summary>
        /// itereate among all eligible entities.
        /// update their shootComponent is they can fire
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                ShootComponent shootComponentAssociated = (ShootComponent) entity.GetComponent("ShootComponent");
                PositionComponent positionComponentAssociated = (PositionComponent) entity.GetComponent("PositionComponent");
                double chanceToFire = GetFireChancePercentage(positionComponentAssociated);
                if (this.pseudoRandomClient.NextDouble() <= chanceToFire / 100)
                {
                    shootComponentAssociated.NeedToShoot = true;
                    shootComponentAssociated.CanShoot = false;
                }
            }
        }

        /// <summary>
        /// calculate the chance for a enemy to fire a missile, in percentage
        /// </summary>
        /// <param name="positionComponent"> the entity position (here the enemy position) </param>
        /// <returns>0 <c> finalChance </c> < 100</returns>
        private double GetFireChancePercentage(PositionComponent positionComponent)
        {
            double initialChance = 0.03;
            double modifierChance = (positionComponent.Position.Y * 100) / Game.windowSize.Height; // ratio height of the enemy / height of the screeen
            double modifier = 0.1;
            double finalChance = initialChance * modifierChance * modifier;
            return finalChance;
        }
    }
}
