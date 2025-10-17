using System;
using UnityEngine;

// Token: 0x02000178 RID: 376
public static class DebugDrawings
{
	// Token: 0x06000A21 RID: 2593 RVA: 0x0005AD14 File Offset: 0x00058F14
	public static void DrawRect(Rect _Rect, Color _Color)
	{
		if (_Rect == Rect.zero)
		{
			return;
		}
		Debug.DrawLine(_Rect.position, _Rect.position + new Vector2(_Rect.size.x, 0f), _Color);
		Debug.DrawLine(_Rect.position, _Rect.position + new Vector2(0f, _Rect.size.y), _Color);
		Debug.DrawLine(_Rect.position + _Rect.size, _Rect.position + new Vector2(_Rect.size.x, 0f), _Color);
		Debug.DrawLine(_Rect.position + _Rect.size, _Rect.position + new Vector2(0f, _Rect.size.y), _Color);
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0005AE2C File Offset: 0x0005902C
	public static void DrawPoint(Vector3 _Point, Color _Color, float _Size = 1f)
	{
		if (Mathf.Approximately(_Size, 0f))
		{
			return;
		}
		Debug.DrawLine(_Point - Vector3.right * _Size * 0.5f, _Point + Vector3.right * _Size * 0.5f, _Color);
		Debug.DrawLine(_Point - Vector3.up * _Size * 0.5f, _Point + Vector3.up * _Size * 0.5f, _Color);
		Debug.DrawLine(_Point - Vector3.forward * _Size * 0.5f, _Point + Vector3.forward * _Size * 0.5f, _Color);
	}
}
