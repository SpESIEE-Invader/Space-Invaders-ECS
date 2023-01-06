using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// the entity manager handle entities that exist in the scene
    /// it is implemented following the singleton pattern 
    /// </summary>
    class EntityManager
    {
        public static EntityManager Instance { get; set; }

        private HashSet<Entity> entities;
        public int currentEntityId;
        public HashSet<Entity> Entities => this.entities;

        private EntityManager()
        {
            this.entities = new HashSet<Entity>();
            this.currentEntityId = 0;
        }

        public static EntityManager CreateEntityManager()
        {
            if (Instance == null)
                Instance = new EntityManager();
            return Instance;
        }

        /// <summary>
        /// add an entity to the list of entities
        /// </summary>
        /// <param name="entity"> the entity Object to add </param>
        public void AddEntity(Entity entity)
        {
            entity.Identify(this.currentEntityId++);
            this.entities.Add(entity);
        }

        /// <summary>
        /// remove an entity to the list of entites 
        /// </summary>
        /// <param name="entity"> the entity Object to remove </param>
        public void RemoveEntity(Entity entity)
        {
            this.entities.Remove(entity);
        }

        /// <summary>
        /// get a specific entity by it ID
        /// be carefull by using this method and ensure that you know the id of the entity,
        /// the programm will surely crash because a null will be thrown to the caller 
        /// </summary>
        /// <param name="id"> the id of the entity </param>
        /// <returns> the entity if found else null </returns>
        public Entity GetEntityById(int id)
        {
            foreach (Entity entity in this.entities)
            {
                if (entity.Id == id)
                    return entity;
            }
            return null;
        }
    }
}
