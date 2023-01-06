using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceInvaders
{
    class Config
    {
        public static Color GAME_BACKGROUND = Color.White;
        public static Vec2 PLAYER_VELOCITY = new Vec2(200, 0);
        public static Vec2 PLAYER_MISSILE_VELOCITY = new Vec2(0, 400);
        public static Vec2 ENEMY_MISSILE_VELOCITY = new Vec2(0, 300);
        public static Vec2 ENEMY_VELOCITY = new Vec2(100, 100);
        public static int PLAYER_LIVES = 5;
        public static int ENEMY_LIVES = 3;
        public static int PLAYER_MISSILE_LIVES = 3;
        public static int ENEMY_MISSILE_LIVES = 1;
        public static Vec2 BLOCK_VELOCITY_SPEED_INCREASE = new Vec2(5, 0);
    }
}
