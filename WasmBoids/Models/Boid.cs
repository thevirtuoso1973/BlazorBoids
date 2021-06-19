using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;

namespace WasmBoids.Models
{
    public class Boid : IEquatable<Boid>
    {
        public const float SightRadius = 100;

        public Vector2 Position { get; private set; }

        // angle from x axis (in radians), taking into account that higher Y values are down in canvas space
        public float Direction => MathF.Atan2(-Velocity.Y, Velocity.X);

        private Vector2 Velocity { get; set; }
        public string Color { get; }

        public Boid(Vector2 position, Vector2 velocity, string color)
        {
            (Position, Velocity, Color) = (position, velocity, color);
        }

        // http://www.vergenet.net/~conrad/boids/pseudocode.html
        public void StepForward(double width, double height, IReadOnlyCollection<Boid> boids)
        {
            Velocity += Rule1(this, boids);
            Velocity += Rule2(this, boids);
            Velocity += Rule3(this, boids);
            Velocity += Rule4(this, width, height);

            LimitVelocity();
            Position += Velocity;
        }

        private void LimitVelocity()
        {
            if (Velocity.Length() > 10)
            {
                Velocity = Velocity / Velocity.Length() * 10;
            }
        }

        [Pure]
        // Align with nearby boids.
        private static Vector2 Rule1(Boid boid, IReadOnlyCollection<Boid> boids)
        {
            const float effect = 16f;

            if (boids.Count <= 1) return Vector2.Zero;
            var combinedVelocity = boids.Where(currBoid =>
                    !currBoid.Equals(boid) && (currBoid.Position - boid.Position).Length() < SightRadius)
                .Select(currBoid => currBoid.Velocity).Aggregate(Vector2.Zero, (current, newV) => current + newV);
            if (combinedVelocity.Length() == 0)
            {
                return Vector2.Zero;
            }
            
            combinedVelocity = combinedVelocity / combinedVelocity.Length() * boid.Velocity.Length();
            var dotProd = Vector2.Dot(combinedVelocity, boid.Velocity);
            
            return (combinedVelocity - boid.Velocity) * MathF.Pow(1 + MathF.Exp(dotProd/effect), -1);
        }

        [Pure]
        // Tries to avoid colliding with nearby boids.
        private static Vector2 Rule2(Boid boid, IEnumerable<Boid> boids)
        {
            const float effect = 1f;
            var c = new Vector2(0, 0);
            return boids.Where(currBoid =>
                    !boid.Equals(currBoid) && (currBoid.Position - boid.Position).Length() < SightRadius)
                .Aggregate(c, (current, currBoid) =>
                {
                    var away = -(currBoid.Position - boid.Position);
                    var dist = away.Length();
                    away /= dist; // normalize it
                    return current + away / (dist * effect);
                });
        }

        [Pure]
        // Move to average position of local flock-mates.
        private static Vector2 Rule3(Boid boid, IEnumerable<Boid> boids)
        {
            var nearby = boids.Where(currBoid =>
                !boid.Equals(currBoid) && (currBoid.Position - boid.Position).Length() < SightRadius).ToArray();
            if (nearby.Length == 0) return Vector2.Zero;
            var avgNearbyPosition = nearby.Aggregate(Vector2.Zero,
                                            (current, currBoid) =>
                                                current + currBoid.Position) / nearby.Length;
            return (avgNearbyPosition - boid.Position) / avgNearbyPosition.Length();
        }

        [Pure]
        // Avoids hitting the edge
        private static Vector2 Rule4(Boid boid, double width, double height)
        {
            var edgeTolerance = Math.Min(width, height) * 0.1;
            const float effect = 0.005f;

            var change = new Vector2(0, 0);
            if (boid.Position.X - 0 < edgeTolerance)
            {
                change.X += effect * (float) (edgeTolerance - boid.Position.X);
            }

            if (width - boid.Position.X < edgeTolerance)
            {
                change.X -= effect * (float) (boid.Position.X - (width - edgeTolerance));
            }

            if (boid.Position.Y - 0 < edgeTolerance)
            {
                change.Y += effect * (float) (edgeTolerance - boid.Position.Y);
            }

            if (height - boid.Position.Y < edgeTolerance)
            {
                change.Y -= effect * (float) (boid.Position.Y - (height - edgeTolerance));
            }

            return change;
        }

        public bool Equals(Boid other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Position.Equals(other.Position) && Velocity.Equals(other.Velocity) && Color == other.Color;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Boid) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Velocity, Color);
        }
    }
}