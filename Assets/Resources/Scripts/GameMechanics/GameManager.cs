using System.Collections;
using System.Collections.Generic;
using GameEntities;
using System.Numerics;
using Physics;

public class GameManager
{
    Player player;
    public int score { get; private set; } = 0;
    float gameIntensity;
    float desiredIntensity = 12;
    float asteroidsSize;
    float aliensSize;
    public bool gameEnded;

    public GameManager(Player p, float asteroidsRadius, float aliensRadius)
    {
        player = p;
        player.gm = this;
        asteroidsSize = asteroidsRadius;
        aliensSize = aliensRadius;
    }

    public void entityDied(Entity e)
    {
        switch ((objectTypes)e.objectType())
        {
            case objectTypes.asteroid:
                if(((Asteroid)e).grade == 1)
                {
                    gameIntensity -= 1;
                }
                else
                {
                    gameIntensity += 2;
                }
                score += ((Asteroid)e).grade * 25;
                break;
            case objectTypes.enemy:
                score += 100;
                gameIntensity -= 5;
                break;
            case objectTypes.player:
                gameEnded = true;
                break;
        }
    }

    public List<Entity> Update(float deltaTime)
    {
        if (gameEnded)
            return null;
        List<Entity> tospawn = new List<Entity>();
        desiredIntensity += deltaTime*2;
        System.Random rng = new System.Random();
        while (gameIntensity < desiredIntensity)
        {
            Vector2 pos;
            while (true)
            {
                pos.X = PhysicsEngine.WorldStart.X + rng.Next((int)(PhysicsEngine.WorldEnd.X - PhysicsEngine.WorldStart.X));
                pos.Y = PhysicsEngine.WorldStart.Y + rng.Next((int)(PhysicsEngine.WorldEnd.Y - PhysicsEngine.WorldStart.Y));
                if (PhysicsEngine.Vec2Magnitude(pos - player.getPosition()) > 2){
                    break;
                }
            }
            int type = rng.Next() % 2;
            if(type == 0)
            {
                int randomAngle = rng.Next(360);
                Vector2 randomDir = new Vector2((float)System.Math.Cos(randomAngle), (float)System.Math.Sin(randomAngle));
                tospawn.Add(new Asteroid(pos, randomDir, Vector2.Zero, asteroidsSize, this));
                gameIntensity += 6;
            }
            else
            {
                tospawn.Add(new UFO(pos, player, aliensSize, this));
            }
        }
        return tospawn;
    }
}
