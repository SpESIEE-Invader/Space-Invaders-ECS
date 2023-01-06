using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// an entity is something that live in the scene, it can be anything 
    /// you can attach component to an entity to shape it 
    /// an entity doesn't have behaviors, it just act as a data structure 
    /// </summary>
    class Entity
    {
        private Dictionary<string, object> components;
        private int id;

        public Entity()
        {
            this.components = new Dictionary<string, object>();
        }

        public void Identify(int id)
        {
            this.id = id;
        }

        public int Id
        {
            get => this.id;
        }

        /// <summary>
        /// add a component 
        /// </summary>
        /// <param name="component"> an Object which is a *Component </param>
        public void RegisterComponent(object component)
        {  
            string name = component.GetType().Name;
            this.components[name] = component;
        }

        /// <summary>
        /// add multiple components
        /// </summary>
        /// <param name="components"> a set of objects which are really some *Component</param>
        public void RegisterComponents(params object[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                this.RegisterComponent(components[i]);        
            }
        }

        /// <summary>
        /// remove a component by ref
        /// </summary>
        /// <param name="component"> an Object which is a *Component </param>
        public void RemoveComponent(object component)
        {
            this.components.Remove(component.GetType().Name);
        }

        /// <summary>
        /// Get a component by the name of his class 
        /// </summary>
        /// <example>entity.GetComponent("PositionComponent") will return the entity PositionComponent</example>
        /// <param name="componentClass"> the name of the Component Class </param>
        /// <returns> the reference of the specified component </returns>
        public object GetComponent(string componentClass)
        {
            return this.components[componentClass];
        }

        /// <summary>
        /// check if the entity have the component specified by the name of its class 
        /// </summary>
        /// <param name="componentClass"> the name of the Component class </param>
        /// <returns> true if the entity posess the component else false </returns>
        public bool HasComponent(string componentClass)
        {
            return this.components.ContainsKey(componentClass);
        }

        /// <summary>
        /// check if the entity have a set of components specified by the name of their class 
        /// </summary>
        /// <param name="componentsString"> a list of Component class name </param>
        /// <returns>true if the entity posess all of the components else false </returns>
        public bool HasComponents(params string[] componentsString)
        {
            for (int i = 0; i < componentsString.Length; i++)
            {
                if (!this.HasComponent(componentsString[i]))
                    return false;
            }
            return true;
        }
    }
}
