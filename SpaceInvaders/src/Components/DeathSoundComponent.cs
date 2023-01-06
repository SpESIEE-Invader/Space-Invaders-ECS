using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class DeathSoundComponent
    {

        private bool hasPlayed;

        public bool HasPlayed
        {
            get => this.hasPlayed;
            set => this.hasPlayed = value;
        }

        public DeathSoundComponent()
        {
            this.hasPlayed = false;            
        }
    }
}
