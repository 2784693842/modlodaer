using System;
using UnityEngine;

// Token: 0x020001CB RID: 459
public static class ExtraMath
{
	// Token: 0x06000C67 RID: 3175 RVA: 0x00065C0E File Offset: 0x00063E0E
	public static bool FloatIsEqual(float _A, float _B)
	{
		return Mathf.Abs(_A - _B) <= 1E-05f;
	}

	// Token: 0x06000C68 RID: 3176 RVA: 0x00065C22 File Offset: 0x00063E22
	public static bool FloatIsLowerOrEqual(float _A, float _B)
	{
		return _A <= _B + 1E-05f;
	}

	// Token: 0x06000C69 RID: 3177 RVA: 0x00065C31 File Offset: 0x00063E31
	public static bool FloatIsGreaterOrEqual(float _A, float _B)
	{
		return _A >= _B - 1E-05f;
	}

	// Token: 0x06000C6A RID: 3178 RVA: 0x00065C40 File Offset: 0x00063E40
	private static int ConvertFloat(float _Float, RoundingMethods _Method)
	{
		switch (_Method)
		{
		default:
			return Mathf.FloorToInt(_Float);
		case RoundingMethods.Ceil:
			return Mathf.CeilToInt(_Float);
		case RoundingMethods.Round:
			return ExtraMath.RoundOrFloor(_Float);
		}
	}

	// Token: 0x06000C6B RID: 3179 RVA: 0x00065C6C File Offset: 0x00063E6C
	public static int RoundOrFloor(float _Number)
	{
		int num = Mathf.FloorToInt(_Number);
		if (_Number - (float)num > 0.5f)
		{
			return num + 1;
		}
		return num;
	}

	// Token: 0x06000C6C RID: 3180 RVA: 0x00065C90 File Offset: 0x00063E90
	public static int RoundOrCeil(float _Number)
	{
		int num = Mathf.CeilToInt(_Number);
		if ((float)num - _Number > 0.5f)
		{
			return num - 1;
		}
		return num;
	}

	// Token: 0x06000C6D RID: 3181 RVA: 0x00065CB4 File Offset: 0x00063EB4
	public static int RoundWithBias(float _Number, float _Bias, RoundingMethods _IfEquals)
	{
		int num = Mathf.FloorToInt(_Number);
		if (_Number - (float)num > _Bias)
		{
			return num + 1;
		}
		if (_Number - (float)num < _Bias)
		{
			return num;
		}
		if (_IfEquals == RoundingMethods.Floor)
		{
			return num;
		}
		if (_IfEquals == RoundingMethods.Ceil)
		{
			return num + 1;
		}
		if (UnityEngine.Random.value <= 0.5f)
		{
			return num + 1;
		}
		return num;
	}

	// Token: 0x06000C6E RID: 3182 RVA: 0x00065CFC File Offset: 0x00063EFC
	public static bool FloatIsInRangeOrGreater(float _Value, Vector2 _Range, RoundingMethods _Rounding)
	{
		if (_Rounding == RoundingMethods.None)
		{
			if (_Range.y >= _Range.x)
			{
				return ExtraMath.FloatIsInRange(_Value, _Range, _Rounding);
			}
			return _Value >= _Range.x;
		}
		else
		{
			float num = Mathf.Abs(_Range.y - _Range.x);
			Vector2Int vector2Int;
			int num2;
			if (num >= 1f || Mathf.Approximately(num, 0f))
			{
				vector2Int = new Vector2Int(ExtraMath.ConvertFloat(_Range.x, _Rounding), ExtraMath.ConvertFloat(_Range.y, _Rounding));
				num2 = ExtraMath.ConvertFloat(_Value, _Rounding);
			}
			else
			{
				vector2Int = new Vector2Int(ExtraMath.ConvertFloat(_Range.x * 10f, _Rounding), ExtraMath.ConvertFloat(_Range.y * 10f, _Rounding));
				num2 = ExtraMath.ConvertFloat(_Value * 10f, _Rounding);
			}
			if (vector2Int.y >= vector2Int.x)
			{
				return vector2Int.x <= num2 && num2 <= vector2Int.y;
			}
			return vector2Int.x <= num2;
		}
	}

	// Token: 0x06000C6F RID: 3183 RVA: 0x00065DF1 File Offset: 0x00063FF1
	public static bool FloatIsInRangeOrGreater(float _Value, Vector2Int _Range, RoundingMethods _Rounding)
	{
		if (_Range.y > _Range.x)
		{
			return ExtraMath.FloatIsInRange(_Value, _Range, _Rounding);
		}
		if (_Rounding != RoundingMethods.None)
		{
			return ExtraMath.ConvertFloat(_Value, _Rounding) >= _Range.x;
		}
		return _Value >= (float)_Range.x;
	}

	// Token: 0x06000C70 RID: 3184 RVA: 0x00065E34 File Offset: 0x00064034
	public static bool FloatIsBeyondRange(float _Value, Vector2 _Range, RoundingMethods _Rounding)
	{
		if (_Rounding == RoundingMethods.None)
		{
			return _Value > Mathf.Max(_Range.x, _Range.y);
		}
		float num = Mathf.Abs(_Range.y - _Range.x);
		if (num >= 1f || Mathf.Approximately(num, 0f))
		{
			return ExtraMath.FloatIsBeyondRange(_Value, new Vector2Int(ExtraMath.ConvertFloat(_Range.x, _Rounding), ExtraMath.ConvertFloat(_Range.y, _Rounding)), _Rounding);
		}
		return ExtraMath.FloatIsBeyondRange(_Value * 10f, new Vector2Int(ExtraMath.ConvertFloat(_Range.x * 10f, _Rounding), ExtraMath.ConvertFloat(_Range.y * 10f, _Rounding)), _Rounding);
	}

	// Token: 0x06000C71 RID: 3185 RVA: 0x00065EDC File Offset: 0x000640DC
	public static bool FloatIsBeyondRange(float _Value, Vector2Int _Range, RoundingMethods _Rounding)
	{
		return ExtraMath.ConvertFloat(_Value, _Rounding) > Mathf.Max(_Range.x, _Range.y);
	}

	// Token: 0x06000C72 RID: 3186 RVA: 0x00065EFC File Offset: 0x000640FC
	public static bool FloatIsInRange(float _Value, Vector2 _Range, RoundingMethods _Rounding)
	{
		if (_Rounding == RoundingMethods.None)
		{
			if (ExtraMath.FloatIsEqual(_Range.x, _Range.y))
			{
				return ExtraMath.FloatIsEqual(_Value, _Range.x);
			}
			return ExtraMath.FloatIsGreaterOrEqual(_Value, Mathf.Min(_Range.x, _Range.y)) && ExtraMath.FloatIsLowerOrEqual(_Value, Mathf.Max(_Range.x, _Range.y));
		}
		else
		{
			float num = Mathf.Abs(_Range.y - _Range.x);
			if (num >= 1f || ExtraMath.FloatIsEqual(num, 0f))
			{
				return ExtraMath.FloatIsInRange(_Value, new Vector2Int(ExtraMath.ConvertFloat(_Range.x, _Rounding), ExtraMath.ConvertFloat(_Range.y, _Rounding)), _Rounding);
			}
			return ExtraMath.FloatIsInRange(_Value * 10f, new Vector2Int(ExtraMath.ConvertFloat(_Range.x * 10f, _Rounding), ExtraMath.ConvertFloat(_Range.y * 10f, _Rounding)), _Rounding);
		}
	}

	// Token: 0x06000C73 RID: 3187 RVA: 0x00065FE4 File Offset: 0x000641E4
	public static bool FloatIsInRange(float _Value, Vector2Int _Range, RoundingMethods _Rounding)
	{
		int num = ExtraMath.ConvertFloat(_Value, _Rounding);
		if (_Range.x == _Range.y)
		{
			return num == _Range.x;
		}
		return num >= Mathf.Min(_Range.x, _Range.y) && num <= Mathf.Max(_Range.x, _Range.y);
	}

	// Token: 0x04001146 RID: 4422
	public const float FloatCompareMargin = 1E-05f;
}
