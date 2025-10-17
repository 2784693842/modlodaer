using System;

// Token: 0x020001C1 RID: 449
public static class SimpleComparison
{
	// Token: 0x06000C4F RID: 3151 RVA: 0x000658F0 File Offset: 0x00063AF0
	public static bool ValidComparison(SimpleComparison.Comparisons _Type, int _A, int _B)
	{
		switch (_Type)
		{
		case SimpleComparison.Comparisons.Different:
			return _A != _B;
		case SimpleComparison.Comparisons.Greater:
			return _A > _B;
		case SimpleComparison.Comparisons.GreaterOrEqual:
			return _A >= _B;
		case SimpleComparison.Comparisons.Equal:
			return _A == _B;
		case SimpleComparison.Comparisons.LowerOrEqual:
			return _A <= _B;
		case SimpleComparison.Comparisons.Lower:
			return _A < _B;
		default:
			return false;
		}
	}

	// Token: 0x06000C50 RID: 3152 RVA: 0x00065948 File Offset: 0x00063B48
	public static bool ValidComparison(SimpleComparison.Comparisons _Type, float _A, float _B)
	{
		switch (_Type)
		{
		case SimpleComparison.Comparisons.Different:
			return !ExtraMath.FloatIsEqual(_A, _B);
		case SimpleComparison.Comparisons.Greater:
			return _A > _B;
		case SimpleComparison.Comparisons.GreaterOrEqual:
			return ExtraMath.FloatIsGreaterOrEqual(_A, _B);
		case SimpleComparison.Comparisons.Equal:
			return ExtraMath.FloatIsEqual(_A, _B);
		case SimpleComparison.Comparisons.LowerOrEqual:
			return ExtraMath.FloatIsLowerOrEqual(_A, _B);
		case SimpleComparison.Comparisons.Lower:
			return _A < _B;
		default:
			return false;
		}
	}

	// Token: 0x020002A7 RID: 679
	public enum Comparisons
	{
		// Token: 0x04001577 RID: 5495
		Different,
		// Token: 0x04001578 RID: 5496
		Greater,
		// Token: 0x04001579 RID: 5497
		GreaterOrEqual,
		// Token: 0x0400157A RID: 5498
		Equal,
		// Token: 0x0400157B RID: 5499
		LowerOrEqual,
		// Token: 0x0400157C RID: 5500
		Lower
	}
}
