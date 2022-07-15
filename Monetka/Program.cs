using System;
using System.Threading;
namespace Monetka
{
	internal class Program
	{
		private static void Main()
		{
			Moneta moneta = new Moneta(200);
			Console.WriteLine($"ваши начальные деньги: {moneta.Money}");
			while (moneta.Money >= 50)
			{
				Console.Write("введите ставку: ");
				try { moneta.Throw( int.Parse( Console.ReadLine() ) ); }
				catch (Exception ex) { Console.WriteLine(ex.Message); continue;
				}
			}
			Console.Write("у вас не осталось денег.");
			Console.ReadKey(true);
			Thread.Sleep(1000);
		}
	}
	public class Moneta
	{
		#region fields
		/// <summary>
		/// деньги в объекте.
		/// </summary>
		private int money;
		/// <summary>
		/// ставка на сторону.
		/// </summary>
		private int side; //true - head, false - tail
		/// <summary>
		/// рандом для определения победы.
		/// </summary>
		private Random random;
		/// <summary>
		/// уведомление о победе или проигрыше.
		/// </summary>
		Action<string, string> notifs; //система вывода
		#endregion
		#region properties
		/// <summary>
		/// все деньги.
		/// </summary>
		public int Money 
		{ 
			get { return money; }
			private set { money = value; }
		}
		/// <summary>
		/// ставка на сторону монеты.
		/// </summary>
		public bool Side 
		{ 
			get { return side == 1; }
			set { side = value ? 1 : 0; }
		}
		/// <summary>
		/// метод уведомления о победе/проигрыше в монетке.
		/// </summary>
		public Action<string, string> Notifs { get { return notifs; } set { notifs = value; } }
		#endregion
		#region constructors
		/// <summary>
		/// конструктор объекта игры в монету.
		/// </summary>
		/// <remarks>
		/// использование внутреннего метода вывода информации победы/проигрыша (<see cref="Cout(string, string)"/>).
		/// </remarks>
		/// <param name="money">стартовые деньги.</param>
		/// <param name="side">стартовая ставка на сторону.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public Moneta(int money = 50, bool side = true) : this(null, money, side) { }
		/// <summary>
		/// конструктор объекта игры в монету.
		/// </summary>
		/// <param name="notifs">метод вывода победы/проигрыша.</param>
		/// <param name="money">стартовые деньги.</param>
		/// <param name="side">стартовая ставка на сторону.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public Moneta(Action<string, string> notifs, int money = 50, bool side = true)
		{
			if (money < 50) throw new ArgumentOutOfRangeException(nameof(money), "ставка не может быть меньше 50.");
			if (notifs is null) notifs = Cout;
			this.notifs = notifs;
			Money = money;
			Side = side;
			random = new Random();
		}
		#endregion
		#region methods
		/// <summary>
		/// бросок монетки.
		/// </summary>
		/// <param name="bet">ставка денег на монетку.</param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public void Throw(int bet)
		{
			if (bet < 50 || bet > money) throw new ArgumentOutOfRangeException(nameof(bet), "ставка не может быть меньше 50 или больше всех денег.");
			if (random.Next(2) == side && (random.Next(2) == 1 || random.Next(2) == 0))
			{
				int win = (int)(bet * 2 - (bet * 0.1));
				money += win;
				notifs?.Invoke($"вы выиграли {win} денег.", $"ваши деньги: {money}");
			}
			else
			{
				money -= bet;
				notifs?.Invoke($"вы проиграли свою ставку {bet}.", $"ваши деньги: {money}");
			}
		}
		/// <summary>
		/// выводит информацию в консоль.
		/// </summary>
		/// <remarks>
		/// стандартный способ вывода.
		/// </remarks>
		/// <param name="message">сообщение о победе/проигрыше.</param>
		/// <param name="money">сообщение о настоящем количестве денег.</param>
		public void Cout(string message, string money)
		{
			Console.WriteLine(message);
			Console.WriteLine(money);
		}
		#endregion
	}
	internal static class stavks
	{
		/// <summary>
		/// ставка на бросок монеты.
		/// </summary>
		/// <param name="ставка">деньги ставящиеся в игре.</param>
		/// <param name="side">
		/// сторона на которую ставится ставка. <br/>
		/// <see langword="true"/> = head;<br/>
		/// <see langword="false"/> = tail.
		/// </param>
		/// <returns>деньги выигранные за бросок.</returns>
		public static int Moneta(int ставка, bool side)
		{
			if (ставка < 50) throw new ArgumentOutOfRangeException(nameof(ставка), "ставка не может быть меньше 50.");
			Random random = new Random();
			int iside = side == true ? 1 : 0;
			if (random.Next(2) == iside) return ставка + (int)(ставка - ставка * 0.5);
			return 0;
		}
	}
}