using System;
using System.Collections.Generic;
public class NinjaUtil
{
	public static void onLoadMapComplete()
	{
		GameCanvas.endDlg();
	}

	public void onLoading()
	{
		GameCanvas.startWaitDlg(mResources.downloading_data);
	}

	public static int randomNumber(int max)
	{
		MyRandom myRandom = new MyRandom();
		return myRandom.nextInt(max);
	}

	public static string Base64Encode(string plainText)
	{
		var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
		return System.Convert.ToBase64String(plainTextBytes);
	}

	public static string Base64Decode(string base64EncodedData)
	{
		var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
		return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
	}

	public static sbyte[] readByteArray(Message msg)
	{
		try
		{
			int length = msg.reader().readInt();
			if (length > 1)
			{
				sbyte[] data = new sbyte[length];
				msg.reader().read(ref data);
				return data;
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	public static sbyte[] readByteArray(myReader dos)
	{
		try
		{
			int num = dos.readInt();
			sbyte[] data = new sbyte[num];
			dos.read(ref data);
			return data;
		}
		catch (Exception)
		{
			Cout.LogError("LOI DOC readByteArray dos  NINJAUTIL");
		}
		return null;
	}

	public static string Replace(string text, string regex, string replacement)
	{
		return text.Replace(regex, replacement);
	}

	public static string NumberTostring(string number)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (number.Equals(string.Empty))
		{
			return text;
		}
		if (number[0] == '-')
		{
			text2 = "-";
			number = number[1..];
		}
		for (int num = number.Length - 1; num >= 0; num--)
		{
			text = ((number.Length - 1 - num) % 3 != 0 || number.Length - 1 - num <= 0) ? (number[num] + text) : (number[num] + "." + text);
		}
		return text2 + text;
	}

	public static string getDate(int second)
	{
		long num = (long)second * 1000L;
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(new TimeSpan(num * 10000)).ToUniversalTime();
		int hour = dateTime.Hour;
		int minute = dateTime.Minute;
		int day = dateTime.Day;
		int month = dateTime.Month;
		int year = dateTime.Year;
		return day + "/" + month + "/" + year + " " + hour + "h";
	}

	public static string getDate2(long second)
	{
		long num = second + 25200000;
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Add(new TimeSpan(num * 10000)).ToUniversalTime();
		int hour = dateTime.Hour;
		int minute = dateTime.Minute;
		return hour + "h" + minute + "m";
	}

	public static string getTime(int timeRemainS)
	{
		int num = 0;
		if (timeRemainS > 60)
		{
			num = timeRemainS / 60;
			timeRemainS %= 60;
		}
		int num2 = 0;
		if (num > 60)
		{
			num2 = num / 60;
			num %= 60;
		}
		int num3 = 0;
		if (num2 > 24)
		{
			num3 = num2 / 24;
			num2 %= 24;
		}
		string empty = string.Empty;
		if (num3 > 0)
		{
			empty += num3;
			empty += "d";
			return empty + num2 + "h";
		}
		if (num2 > 0)
		{
			empty += num2;
			empty += "h";
			return empty + num + "'";
		}
		empty = ((num <= 9) ? (empty + "0" + num) : (empty + num));
		empty += ":";
		if (timeRemainS > 9)
		{
			return empty + timeRemainS;
		}
		return empty + "0" + timeRemainS;
	}
	public static string FormatNumber(double number)
	{
		if (number == double.PositiveInfinity) return "∞";
		if (number == double.NegativeInfinity) return "-∞";
		if (double.IsNaN(number)) return "NaN";

		// Danh sách đơn vị cơ bản
		string[] baseUnits = { "", " nghìn", " triệu", " tỉ" };
		List<string> units = new List<string>(baseUnits);

		// Tạo các đơn vị tiếp theo theo mẫu: " tỉ", " tỉ tỉ", " tỉ tỉ tỉ"...
		while (units.Count < 100) // Giới hạn số đơn vị để tránh vòng lặp vô hạn
		{
			string lastUnit = units[^1]; // Đơn vị cuối cùng trong danh sách
			units.Add(" tỉ" + lastUnit); // Thêm " tỉ" vào đơn vị cuối
		}

		int unitIndex = 0;

		// Kiểm tra nếu số lớn hơn giá trị tối đa của kiểu double
		if (number >= double.MaxValue)
		{
			return $"{double.MaxValue:N0}{units[unitIndex]}";
		}

		// Chia số theo các bậc 1000
		while (number >= 1000 && unitIndex < units.Count - 1)
		{
			number /= 1000;
			unitIndex++;
		}

		// Làm tròn số và ghép với đơn vị tương ứng
		number = System.Math.Round(number); // Làm tròn để loại bỏ phần thập phân

		// Trả về số đã format với đơn vị tương ứng
		return $"{number:N0}{units[unitIndex]}";
	}

	public static string getMoneys(long m)
	{
		string text = string.Empty;
		long num = m / 1000 + 1;
		for (int i = 0; i < num; i++)
		{
			if (m >= 1000)
			{
				long num2 = m % 1000;
				text = ((num2 != 0) ? ((num2 >= 10) ? ((num2 >= 100) ? ("." + num2 + text) : (".0" + num2 + text)) : (".00" + num2 + text)) : (".000" + text));
				m /= 1000;
				continue;
			}
			text = m + text;
			break;
		}
		return text;
	}
	 

	public static string getTimeAgo(long timeRemainS)
	{
		long num = 0;
		if (timeRemainS > 60)
		{
			num = timeRemainS / 60;
		}
		long num2 = 0;
		if (num > 60)
		{
			num2 = num / 60;
			num %= 60;
		}
		long num3 = 0;
		if (num2 > 24)
		{
			num3 = num2 / 24;
			num2 %= 24;
		}
		string empty = string.Empty;
		if (num3 > 0)
		{
			empty += num3;
			empty += "d";
			return empty + num2 + "h";
		}
		if (num2 > 0)
		{
			empty += num2;
			empty += "h";
			return empty + num + "'";
		}
		if (num == 0)
		{
			num = 1;
		}
		empty += num;
		return empty + "ph";
	}

	public static string[] split(string original, string separator)
	{
		MyVector myVector = new MyVector();
		for (int num = original.IndexOf(separator); num >= 0; num = original.IndexOf(separator))
		{
			myVector.addElement(original.Substring(0, num));
			original = original.Substring(num + separator.Length);
		}
		myVector.addElement(original);
		string[] array = new string[myVector.size()];
		if (myVector.size() > 0)
		{
			for (int i = 0; i < myVector.size(); i++)
			{
				array[i] = (string)myVector.elementAt(i);
			}
		}
		return array;
	}

	public static bool checkNumber(string numberStr)
	{
		try
		{
			int.Parse(numberStr);
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}
}
