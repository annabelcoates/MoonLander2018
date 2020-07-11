using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class lander
    {
        private readonly MainWindow window;
        public double Vy = 0;//Initially the lander is stationary
        public double Vx = 5;//ms^-1

        public double yMax = 1000;
        public double y;//Initially the lander is 1000m high
        public double x;

        public double t=0;
        public double dt = 0.1;//s
        public double g = 9.81;//ms^-2
        private double a;
        public double thrustAcceleration=0;
        public lander(MainWindow window)
        {
            this.window = window;
            this.y = yMax;
            this.x = 0;//This is the centre of the window
        }
        public void UpdateLander()
        {
            thrustAcceleration = window.thrustSlider.Value;
            a = thrustAcceleration - g;
            if (Vy <= 0)
            {
                Vy = Vy + a * dt;

            }
            else
            {
                Vy = 0;
            }
            y = y + Vy * dt;
            t = t + dt;
        }
        public void Clear()
        {
            t = 0;
            Vy = 0;
            y = yMax;
            //thrustAcceleration = 0;
        }
        public bool HasLanded()
        {
            if (y <= 200)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MoveHorizontally(string direction)
        {
            if (direction == "right" && x < 80)
            {
                x = x + 10;
            }
            else if (direction == "left"&&x>-80)
            {
                x = x - 10;
            }
        }

    }
}
