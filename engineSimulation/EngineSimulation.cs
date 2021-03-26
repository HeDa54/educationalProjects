using System;
using System.Collections.Generic;

namespace TestTaskForForward
{
    internal class EngineSimulation
    {
        public static void Main(string[] args)
        {
            double ambientTemperature;
            double time;

            Console.Write("Введите температуру окружающей среды: ");
            while (!double.TryParse(Console.ReadLine(), out ambientTemperature))    // проверка на корректный ввод данных
            {
                Console.WriteLine("Ввод данных должен быть в формате: X,XX...(or X.XX...)");
            }
            Console.WriteLine();

            var testStand = new TestStand(ambientTemperature);
            

            time = testStand.TimeToOverheat;
            if (time != 0)
            {
                Console.WriteLine("Время работы двигателя до перегрева: " + time + " секунд");
            }
            else
            {
                Console.WriteLine("Ошибка! Задана слишком низкая/высокая температура окружающей среды");
            }
            Console.ReadLine(); // чтобы не закрывалось окно консоли после выполнения программы
        }
        
    }    
}

 