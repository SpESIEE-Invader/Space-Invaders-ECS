using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Owner component can be added to an entity 
    /// an entity implementing this component is owned by another entity
    /// this component is used to track missile owner, there can be two owner 
    ///     1: Player 
    ///     2: Enemy
    /// </summary>
    class OwnerComponent
    {
        private string owner;
        
        public OwnerComponent(string owner)
        {
            this.owner = owner;
        }

        public string Owner
        {
            get => this.owner;
            set => this.owner = value;
        }
    }
}
