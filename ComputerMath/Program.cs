using System;

namespace ComputerMath
{
	public static class Solver
	{
		/// <summary>
		/// Метод половинного деления
		/// </summary>
		/// <param name="left">Левая граница</param>
		/// <param name="right">Правая граница</param>
		/// <param name="f">Левая часть уравнения</param>
		/// <returns>Корень уравнения</returns>
		public static (double x, int iter) Bisection(double left, double right, Func<double, double> f)
		{
			const double epsilon = 1E-3;
			var result = left;
			var iter = 0;
 
			while (right - left >= epsilon)
			{
				result = (left + right) / 2;
				
				if (f(left) * f(result) <= 0) 
					right = result;
				else 
					left = result;
				
				iter++;
			}

			return (result, iter);
		}
		
		/// <summary>
		/// Метод простой итерации
		/// </summary>
		/// <param name="xStart">Начальное приближение</param>
		/// <param name="f">Функция фи, полученная выражением x из левой части уравнения</param>
		/// <returns>Корень уравнения</returns>
		public static (double x, int iter) SimpleIterations(double xStart, Func<double, double> f)
		{
			const double epsilon = 1E-3;
			double x;
			var xPrev = xStart;
			var iter = 0;
			
			while (true)
			{
				x = f(xPrev);
				if (Math.Abs(x - xPrev) <= epsilon) break;
				xPrev = x;
				iter++;
			}

			return (x, iter);
		}
		
		/// <summary>
		/// Метод Ньютона
		/// </summary>
		/// <param name="x">Начальное приближение</param>
		/// <param name="f">Левая часть уравнения</param>
		/// <param name="df">Производная левой части уравнения</param>
		/// <returns>Корень уравнения</returns>
		public static (double x, int iter) Newton(double x, Func<double, double> f, Func<double, double> df)
		{
			const double epsilon = 1E-3;
			var iter = 0;
			double y;
			
			do
			{
				var x1 = x - f(x) / df(x);
				x = x1;
				y = f(x);
				iter++;
			}
			while (Math.Abs(y) >= epsilon);
			
			return (x, iter);
		}

		/// <summary>
		/// Нужно задать функции и их частные производные
		/// </summary>
		public static class NewtonSystem
		{
			private const double Epsilon = 1E-4;
			
			public static double Iterations { get; private set; }
	        public static double X { get; private set; }
	        public static double Y { get; private set; }

	        private static double Df1Dx(double x, double y)
	        {
	            return y * (1 / Math.Cos(x * y + 0.1) - 2 * x);
	        }

	        private static double Df1Dy(double x, double y)
	        {
	            return x * Math.Pow(1 / Math.Cos(x * y + 0.1), 2);
	        }

	        private static double Df2Dx(double x)
	        {
	            return x;
	        }

	        private static double Df2Dy(double y)
	        {
	            return 4 * y;
	        }

	        private static double F1(double x, double y)
	        {
		        return Math.Tan(x * y + 0.1) - x * x;
	        }

	        private static double F2(double x, double y)
	        {
		        return 0.5 * x * x + 2 * y * y - 1;
	        }

	        public static void Solve()
	        {
	            double[] x = { 0.8, 0.8 };
	            double prevX;
	            
	            do
	            {
	                prevX = x[0];
	                var prevY = x[1];
	                x[0] = prevX - 1 /
		                (Df1Dx(prevX, prevY) * Df2Dy(prevY) -
		                 Df1Dy(prevX, prevY) * Df2Dx(prevX)) *
		                (Df2Dy(prevY) * F1(prevX, prevY) -
		                 Df1Dy(prevX, prevY) * F2(prevX, prevY));
	                x[1] = prevY - 1 /
		                (Df1Dx(prevX, prevY) * Df2Dy(prevY) -
		                 Df1Dy(prevX, prevY) * Df2Dx(prevX)) *
		                (-Df2Dx(prevX) * F1(prevX, prevY) +
		                 Df1Dx(prevX, prevY) * F2(prevX, prevY));
	                prevX = Math.Max(Math.Abs(prevX - x[0]), Math.Abs(prevY - x[1]));
	                Iterations++;
	            } while (prevX > Epsilon);

	            X = x[0];
	            Y = x[1];
	        }
	    }
		
		/// <summary>
		/// Нужно выразить из первого уравнения x, а из второго y и написать фи-функции
		/// </summary>
		public static class SimpleIterationsSystem
		{
			private const double Epsilon = 1E-4;
			
			public static double X { get; private set; }
			public static double Y { get; private set; }
			public static double Iterations { get; private set; }

			private static double F1(double x, double y)
			{
				return Math.Sqrt(Math.Tan(x * y + 0.1));
			}

			private static double F2(double x)
			{
				return 0.5 * Math.Sqrt(2 - x * x);
			}

			public static void Solve()
			{
				double[] x = { 0.8, 0.8 };
				double yPrev;

				do
				{
					var xPrev = x[0];
					yPrev = x[1];
					x[0] = F1(x[0], x[1]);
					x[1] = F2(x[0]);
					yPrev = Math.Max(Math.Abs(yPrev - x[1]), Math.Abs(xPrev - x[0]));
					Iterations++;
				} while (yPrev > Epsilon);

				X = x[0];
				Y = x[1];
			}
		}
	}
	
	public static class Program
	{
		private static void Task1()
		{
			Console.WriteLine("Задача 1.");
			var (x1, iter1) = 
				Solver.Bisection(0.1, 6.2, d => 2 * Math.Log10(d) - d / 2 + 1);
			Console.WriteLine("Метод половинного деления");
			Console.WriteLine($"Корень: {x1}");
			Console.WriteLine($"Итерации: {iter1}");
		}
		
		private static void Task2()
		{
			Console.WriteLine("Задача 2.");
			var (x2, iter2) = 
				Solver.SimpleIterations(8.75, d => (Math.Cos(d) + 2) / 3);
			Console.WriteLine("Метод простой итерации:");
			Console.WriteLine($"Корень: {x2}");
			Console.WriteLine($"Итерации: {iter2}");
			var (x3, iter3) = 
				Solver.Newton(8.75, d => 3 * d - Math.Cos(d) - 2, d => 3 + Math.Sin(d));
			Console.WriteLine("Метод Ньютона:");
			Console.WriteLine($"Корень: {x3}");
			Console.WriteLine($"Итерации: {iter3}");
		}
		
		private static void Task4()
		{
			Console.WriteLine("Задача 4.");
			Console.WriteLine("Метод Ньютона для системы.");
			Solver.NewtonSystem.Solve();
			Console.WriteLine("Корень(1):" + Solver.NewtonSystem.X);
			Console.WriteLine("Корень(2):" + Solver.NewtonSystem.Y);
			Console.WriteLine("Итерации:" + Solver.NewtonSystem.Iterations);
			Console.WriteLine("Метод простой итерации для системы.");
			Solver.SimpleIterationsSystem.Solve();
			Console.WriteLine("Корень(1):" + Solver.SimpleIterationsSystem.X);
			Console.WriteLine("Корень(2):" + Solver.SimpleIterationsSystem.Y);
			Console.WriteLine("Итерации:" + Solver.SimpleIterationsSystem.Iterations);
		}
		
		public static void Main()
		{
			Task1();
			Task2();
			Task4();
		}
	}
}