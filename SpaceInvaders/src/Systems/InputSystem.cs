using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SpaceInvaders
{
    /// <summary>
    /// the input system handle player keystrokes
    /// </summary>
    class InputSystem : SuperSystem
    {
        public InputSystem(HashSet<Entity> allEntities, params string[] requirements)
            : base(allEntities, requirements) { }

        /// <summary>
        /// itereate among all eligible entities.
        /// update the inputComponent, indicate that the player can shoot, change the player direction if needed
        /// </summary>
        public override void Update()
        {
            foreach(Entity entity in base.entitesWithRequiredComponents)
            {
                this.UpdateKeys(entity);
                this.UpdateDirection(entity);
                this.UpdateShoot(entity);
            }
        }

        /// <summary>
        /// update the <c>inputComponent</c> to the current state
        /// </summary>
        /// <param name="entity"> the current entity </param>
        private void UpdateKeys(Entity entity)
        {
            InputComponent inputComponent = (InputComponent) entity.GetComponent("InputComponent");
            inputComponent.KeyPressed = Game.keyPressed;
        }

        /// <summary>
        /// update the player direction 
        /// </summary>
        /// <param name="entity"> the current entity </param>
        private void UpdateDirection(Entity entity)
        {
            DirectionComponent directionComponentAssociated = (DirectionComponent) entity.GetComponent("DirectionComponent");
            int positionOffset = ChangeDirections(entity, Keys.Right, 1) + ChangeDirections(entity, Keys.Left, -1);
            directionComponentAssociated.Direction = new Vec2(positionOffset, 0);
        }

        /// <summary>
        /// if the player press SPACE
        /// tell the shoot component that the player can shoot 
        /// so the shoot system can make the player fire a missile next game loop
        /// </summary>
        /// <param name="entity"> the current entity </param>
        private void UpdateShoot(Entity entity)
        {
            InputComponent inputComponent = (InputComponent) entity.GetComponent("InputComponent");
            ShootComponent shootComponent = (ShootComponent) entity.GetComponent("ShootComponent");
            if (inputComponent.KeyPressed.Contains(Keys.Space) && shootComponent.CanShoot)
            {
                shootComponent.CanShoot = false;
                shootComponent.NeedToShoot = true;
            } 
            else
                if (!(inputComponent.KeyPressed.Contains(Keys.Space)))
                    shootComponent.CanShoot = true;
        }

        /// <summary>
        /// change the <c>directionComponent</c> accordingly to the player keystroke 
        /// </summary>
        /// <param name="entity"> the current entity </param>
        /// <param name="key">a key, for exemple Arrow left</param>
        /// <param name="direction"> -1 if left, 1 is right </param>
        /// <returns> the new player direction </returns>
        private int ChangeDirections(Entity entity, Keys key, int direction)
        {
            InputComponent inputComponent = (InputComponent) entity.GetComponent("InputComponent");
            if (inputComponent.KeyPressed.Contains(key))
                return direction;
            return 0;
        }
    }
}
