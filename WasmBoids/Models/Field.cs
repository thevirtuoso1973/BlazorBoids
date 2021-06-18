﻿using System;
using System.Collections.Generic;

namespace WasmBoids.Models
{
    public class Field
    {
        public readonly List<Ball> Balls = new List<Ball>();
        public double Width { get; private set; }
        public double Height { get; private set; }

        public void Resize(double width, double height) =>
            (Width, Height) = (width, height);

        public void StepForward()
        {
            foreach (Ball ball in Balls)
                ball.StepForward(Width, Height);
        }

        private double RandomVelocity(Random rand, double min, double max)
        {
            double v = min + (max - min) * rand.NextDouble();
            if (rand.NextDouble() > .5)
                v *= -1;
            return v;
        }


        private string RandomColor(Random rand) => 
            string.Format("#{0:X6}", rand.Next(0xFFFFFF));

        public void AddRandomBalls(int count = 10)
        {
            double minSpeed = .5;
            double maxSpeed = 5;
            double radius = 10;
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                Balls.Add(
                    new Ball(
                        x: Width * rand.NextDouble(),
                        y: Height * rand.NextDouble(),
                        xVel: RandomVelocity(rand, minSpeed, maxSpeed),
                        yVel: RandomVelocity(rand, minSpeed, maxSpeed),
                        radius: radius,
                        color: RandomColor(rand)
                    )
                );
            }
        }
    }
}