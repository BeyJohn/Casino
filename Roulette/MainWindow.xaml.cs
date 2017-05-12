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
			try
			{
				_CurrentBet = int.Parse(Bet.Text);
				if (_CurrentBet > _MoneyLeft)
				{
					DisplayText.Text += "\nYour bet cannot be greater than the money you have left!";
					return;
				}
				else if (_CurrentBet == 0)
				{
					DisplayText.Text += "\nYour bet cannot be 0!";
					return;
				}
			}
			catch (Exception E)
			{
				DisplayText.Text += "\nChoose your space(s), and place your bet.";
				return;
			}
			int multiplier = GetMult();

			int r = new Random().Next(38);
			switch (r)
			{
				case 0: // 0
					DisplayText.Text += "\nSpun a 0.";
					if ((string)_CurrentRadioButton.Content == "0")
					{
						_MoneyLeft += _CurrentBet * multiplier;
					}
					else
					{
						_MoneyLeft -= _CurrentBet;
					}
					break;

				case 37: // 00
					DisplayText.Text += "\nSpun a 00.";
					if ((string)_CurrentRadioButton.Content == "00")
					{
						_MoneyLeft += _CurrentBet * multiplier;
					}
					else
					{
						_MoneyLeft -= _CurrentBet;
					}
					break;

				default:
					DisplayText.Text += "\nSpun a " + r + ".";
					if ((string)_CurrentRadioButton.Content == ("" + r) || ((string)_CurrentRadioButton.Content == "Row 1" && _Row1.Contains(r)) || ((string)_CurrentRadioButton.Content == "Row 2" && _Row2.Contains(r)) || ((string)_CurrentRadioButton.Content == "Row 3" && _Row3.Contains(r)) || ((string)_CurrentRadioButton.Content == "Evens" && r % 2 == 0) || ((string)_CurrentRadioButton.Content == "Odds" && r % 2 == 1) || ((string)_CurrentRadioButton.Content == "Blacks" && _Blacks.Contains(r)) || ((string)_CurrentRadioButton.Content == "Reds" && _Reds.Contains(r)) || ((string)_CurrentRadioButton.Content == "1st 12" && r < 13 && r > 0) || ((string)_CurrentRadioButton.Content == "2nd 12" && r < 25 && r > 12) || ((string)_CurrentRadioButton.Content == "3rd 12" && r < 37 && r > 24) || ((string)_CurrentRadioButton.Content == "1-18" && r > 0 && r < 19) || ((string)_CurrentRadioButton.Content == "19-36" && r > 18 && r < 37))
					{
						_MoneyLeft += _CurrentBet * multiplier;
						DisplayText.Text += "\nYou won " + (_CurrentBet * multiplier) + ".";
					}
					else
					{
						_MoneyLeft -= _CurrentBet;
						DisplayText.Text += "\nYou lost " + _CurrentBet + ".";
					}
					break;
			}
			RemainingMoney.Text = _MoneyLeft + "";
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
					return 1;

				case "Evens":
				case "Odds":
				case "Reds":
				case "Blacks":
				case "1-18":
				case "18-36":
					return 2;

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
			// DisplayText.Text += "\n" + _CurrentRadioButton.Content;
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