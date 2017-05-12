using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Roulette
{

	public partial class MainWindow : Window
	{
		private int _MoneyLeft;
		private int _CurrentBet;
		private RadioButton _CurrentRadioButton;

		private List<int> _Row1, _Row2, _Row3, _Blacks, _Reds;

		public MainWindow()
		{
			InitializeComponent();
			_MoneyLeft = 1000;
			RemainingMoney.Text = _MoneyLeft + "";
			_Row1 = new List<int>();
			_Row2 = new List<int>();
			_Row3 = new List<int>();
			for (int o = 0; o < 12; o++)
			{
				_Row1.Add(o * 3 + 1);
				_Row2.Add(o * 3 + 2);
				_Row3.Add(o * 3 + 2);
			}

			_Reds = new List<int>
			{
				1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36
			};
			_Blacks = new List<int>
			{
				2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35
			};
		}

		private void Spin(object sender, RoutedEventArgs e)
		{
			int multiplier;
			try
			{
				multiplier = GetMult();
				_CurrentBet = int.Parse(Bet.Text);
				if (_CurrentBet > _MoneyLeft)
				{
					DisplayText.AppendText("\n\nYour bet cannot be greater than the money you have left!");
					return;
				}
				else if (_CurrentBet == 0)
				{
					DisplayText.AppendText("\n\nYour bet cannot be 0!");
					return;
				}
			}
			catch (Exception E)
			{
				DisplayText.AppendText("\n\nChoose your space(s), and place your bet.");
				return;
			}

			int r = new Random().Next(38);
			switch (r)
			{
				case 0: // 0
					DisplayText.AppendText("\n\nSpun a 0.");
					if ((string)_CurrentRadioButton.Content == "0")
					{
						_MoneyLeft += _CurrentBet * multiplier;
						DisplayText.AppendText("\n\nYou won " + (_CurrentBet * multiplier) + ".");
					}
					else
					{
						_MoneyLeft -= _CurrentBet;
						DisplayText.AppendText("\n\nYou lost " + _CurrentBet + ".");
					}
					break;

				case 37: // 00
					DisplayText.AppendText("\n\nSpun a 00.");
					if ((string)_CurrentRadioButton.Content == "00")
					{
						_MoneyLeft += _CurrentBet * multiplier;
						DisplayText.AppendText("\n\nYou won " + (_CurrentBet * multiplier) + ".");
					}
					else
					{
						_MoneyLeft -= _CurrentBet;
						DisplayText.AppendText("\n\nYou lost " + _CurrentBet + ".");
					}
					break;

				default:
					DisplayText.AppendText("\n\nSpun a " + r + ".");
					bool win = false;
					switch ((string)_CurrentRadioButton.Content)
					{
						case "Row 1":
							win = _Row1.Contains(r);
							break;

						case "Row 2":
							win = _Row2.Contains(r);
							break;

						case "Row 3":
							win = _Row3.Contains(r);
							break;

						case "Evens":
							win = r % 2 == 0;
							break;

						case "Odds":
							win = r % 2 == 1;
							break;

						case "Blacks":
							win = _Blacks.Contains(r);
							break;

						case "Reds":
							win = _Reds.Contains(r);
							break;

						case "1st 12":
							win = r < 13 && r > 0;
							break;

						case "2nd 12":
							win = r < 25 && r > 12;
							break;

						case "3rd 12":
							win = r < 37 && r > 24;
							break;

						case "1-18":
							win = r > 0 && r < 19;
							break;

						case "19-36":
							win = r > 18 && r < 37;
							break;

						default:
							win = (string)_CurrentRadioButton.Content == ("" + r);
							break;
					}

					if (win)
					{
						_MoneyLeft += _CurrentBet * multiplier;
						DisplayText.AppendText("\n\nYou won " + (_CurrentBet * multiplier) + ".");
					}
					else
					{
						_MoneyLeft -= _CurrentBet;
						DisplayText.AppendText("\n\nYou lost " + _CurrentBet + ".");
					}
					break;
			}
			RemainingMoney.Text = _MoneyLeft + "";
			if (DisplayText.LineCount > 50)
			{

			}
		}

		private int GetMult()
		{
			switch (_CurrentRadioButton.Content)
			{
				case "Row 1":
				case "Row 2":
				case "Row 3":
				case "1st 12":
				case "2nd 12":
				case "3rd 12":
					return 2;

				case "Evens":
				case "Odds":
				case "Reds":
				case "Blacks":
				case "1-18":
				case "18-36":
					return 1;

				default:
					return 35;
			}
		}

		private void AllCheckBoxes_CheckedChanged(object sender, EventArgs e)
		{
			if ((bool)((RadioButton)sender).IsChecked)
			{
				_CurrentRadioButton = (RadioButton)sender;
			}
			// DisplayText.Text += "\n\n" + _CurrentRadioButton.Content;
		}

		private void DisplayText_TextChanged(object sender, TextChangedEventArgs e)
		{
			DisplayText.ScrollToEnd();
		}

		private void Bet_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (Bet.Text != "" && !char.IsNumber(Bet.Text[Bet.Text.Length - 1]))
			{
				Bet.Text = Bet.Text.Substring(0, Bet.Text.Length - 1);
			}
			Bet.CaretIndex = Bet.Text.Length;
		}
	}
}