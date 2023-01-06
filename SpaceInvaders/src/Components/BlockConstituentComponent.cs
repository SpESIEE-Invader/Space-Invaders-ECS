using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// The BlockConstituent component can be added to an entity 
    /// an entity implementing this component is a enemy member of the main block
    /// </summary>
    class BlockConstituentComponent
    {
        private bool goingDown;
        private int goingDownTimer;
        private int goingDownInitialTimer;

        public BlockConstituentComponent()
        {
            this.goingDown = false;
            this.goingDownInitialTimer = 10;
            this.goingDownTimer = this.goingDownInitialTimer;
        }

        public bool GoingDown
        {
            get => this.goingDown;
            set => this.goingDown = value;
        }

        public int GoingDownTimer
        {
            get => this.goingDownTimer;
            set => this.goingDownTimer = value;
        } 

        public int GoingDownInitialTimer
        {
            get => this.goingDownInitialTimer;
            set => this.goingDownInitialTimer = value;
        }
    }
}
