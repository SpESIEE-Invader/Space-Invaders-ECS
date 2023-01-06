using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class DeathSpriteComponent
    {
        private bool needToShowSprite;
        private bool needToRemoveEntity;
        private int timer;

        public DeathSpriteComponent()
        {
            this.needToShowSprite = false;
            this.needToRemoveEntity = false;
            this.timer = 50;
        }

        public bool NeedToShowSprite
        {
            get => this.needToShowSprite;
            set => this.needToShowSprite = value;
        }

        public int Timer
        {
            get => this.timer;
            set => this.timer = value;
        }

        public bool NeedToRemoveEntity
        {
            get => this.needToRemoveEntity;
            set => this.needToRemoveEntity = value;
        }
    }
}
