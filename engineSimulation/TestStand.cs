using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskForForward
{
    class TestStand
    {
        private double M;
        private double V;
        private double time;
        private Engine engine;

        public TestStand(Engine engine, double ambientTemperature)
        {
            this.engine = engine;      
            TimeToOverheat = CalcTimeToOverheat(ambientTemperature);
        }

        public double TimeToOverheat
        {
            get { return time; }
            private set { time = value; } 
        }

        private double CalcTimeToOverheat(double ambientTemperature)
        { 
            var engineTemperature = ambientTemperature;
            
            double x = 0;
            double time = 0;
            const double step = 0.001;

            while (engineTemperature < engine.overheatTemperature) 
                {           
                M = CalkM(x);
                V += CalcA() * step;
                x = V;
                double Vh = CalcVh();
                Vh += Vh;
                double Vc = CalcVc(ambientTemperature, engineTemperature);
                double Vsum = Vh + Vc;
                engineTemperature += Vsum * step;
                time += step;

                if (M < engine.errorStable) // проверка на слишком низкую температуру, при которой перегрев труднодостижим
                    return -1;
            }             
            return Math.Round(time, 3);              
        }

        private double CalkM(double x)
        {
            double y = 0;
            int start = 0;
            int end = 0;

            //for (int i = 0; i < engine.listV.Count - 1; i++)
            for (int i = 0; i < engine.listV.Length - 1; i++)
                {
                if (engine.listV[i] <= x && engine.listV[i + 1] >= x)
                {
                    start = i;
                    end = i + 1;
                }
                double y1 = engine.listM[start];
                double y2 = engine.listM[end];
                double x1 = engine.listV[start];
                double x2 = engine.listV[end];

                double k = (y2 - y1) / (x2 - x1);            //------------------------
                double b = y1 - k * x1;                      // линейная функция, реализация с дополнительными переменными
                y = k * x + b;                               //------------------------

                //y = y1 + (y2 - y1) / (x2 - x1) * (x - x1); // формула линейной интерполяции, второй вариант реализации без ввода дополнительных переменных
            }
            return y;
        }

        private double CalcA() //вычисление ускорения скорости вращения
        {
            return M / engine.I;
        }

        private double CalcVc(double ambientTemperature, double engineTemperature)
        {
            return engine.C * (ambientTemperature - engineTemperature);
        }

        private double CalcVh()
        {
            return M * engine.Hm + Math.Pow(V, 2) * engine.Hv;
        }

    }
}
