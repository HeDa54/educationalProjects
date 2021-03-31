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
                V += CalcAcceleration(M, engine.I) * step;
                x = V;
                double Vh = CalculateVh(M, engine.Hm, V, engine.Hv);
                Vh += Vh;
                double Vc = CalculateVc(ambientTemperature, engineTemperature, engine.C);
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
     

        //вычисление ускорения скорости вращения
        private Func<double, double, double> CalcAcceleration = (M, I) => M / I; 

        //вычисление скорости охлаждения двигателя
        private Func<double, double, double, double> CalculateVc = (aT, eT, C) => C * (aT - eT);

        //вычисление скорости нагрева двигателя
        private Func<double, double, double, double, double> CalculateVh = (M, Hm, V, Hv) => M * Hm + Math.Pow(V, 2) * Hv;

        

    }
}
