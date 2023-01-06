using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    /// <summary>
    /// the hand box sync system is used to sync between an entity hitbox and the entity position 
    /// </summary>
    class HitboxSyncSystem: SuperSystem
    {
        public HitboxSyncSystem(HashSet<Entity> allEntities, params string[] requirements)
            : base(allEntities, requirements) { }

        /// <summary>
        /// itereate among all eligible entities.
        /// creating a new Rectangle at the entity positon, reseting the hitbox of the hitboxComponent 
        /// </summary>
        public override void Update()
        {
            foreach (Entity entity in base.entitesWithRequiredComponents)
            {
                HitboxComponent hitboxComponentAssociated = (HitboxComponent) entity.GetComponent("HitboxComponent");
                PositionComponent positionComponentAssociated = (PositionComponent) entity.GetComponent("PositionComponent");
                HitboxComponent newHitBoxComponent = new HitboxComponent
                    (
                        positionComponentAssociated.Position,
                        hitboxComponentAssociated.Hitbox.Width,
                        hitboxComponentAssociated.Hitbox.Height
                    );
                entity.RemoveComponent(hitboxComponentAssociated);
                entity.RegisterComponent(newHitBoxComponent);
            }
        }
    }
}
