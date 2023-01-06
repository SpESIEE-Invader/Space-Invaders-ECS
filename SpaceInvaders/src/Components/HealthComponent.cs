using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Health component can be added to an entity 
    /// an entity implementing this component has health, when reaching 0, the entity no longer exist in the game.
    /// </summary>
    class HealthComponent
    {
        private int health;
        public int Health
        {
            get => this.health;
            set => this.health = value;
        }

        public HealthComponent(int health)
        {
            this.health = health;
        }
    }
}
