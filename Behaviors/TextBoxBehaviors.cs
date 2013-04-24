using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Anythink.Wpf.Utilities.Behaviors
{
	/// <summary>
	/// A collection of Behaviors for TextBoxes
	/// </summary>
	public static class TextBoxBehaviors
	{
		// Filters text input for a TextBox
		#region InputFilter Behavior

		private static Dictionary<InputFilterTypes, Func<char, TextBox, bool>> _filters;

		/// <summary>
		/// Constructor.  Setup the filters for this behavior
		/// </summary>
		static TextBoxBehaviors()
		{
			//Setup the Filters for this behavior
			_filters = new Dictionary<InputFilterTypes, Func<char, TextBox, bool>>();

			_filters.Add(InputFilterTypes.Digits, (c, tb) => { return !char.IsDigit(c); });
			_filters.Add(InputFilterTypes.Letters, (c, tb) => { return !char.IsLetter(c); });
			_filters.Add(InputFilterTypes.Punctuation, (c, tb) => { return !char.IsPunctuation(c); });
			_filters.Add(InputFilterTypes.Symbol, (c, tb) => { return !char.IsSymbol(c); });

			_filters.Add(InputFilterTypes.Decimal, (c, tb) => { return (!char.IsDigit(c) && !IsOneOf(c, '.')) || AlreadyHasOne(tb, c, '.'); });
			_filters.Add(InputFilterTypes.Currency, (c, tb) => { return (!char.IsDigit(c) && !IsOneOf(c, '.', '$')) || AlreadyHasOne(tb, c, '$', '.'); });
			_filters.Add(InputFilterTypes.PhoneNumber, (c, tb) => { return (!char.IsDigit(c) && !IsOneOf(c, '-', 'x', '(', ')')) || AlreadyHasOne(tb, c, '(', ')', 'x'); });
			_filters.Add(InputFilterTypes.ZipCode, (c, tb) => { return (!char.IsDigit(c) && !IsOneOf(c, '-')) || AlreadyHasOne(tb, c, '-'); });
		}

		/// <summary>
		/// An Attached Behavior that will filter the values that can be typed into a texbox based on the InputFilterType.
		/// </summary>
		public static readonly DependencyProperty InputFilterProperty = DependencyProperty.RegisterAttached("InputFilter", typeof(InputFilterTypes), typeof(TextBoxBehaviors), new UIPropertyMetadata(OnInputFilterChanged));

		/// <summary>
		/// Gets the InputFilter attached property value from a TextBox
		/// </summary>
		/// <param name="textbox"></param>
		/// <returns></returns>
		public static InputFilterTypes GetInputFilter(DependencyObject textbox)
		{
			return (InputFilterTypes)textbox.GetValue(InputFilterProperty);
		}

		/// <summary>
		/// Sets the InputFilter attached property value on a TextBox
		/// </summary>
		/// <param name="textbox"></param>
		/// <param name="filterType"></param>
		public static void SetInputFilter(DependencyObject textbox, InputFilterTypes filterType)
		{
			textbox.SetValue(InputFilterProperty, filterType);
		}

		/// <summary>
		/// Handles the addition/removal of the Attached Property
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnInputFilterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			TextBox txt = d as TextBox;
			if (txt != null)
			{
				if (e.NewValue == null)
				{
					txt.PreviewTextInput -= PreviewTextInputForFilter;
				}
				else
				{
					txt.PreviewTextInput += PreviewTextInputForFilter;
				}
			}
		}

		/// <summary>
		/// Handle the TextInput to determine if this character is acceptable
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void PreviewTextInputForFilter(object sender, TextCompositionEventArgs e)
		{
			TextBox txt = sender as TextBox;
			InputFilterTypes filterType = GetInputFilter(txt);
			char LetterOrDigit = Convert.ToChar(e.Text);

			if (_filters.ContainsKey(filterType))
				e.Handled = _filters[filterType](LetterOrDigit, txt);
		}

		#region Helper Methods

		private static bool IsOneOf(char candidate, params char[] values)
		{
			return values.Contains(candidate);
		}

		private static bool AlreadyHasOne(TextBox txt, char candidate, params char[] values)
		{
			return IsOneOf(candidate, values) && (txt.Text.IndexOf(candidate) >= 0 && txt.SelectedText.IndexOf(candidate) < 0);		//Do we already have one?  If so, are we over-writing it?
		}

		#endregion

		/// <summary>
		/// Types of InputFilters
		/// </summary>
		public enum InputFilterTypes
		{
			None,
			Digits,
			Letters,
			Punctuation,
			Symbol,
			Decimal,
			Currency,
			PhoneNumber,
			ZipCode
		}

		#endregion

		//Attempts to mask text in a textbox (not very robust right now).
		#region InputMask Behavior

		//private static readonly DependencyProperty InputMaskProperty = DependencyProperty.RegisterAttached("InputMask", typeof(string), typeof(TextBoxBehaviors), new UIPropertyMetadata(OnInputMaskChanged));
		//private static readonly DependencyProperty InputMaskPromptCharacter = DependencyProperty.RegisterAttached("InputMaskPromptCharacter", typeof(char), typeof(TextBoxBehaviors), new PropertyMetadata('_'));
		//private static readonly DependencyProperty InputMaskProvider = DependencyProperty.RegisterAttached("InputMaskProvider", typeof(MaskedTextProvider), typeof(TextBoxBehaviors), null);

		//public static string GetInputMask(DependencyObject textbox)
		//{
		//	return (string)textbox.GetValue(InputMaskProperty);
		//}
		//public static char GetInputMaskPromptCharacter(DependencyObject textbox)
		//{
		//	return (char)textbox.GetValue(InputMaskPromptCharacter);
		//}
		//public static MaskedTextProvider GetInputMaskProvider(DependencyObject textbox)
		//{
		//	return (MaskedTextProvider)textbox.GetValue(InputMaskProvider);
		//}

		//public static void SetInputMask(DependencyObject textbox, string mask)
		//{
		//	textbox.SetValue(InputMaskProperty, mask);
		//}
		//public static void SetInputMaskPromptCharacter(DependencyObject textbox, char promptCharacter)
		//{
		//	textbox.SetValue(InputMaskPromptCharacter, promptCharacter);
		//}
		//public static void SetInputMaskProvider(DependencyObject textbox, MaskedTextProvider maskProvider)
		//{
		//	textbox.SetValue(InputMaskProvider, maskProvider);
		//}

		//private static void OnInputMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		//{
		//	TextBox txt = d as TextBox;
		//	if (txt != null)
		//	{
		//		if (e.NewValue != null)
		//		{
		//			txt.Loaded += TextBoxLoadedForMask;
		//			txt.PreviewTextInput += TextBoxPreviewTextInputForMask;
		//			txt.PreviewKeyDown += TextBoxPreviewKeyDownForMask;

		//			DataObject.AddPastingHandler(txt, PastingForMask);
		//		}
		//		else
		//		{
		//			txt.Loaded -= TextBoxLoadedForMask;
		//			txt.PreviewTextInput -= TextBoxPreviewTextInputForMask;
		//			txt.PreviewKeyDown -= TextBoxPreviewKeyDownForMask;

		//			DataObject.RemovePastingHandler(txt, PastingForMask);
		//			var provider = GetInputMaskProvider(txt);
		//			if (provider != null)
		//			{
		//				provider = null;
		//				SetInputMaskProvider(txt, null);
		//			}
		//		}
		//	}
		//}

		///*
		//Mask Character  Accepts  Required?  
		//0  Digit (0-9)  Required  
		//9  Digit (0-9) or space  Optional  
		//#  Digit (0-9) or space  Required  
		//L  Letter (a-z, A-Z)  Required  
		//?  Letter (a-z, A-Z)  Optional  
		//&  Any character  Required  
		//C  Any character  Optional  
		//A  Alphanumeric (0-9, a-z, A-Z)  Required  
		//a  Alphanumeric (0-9, a-z, A-Z)  Optional  
		//   Space separator  Required 
		//.  Decimal separator  Required  
		//,  Group (thousands) separator  Required  
		//:  Time separator  Required  
		///  Date separator  Required  
		//$  Currency symbol  Required  

		//In addition, the following characters have special meaning:

		//Mask Character  Meaning  
		//<  All subsequent characters are converted to lower case  
		//>  All subsequent characters are converted to upper case  
		//|  Terminates a previous < or >  
		//\  Escape: treat the next character in the mask as literal text rather than a mask symbol  
		//*/

		//private static void TextBoxLoadedForMask(object sender, System.Windows.RoutedEventArgs e)
		//{
		//	TextBox txt = sender as TextBox;
		//	if (txt != null)
		//	{
		//		string inputMask = GetInputMask(txt);
		//		char prompt = GetInputMaskPromptCharacter(txt);

		//		var provider = new MaskedTextProvider(inputMask, CultureInfo.CurrentCulture);
		//		SetInputMaskProvider(txt, provider);

		//		provider.Set(txt.Text);
		//		provider.PromptChar = prompt;
		//		txt.Text = provider.ToDisplayString();

		//		//seems the only way that the text is formatted correct, when source is updated
		//		var textProp = DependencyPropertyDescriptor.FromProperty(TextBox.TextProperty, typeof(TextBox));
		//		if (textProp != null)
		//		{
		//			textProp.AddValueChanged(txt, (s, args) => UpdateText(txt, provider));
		//		}
		//	}
		//}

		//private static void TextBoxPreviewTextInputForMask(object sender, TextCompositionEventArgs e)
		//{
		//	TextBox txt = sender as TextBox;
		//	if (txt != null)
		//	{
		//		MaskedTextProvider provider = GetInputMaskProvider(txt);

		//		TreatSelectedText(txt, provider);

		//		var position = GetNextCharacterPosition(txt.SelectionStart, provider);

		//		if (Keyboard.IsKeyToggled(Key.Insert))
		//		{
		//			if (provider.Replace(e.Text, position))
		//				position++;
		//		}
		//		else
		//		{
		//			if (provider.InsertAt(e.Text, position))
		//				position++;
		//		}

		//		position = GetNextCharacterPosition(position, provider);

		//		RefreshText(position, txt, provider);

		//		e.Handled = true;
		//	}
		//}

		//private static void TextBoxPreviewKeyDownForMask(object sender, KeyEventArgs e)
		//{
		//	TextBox txt = sender as TextBox;
		//	if (txt != null)
		//	{
		//		MaskedTextProvider provider = GetInputMaskProvider(txt);

		//		if (e.Key == Key.Space)//handle the space
		//		{
		//			TreatSelectedText(txt, provider);

		//			var position = GetNextCharacterPosition(txt.SelectionStart, provider);

		//			if (provider.InsertAt(" ", position))
		//				RefreshText(position, txt, provider);

		//			e.Handled = true;
		//		}

		//		if (e.Key == Key.Back)//handle the back space
		//		{
		//			if (TreatSelectedText(txt, provider))
		//			{
		//				RefreshText(txt.SelectionStart, txt, provider);
		//			}
		//			else
		//			{
		//				if (txt.SelectionStart != 0)
		//				{
		//					if (provider.RemoveAt(txt.SelectionStart - 1))
		//						RefreshText(txt.SelectionStart - 1, txt, provider);
		//				}
		//			}

		//			e.Handled = true;
		//		}

		//		if (e.Key == Key.Delete)//handle the delete key
		//		{
		//			//treat selected text
		//			if (TreatSelectedText(txt, provider))
		//			{
		//				RefreshText(txt.SelectionStart, txt, provider);
		//			}
		//			else
		//			{

		//				if (provider.RemoveAt(txt.SelectionStart))
		//					RefreshText(txt.SelectionStart, txt, provider);

		//			}

		//			e.Handled = true;
		//		}
		//	}

		//}

		//private static void PastingForMask(object sender, DataObjectPastingEventArgs e)
		//{
		//	if (e.DataObject.GetDataPresent(typeof(string)))
		//	{
		//		TextBox txt = sender as TextBox;
		//		if (txt != null)
		//		{
		//			MaskedTextProvider provider = GetInputMaskProvider(txt);
		//			var pastedText = (string)e.DataObject.GetData(typeof(string));

		//			TreatSelectedText(txt, provider);

		//			var position = GetNextCharacterPosition(txt.SelectionStart, provider);

		//			if (provider.InsertAt(pastedText, position))
		//			{
		//				RefreshText(position, txt, provider);
		//			}
		//		}
		//	}

		//	e.CancelCommand();
		//}

		//#region Helper Methods

		//private static void UpdateText(TextBox txt, MaskedTextProvider provider)
		//{
		//	//check Provider.Text + TextBox.Text
		//	if (provider.ToDisplayString().Equals(txt.Text))
		//		return;

		//	//use provider to format
		//	var success = provider.Set(txt.Text);

		//	//ui and mvvm/codebehind should be in sync
		//	SetText(txt, success ? provider.ToDisplayString() : txt.Text);
		//}

		//private static void SetText(TextBox txt, string text)
		//{
		//	txt.Text = String.IsNullOrWhiteSpace(text) ? String.Empty : text;
		//}

		///// <summary>
		///// 
		///// </summary>
		///// <returns></returns>
		//private static bool TreatSelectedText(TextBox txt, MaskedTextProvider provider)
		//{
		//	if (txt.SelectionLength > 0)
		//	{
		//		return provider.RemoveAt(txt.SelectionStart,
		//									  txt.SelectionStart + txt.SelectionLength - 1);
		//	}
		//	return false;
		//}

		//private static int GetNextCharacterPosition(int startPosition, MaskedTextProvider provider)
		//{
		//	var position = provider.FindEditPositionFrom(startPosition, true);

		//	if (position == -1)
		//		return startPosition;
		//	else
		//		return position;
		//}

		//private static void RefreshText(int position, TextBox txt, MaskedTextProvider provider)
		//{
		//	SetText(txt, provider.ToDisplayString());
		//	txt.SelectionStart = position;
		//}

		//public static string UnMask(string text, string prompt, params string[] otherChars)
		//{
		//	text = text.Replace(prompt, "");
		//	foreach (var c in otherChars)
		//	{
		//		text = text.Replace(c, "");
		//	}			

		//	return text.Trim();
		//}
		//#endregion

		#endregion

	}
}
