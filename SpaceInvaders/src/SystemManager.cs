using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// the system manager handle systems
    /// it is implemented following the singleton pattern
    /// </summary>
    class SystemManager
    {
        public static SystemManager Instance { get; private set; }
        private List<SuperSystem> systems;

        private SystemManager()
        {
            this.systems = new List<SuperSystem>();
        }

        public static SystemManager CreateSystemManager()
        {
            if (Instance == null)
                Instance = new SystemManager();
            return Instance;
        }

        /// <summary>
        /// add a System to the manager 
        /// the system must be an implementation of a superSystem to override behavior
        /// </summary>
        /// <param name="system"> the system to add </param>
        public void AddSystem(SuperSystem system)
        {
            this.systems.Add(system);
        }

        /// <summary>
        /// add multiple system to the manager 
        /// </summary>
        /// <param name="systems"> the list of systems to add </param>
        public void AddSystems(params SuperSystem[] systems)
        {
            for (int i = 0; i < systems.Length; i++)
            {
                this.AddSystem(systems[i]);
            }
        }

        /// <summary>
        /// update each of the systems contained in the manager 
        /// updating a system consist of two part:
        ///     1: updating the list of entities with required component 
        ///     2: call the overrided update method of the system
        /// </summary>
        /// <param name="allEntities"> the current list of all entities that live in the scene </param>
        public void Update(HashSet<Entity> allEntities)
        {
            foreach (SuperSystem system in this.systems)
            {
                system.UpdateEntitiesWithRequirement(allEntities);
                system.Update();
            }
        }
    }
}
