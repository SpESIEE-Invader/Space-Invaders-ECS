using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// The Input component can be added to an entity 
    /// an entity implementing this component can listen for the player keypress,
    /// the HashSet <c>this.keyPressed</c> is the player keystroke for the current gameLoop
    /// </summary>
    class InputComponent
    {
        private HashSet<Keys> keyPressed;
        public HashSet<Keys> KeyPressed
        {
            get => this.keyPressed;
            set => this.keyPressed = value; 
        }
        
        public InputComponent()
        {
            keyPressed = new HashSet<Keys>();
        }
    }
}
