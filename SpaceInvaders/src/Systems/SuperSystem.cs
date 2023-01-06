using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// the superSystem is the abstract implementation of a System
    /// </summary>
    abstract class SuperSystem
    {
        private string[] requirements;

        protected HashSet<Entity> entitesWithRequiredComponents;

        public SuperSystem(HashSet<Entity> allEntities, params string[] requirements)
        {
            this.requirements = requirements;
            this.UpdateEntitiesWithRequirement(allEntities);
        }

        /// <summary>
        /// updating the HashSet of entities <c>this.entitiesWithRequiredComponents</c>,
        /// keeping only entities that have components in the <c>this.requirements</c>
        /// </summary>
        /// <param name="allEntities"> list of all avaible entities in the entity manager </param>
        public void UpdateEntitiesWithRequirement(HashSet<Entity> allEntities)
        {
            this.entitesWithRequiredComponents = new HashSet<Entity>();
            foreach (Entity entity in allEntities)
            {
                bool isEntityWithRequiredComponents = true;
                foreach (string requirement in this.requirements)
                {
                    if (!entity.HasComponent(requirement))
                    {
                        isEntityWithRequiredComponents = false;
                        break;
                    }
                }
                if (isEntityWithRequiredComponents)
                    this.entitesWithRequiredComponents.Add(entity);
            }
        }

        /// <summary>
        /// this method need to be implemented by all system to add functionality
        /// </summary>
        public abstract void Update();
    }
}
