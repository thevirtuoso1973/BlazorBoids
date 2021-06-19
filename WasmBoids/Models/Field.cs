using System;
using System.Collections.Generic;
using System.Numerics;

namespace WasmBoids.Models
{
    public class Field
    {
        public readonly List<Boid> Boids = new();
        public double Width { get; private set; }
        public double Height { get; private set; }

        public void Resize(double width, double height) =>
            (Width, Height) = (width, height);

        public Field(double width, double height, int numBoids, int numPredators)
        {
            Resize(width, height);
            
            AddRandomBoids(numBoids);
            // TODO: add predators
        }

        public void StepForward()
        {
            foreach (var boid in Boids)
                boid.StepForward(Width, Height);
        }

        // returns random normalized vector
        private static Vector2 RandomVector2(Random rand)
        {
            var v = new Vector2((float) rand.NextDouble(), (float) rand.NextDouble());
            return v / v.Length();
        }


        private static string RandomColor(Random rand) =>
            $"#{rand.Next(0xFFFFFF):X6}";

        public void AddRandomBoids(int count = 10)
        {
            var rand = new Random();

            for (var i = 0; i < count; i++)
            {
                var newBoid = new Boid(
                    new Vector2((float) (rand.NextDouble() * Width),
                        (float) (rand.NextDouble() * Height)),
                    RandomVector2(rand),
                    RandomColor(rand)
                );
                Boids.Add(newBoid);
            }
        }
    }
}