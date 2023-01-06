using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The Shoot component can be added to an entity 
    /// an entity implementing this component have the possibility to shoot
    /// that is to spawn missile in the same direction as the entity facing direction
    /// the firing entity cannot shoot anymore missile util the previous one die
    /// </summary>
    class ShootComponent
    {
        private Entity missile;
        private bool needToShoot;
        private bool canShoot;

        public bool NeedToShoot
        {
            get => this.needToShoot;
            set => this.needToShoot = value;
        }
        public bool CanShoot
        {
            get => this.canShoot;
            set => this.canShoot = value;
        }

        public Entity Missile
        {
            get => this.missile;
            set => this.missile = value;
        }

        public ShootComponent()
        {
            this.canShoot = true;
            this.needToShoot = false;
        }
    }
}
