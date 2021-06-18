using System;
using System.Collections.Generic;
using System.Numerics;

namespace WasmBoids.Models
{
    public class Boid
    {
        public Vector2 Position { get; private set; }

        // angle from x axis (in radians), taking into account that higher Y values are down in canvas space
        public float Direction => MathF.Atan2(-Velocity.Y, Velocity.X);

        public Vector2 Velocity { get; set; }
        public string Color { get; private set; }

        public Boid(Vector2 position, Vector2 velocity, string color)
        {
            (Position, Velocity, Color) = (position, velocity, color);
        }

        // http://www.vergenet.net/~conrad/boids/pseudocode.html
        public void StepForward(double width, double height)
        {
            // TODO: more rules
            Velocity += Rule4(this, width, height);

            Position += Velocity;
        }

        // Flies toward 'center of mass' of boids.
        private static Vector2 Rule1(Boid boid, List<Boid> boids)
        {
            // TODO
            throw new NotImplementedException();
        }

        // Tries to avoid colliding with nearby boids.
        private static Vector2 Rule2(Boid boid, List<Boid> boids)
        {
            // TODO
            throw new NotImplementedException();
        }

        // Matches velocity with near boids.
        private static Vector2 Rule3(Boid boid, List<Boid> boids)
        {
            // TODO
            throw new NotImplementedException();
        }

        // Avoids hitting the edge
        private static Vector2 Rule4(Boid boid, double width, double height)
        {
            var edgeTolerance = Math.Min(width, height) * 0.1;

            var change = new Vector2(0, 0);
            if (boid.Position.X - 0 < edgeTolerance)
            {
                change.X += 1f / (boid.Position.X - 0);
            }

            if (width - boid.Position.X < edgeTolerance)
            {
                change.X -= 1f / (float) (width - boid.Position.X);
            }

            if (boid.Position.Y - 0 < edgeTolerance)
            {
                change.Y += 1f / (boid.Position.Y - 0);
            }

            if (height - boid.Position.Y < edgeTolerance)
            {
                change.Y -= 1f / (float) (height - boid.Position.Y);
            }

            return change;
        }
    }
}