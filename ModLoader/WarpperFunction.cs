using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LitJson;
using ModLoader.LoaderUtil;
using UnityEngine;

namespace ModLoader
{
	// Token: 0x02000017 RID: 23
	public class WarpperFunction
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00003294 File Offset: 0x00001494
		public static void JsonCommonRefWarpper(object obj, string data, string field_name, Type field_type, WarpperFunction.WarpType warp_type = WarpperFunction.WarpType.REFERENCE)
		{
			if (field_type.IsSubclassOf(typeof(UniqueIDScriptable)))
			{
				WarpperFunction.ObjectReferenceWarpper<UniqueIDScriptable>(obj, data, field_name, ModLoader.AllGUIDDict);
				return;
			}
			if (field_type.IsSubclassOf(typeof(ScriptableObject)))
			{
				Dictionary<string, ScriptableObject> dict;
				if (ModLoader.AllScriptableObjectWithoutGuidTypeDict.TryGetValue(field_type, out dict))
				{
					WarpperFunction.ObjectReferenceWarpper<ScriptableObject>(obj, data, field_name, dict);
					return;
				}
				ModLoader.LogErrorWithModInfo("CommonWarpper No Such Dict " + field_type.Name);
				return;
			}
			else
			{
				if (field_type == typeof(Sprite))
				{
					Action<object, object> item = obj.GetType().FieldFromCache(field_name, false, true).Item3;
					obj.PostSetEnQueue(item, data);
					return;
				}
				if (field_type == typeof(AudioClip))
				{
					WarpperFunction.ObjectReferenceWarpper<AudioClip>(obj, data, field_name, ModLoader.AudioClipDict);
					return;
				}
				if (field_type == typeof(WeatherSpecialEffect) || field_type.IsSubclassOf(typeof(WeatherSpecialEffect)))
				{
					WarpperFunction.ObjectReferenceWarpper<WeatherSpecialEffect>(obj, data, field_name, ModLoader.WeatherSpecialEffectDict);
					return;
				}
				if (field_type == typeof(ScriptableObject))
				{
					WarpperFunction.ObjectReferenceWarpper<ScriptableObject>(obj, data, field_name, ModLoader.AllScriptableObjectDict);
					return;
				}
				ModLoader.LogErrorWithModInfo("JsonCommonRefWarpper Unexpect Object Type " + field_type.Name);
				return;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000033B8 File Offset: 0x000015B8
		public static void JsonCommonRefWarpper(object obj, List<string> data, string field_name, Type field_type, WarpperFunction.WarpType warp_type = WarpperFunction.WarpType.REFERENCE)
		{
			if (field_type.IsSubclassOf(typeof(UniqueIDScriptable)))
			{
				if (warp_type == WarpperFunction.WarpType.ADD_REFERENCE)
				{
					WarpperFunction.ObjectAddReferenceWarpper<UniqueIDScriptable>(obj, data, field_name, ModLoader.AllGUIDDict);
					return;
				}
				WarpperFunction.ObjectReferenceWarpper<UniqueIDScriptable>(obj, data, field_name, ModLoader.AllGUIDDict);
				return;
			}
			else if (field_type.IsSubclassOf(typeof(ScriptableObject)))
			{
				Dictionary<string, ScriptableObject> dict;
				if (!ModLoader.AllScriptableObjectWithoutGuidTypeDict.TryGetValue(field_type, out dict))
				{
					ModLoader.LogErrorWithModInfo("CommonWarpper No Such Dict " + field_type.Name);
					return;
				}
				if (warp_type == WarpperFunction.WarpType.ADD_REFERENCE)
				{
					WarpperFunction.ObjectAddReferenceWarpper<ScriptableObject>(obj, data, field_name, dict);
					return;
				}
				WarpperFunction.ObjectReferenceWarpper<ScriptableObject>(obj, data, field_name, dict);
				return;
			}
			else if (field_type == typeof(Sprite))
			{
				if (warp_type == WarpperFunction.WarpType.ADD_REFERENCE)
				{
					WarpperFunction.ObjectAddReferenceWarpper<Sprite>(obj, data, field_name, ModLoader.SpriteDict);
					return;
				}
				WarpperFunction.ObjectReferenceWarpper<Sprite>(obj, data, field_name, ModLoader.SpriteDict);
				return;
			}
			else if (field_type == typeof(AudioClip))
			{
				if (warp_type == WarpperFunction.WarpType.ADD_REFERENCE)
				{
					WarpperFunction.ObjectAddReferenceWarpper<AudioClip>(obj, data, field_name, ModLoader.AudioClipDict);
					return;
				}
				WarpperFunction.ObjectReferenceWarpper<AudioClip>(obj, data, field_name, ModLoader.AudioClipDict);
				return;
			}
			else
			{
				if (field_type == typeof(WeatherSpecialEffect) || field_type.IsSubclassOf(typeof(WeatherSpecialEffect)))
				{
					WarpperFunction.ObjectReferenceWarpper<WeatherSpecialEffect>(obj, data, field_name, ModLoader.WeatherSpecialEffectDict);
					return;
				}
				if (!(field_type == typeof(ScriptableObject)))
				{
					ModLoader.LogErrorWithModInfo("JsonCommonRefWarpper Unexpect List Object Type " + field_type.Name);
					return;
				}
				if (warp_type == WarpperFunction.WarpType.ADD_REFERENCE)
				{
					WarpperFunction.ObjectAddReferenceWarpper<ScriptableObject>(obj, data, field_name, ModLoader.AllScriptableObjectDict);
					return;
				}
				WarpperFunction.ObjectReferenceWarpper<ScriptableObject>(obj, data, field_name, ModLoader.AllScriptableObjectDict);
				return;
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003528 File Offset: 0x00001728
		public static void JsonCommonWarpper(object obj, JsonData json)
		{
			if (!json.IsObject)
			{
				return;
			}
			Type type = obj.GetType();
			UniqueIDScriptable uniqueIDScriptable = obj as UniqueIDScriptable;
			if (uniqueIDScriptable != null && json.ContainsKey(WarpperFunction.ExtraData))
			{
				ModLoader.UniqueIdObjectExtraData[uniqueIDScriptable.UniqueID] = json[WarpperFunction.ExtraData];
			}
			else
			{
				ScriptableObject scriptableObject = obj as ScriptableObject;
				if (scriptableObject != null && json.ContainsKey(WarpperFunction.ExtraData))
				{
					ModLoader.ScriptableObjectExtraData[scriptableObject.GetInstanceID()] = json[WarpperFunction.ExtraData];
				}
				else if (!type.IsValueType && json.ContainsKey(WarpperFunction.ExtraData))
				{
					ModLoader.ClassObjectExtraData[obj] = json[WarpperFunction.ExtraData];
				}
			}
			foreach (string text in json.Keys)
			{
				try
				{
					JsonData jsonData = json[text];
					if (text.EndsWith("WarpType"))
					{
						if (jsonData.IsInt && json.ContainsKey(text.Substring(0, text.Length - 8) + "WarpData"))
						{
							if ((int)jsonData == 3 || (int)jsonData == 6)
							{
								string text2 = text.Substring(0, text.Length - 8);
								FieldInfo item = type.FieldFromCache(text2, false, false).Item1;
								Type fieldType = item.FieldType;
								JsonData jsonData2 = json[text2 + "WarpData"];
								if (jsonData2.IsString)
								{
									WarpperFunction.JsonCommonRefWarpper(obj, jsonData2.ToString(), text2, fieldType, (WarpperFunction.WarpType)((int)jsonData));
								}
								else if (jsonData2.IsArray)
								{
									Type field_type = null;
									if (item.FieldType.IsGenericType && item.FieldType.GetGenericTypeDefinition() == typeof(List<>))
									{
										field_type = item.FieldType.GetGenericArguments().Single<Type>();
									}
									else if (item.FieldType.IsArray)
									{
										field_type = item.FieldType.GetElementType();
									}
									else
									{
										ModLoader.LogErrorWithModInfo("CommonWarpper REFERENCE Must be list or array " + fieldType.Name);
									}
									List<string> list = new List<string>();
									for (int i = 0; i < jsonData2.Count; i++)
									{
										if (jsonData2[i].IsString)
										{
											list.Add(jsonData2[i].ToString());
										}
										else
										{
											ModLoader.LogErrorWithModInfo("CommonWarpper REFERENCE Wrong SubWarpData Format " + fieldType.Name);
										}
									}
									if (list.Count != jsonData2.Count)
									{
										ModLoader.LogErrorWithModInfo("CommonWarpper REFERENCE Size Error" + fieldType.Name);
									}
									WarpperFunction.JsonCommonRefWarpper(obj, list, text2, field_type, (WarpperFunction.WarpType)((int)jsonData));
								}
								else
								{
									ModLoader.LogErrorWithModInfo("CommonWarpper REFERENCE Wrong WarpData Format " + fieldType.Name);
								}
							}
							else if ((int)jsonData == 4)
							{
								string text3 = text.Substring(0, text.Length - 8);
								ValueTuple<FieldInfo, Func<object, object>, Action<object, object>> valueTuple = type.FieldFromCache(text3, true, true);
								FieldInfo item2 = valueTuple.Item1;
								Func<object, object> item3 = valueTuple.Item2;
								Action<object, object> item4 = valueTuple.Item3;
								Type fieldType2 = item2.FieldType;
								JsonData jsonData3 = json[text3 + "WarpData"];
								if (jsonData3.IsArray)
								{
									if (item2.FieldType.IsGenericType && item2.FieldType.GetGenericTypeDefinition() == typeof(List<>))
									{
										Type type2 = item2.FieldType.GetGenericArguments().Single<Type>();
										IList list2 = item3(obj) as IList;
										for (int j = 0; j < jsonData3.Count; j++)
										{
											if (jsonData3[j].IsObject)
											{
												object obj2 = type2.IsSubclassOf(typeof(ScriptableObject)) ? ScriptableObject.CreateInstance(type2) : type2.ConstructorFromCache()();
												JsonUtility.FromJsonOverwrite(jsonData3[j].ToJson(), obj2);
												WarpperFunction.JsonCommonWarpper(obj2, jsonData3[j]);
												list2.Add(obj2);
											}
											else
											{
												ModLoader.LogErrorWithModInfo("CommonWarpper ADD Wrong SubWarpData Format " + fieldType2.Name);
											}
										}
									}
									else if (item2.FieldType.IsArray)
									{
										Type type2 = item2.FieldType.GetElementType();
										Array array = item3(obj) as Array;
										int length = array.Length;
										WarpperFunction.ArrayResize(ref array, jsonData3.Count + array.Length);
										for (int k = 0; k < jsonData3.Count; k++)
										{
											if (jsonData3[k].IsObject)
											{
												object obj3 = type2.IsSubclassOf(typeof(ScriptableObject)) ? ScriptableObject.CreateInstance(type2) : type2.ConstructorFromCache()();
												JsonUtility.FromJsonOverwrite(jsonData3[k].ToJson(), obj3);
												WarpperFunction.JsonCommonWarpper(obj3, jsonData3[k]);
												array.SetValue(obj3, k + length);
											}
											else
											{
												ModLoader.LogErrorWithModInfo("CommonWarpper ADD Wrong SubWarpData Format " + fieldType2.Name);
											}
										}
										item4(obj, array);
									}
									else
									{
										ModLoader.LogErrorWithModInfo("CommonWarpper ADD Must be list or array " + fieldType2.Name);
									}
								}
								else
								{
									ModLoader.LogErrorWithModInfo("CommonWarpper ADD Wrong WarpData Format " + fieldType2.Name);
								}
							}
							else if ((int)jsonData == 5)
							{
								string text4 = text.Substring(0, text.Length - 8);
								ValueTuple<FieldInfo, Func<object, object>, Action<object, object>> valueTuple2 = type.FieldFromCache(text4, true, true);
								FieldInfo item5 = valueTuple2.Item1;
								Func<object, object> item6 = valueTuple2.Item2;
								Action<object, object> item7 = valueTuple2.Item3;
								Type fieldType3 = item5.FieldType;
								JsonData jsonData4 = json[text4 + "WarpData"];
								if (jsonData4.IsObject)
								{
									object obj4 = item6(obj);
									WarpperFunction.JsonCommonWarpper(obj4, jsonData4);
									item7(obj, obj4);
								}
								else if (jsonData4.IsArray)
								{
									if (item5.FieldType.IsGenericType && item5.FieldType.GetGenericTypeDefinition() == typeof(List<>))
									{
										IList list3 = item6(obj) as IList;
										for (int l = 0; l < jsonData4.Count; l++)
										{
											if (jsonData4[l].IsObject)
											{
												object obj5 = list3[l];
												WarpperFunction.JsonCommonWarpper(obj5, jsonData4[l]);
												list3[l] = obj5;
											}
											else
											{
												ModLoader.LogErrorWithModInfo("CommonWarpper MODIFY Wrong SubWarpData Format " + fieldType3.Name);
											}
										}
									}
									else if (item5.FieldType.IsArray)
									{
										Array array2 = item6(obj) as Array;
										for (int m = 0; m < jsonData4.Count; m++)
										{
											if (jsonData4[m].IsObject)
											{
												object obj6 = null;
												try
												{
													obj6 = array2.GetValue(m);
												}
												catch (Exception ex)
												{
													string text5 = "NullId";
													UniqueIDScriptable uniqueIDScriptable2 = obj as UniqueIDScriptable;
													if (uniqueIDScriptable2 != null)
													{
														text5 = uniqueIDScriptable2.UniqueID;
													}
													else
													{
														ScriptableObject scriptableObject2 = obj as ScriptableObject;
														if (scriptableObject2 != null)
														{
															text5 = scriptableObject2.name;
														}
														else
														{
															CardAction cardAction = obj as CardAction;
															if (cardAction != null)
															{
																text5 = cardAction.ActionName.ParentObjectID;
															}
														}
													}
													Debug.LogWarning(string.Format("On access {0}::{1}.{2} : {3}", new object[]
													{
														text5,
														type,
														text4,
														ex
													}));
												}
												WarpperFunction.JsonCommonWarpper(obj6, jsonData4[m]);
												array2.SetValue(obj6, m);
											}
											else
											{
												ModLoader.LogErrorWithModInfo("CommonWarpper MODIFY Wrong SubWarpData Format " + fieldType3.Name);
											}
										}
										item7(obj, array2);
									}
									else
									{
										ModLoader.LogErrorWithModInfo("CommonWarpper MODIFY Must be list or array " + fieldType3.Name);
									}
								}
								else
								{
									ModLoader.LogErrorWithModInfo("CommonWarpper MODIFY Wrong WarpData Format " + fieldType3.Name);
								}
							}
							else
							{
								ModLoader.LogErrorWithModInfo("CommonWarpper Unexpect WarpType");
							}
						}
					}
					else if (!text.EndsWith("WarpData"))
					{
						if (jsonData.IsObject)
						{
							string field_name = text;
							ValueTuple<FieldInfo, Func<object, object>, Action<object, object>> valueTuple3 = type.FieldFromCache(field_name, true, true);
							FieldInfo item8 = valueTuple3.Item1;
							Func<object, object> item9 = valueTuple3.Item2;
							Action<object, object> item10 = valueTuple3.Item3;
							if (!item8.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
							{
								object obj7 = item9(obj);
								WarpperFunction.JsonCommonWarpper(obj7, jsonData);
								item10(obj, obj7);
							}
						}
						else if (jsonData.IsArray)
						{
							string text6 = text;
							ValueTuple<FieldInfo, Func<object, object>, Action<object, object>> valueTuple4 = type.FieldFromCache(text6, true, true);
							FieldInfo item11 = valueTuple4.Item1;
							Func<object, object> item12 = valueTuple4.Item2;
							Action<object, object> item13 = valueTuple4.Item3;
							for (int n = 0; n < jsonData.Count; n++)
							{
								if (jsonData[n].IsObject)
								{
									if (item11.FieldType.IsGenericType && item11.FieldType.GetGenericTypeDefinition() == typeof(List<>))
									{
										if (item11.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
										{
											break;
										}
										IList list4 = item12(obj) as IList;
										object obj8 = list4[n];
										if (obj8 != null)
										{
											WarpperFunction.JsonCommonWarpper(obj8, jsonData[n]);
											list4[n] = obj8;
											item13(obj, list4);
										}
									}
									else if (item11.FieldType.IsArray)
									{
										if (item11.FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
										{
											break;
										}
										Array array3 = item12(obj) as Array;
										object obj9 = null;
										try
										{
											obj9 = array3.GetValue(n);
										}
										catch (Exception ex2)
										{
											string text7 = "NullId";
											UniqueIDScriptable uniqueIDScriptable3 = obj as UniqueIDScriptable;
											if (uniqueIDScriptable3 != null)
											{
												text7 = uniqueIDScriptable3.UniqueID;
											}
											else
											{
												ScriptableObject scriptableObject3 = obj as ScriptableObject;
												if (scriptableObject3 != null)
												{
													text7 = scriptableObject3.name;
												}
											}
											Debug.LogWarning(string.Format("On access {0}::{1}.{2} : {3}", new object[]
											{
												text7,
												type,
												text6,
												ex2
											}));
										}
										if (obj9 != null)
										{
											WarpperFunction.JsonCommonWarpper(obj9, jsonData[n]);
											array3.SetValue(obj9, n);
											item13(obj, array3);
										}
									}
								}
							}
						}
					}
				}
				catch (Exception ex3)
				{
					ModLoader.LogErrorWithModInfo(string.Format("CommonWarpper {0} {1}", type.Name, ex3.Message));
				}
			}
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00004004 File Offset: 0x00002204
		public static void ObjectReferenceWarpper<TValueType>(object obj, string data, string field_name, Dictionary<string, TValueType> dict)
		{
			TValueType tvalueType;
			if (dict.TryGetValue(data, out tvalueType))
			{
				try
				{
					obj.GetType().FieldFromCache(field_name, false, true).Item3(obj, tvalueType);
				}
				catch (Exception ex)
				{
					ModLoader.LogErrorWithModInfo(string.Format("ObjectReferenceWarpper {0}.{1} {2}", obj.GetType().Name, field_name, ex.Message));
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00004074 File Offset: 0x00002274
		public static void ObjectReferenceWarpper<TValueType>(object obj, List<string> data, string field_name, Dictionary<string, TValueType> dict)
		{
			try
			{
				ValueTuple<FieldInfo, Func<object, object>, Action<object, object>> valueTuple = obj.GetType().FieldFromCache(field_name, true, true);
				FieldInfo item = valueTuple.Item1;
				Func<object, object> item2 = valueTuple.Item2;
				Action<object, object> item3 = valueTuple.Item3;
				if (item.FieldType.IsGenericType && item.FieldType.GetGenericTypeDefinition() == typeof(List<>))
				{
					IList list = item2(obj) as IList;
					using (List<string>.Enumerator enumerator = data.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string key = enumerator.Current;
							TValueType tvalueType;
							if (dict.TryGetValue(key, out tvalueType) && list != null)
							{
								list.Add(tvalueType);
							}
						}
						goto IL_109;
					}
				}
				if (item.FieldType.IsArray)
				{
					Array array = item2(obj) as Array;
					WarpperFunction.ArrayResize(ref array, data.Count);
					for (int i = 0; i < data.Count; i++)
					{
						TValueType tvalueType2;
						if (dict.TryGetValue(data[i], out tvalueType2))
						{
							array.SetValue(tvalueType2, i);
						}
					}
					item3(obj, array);
				}
				IL_109:;
			}
			catch (Exception ex)
			{
				ModLoader.LogErrorWithModInfo(string.Format("ObjectReferenceWarpper {0}.{1} {2}", obj.GetType().Name, field_name, ex.Message));
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000041E8 File Offset: 0x000023E8
		public static void ObjectAddReferenceWarpper<TValueType>(object obj, string data, string field_name, Dictionary<string, TValueType> dict)
		{
			ModLoader.LogErrorWithModInfo(string.Format("ObjectAddReferenceWarpper {0}.{1} {2}", obj.GetType().Name, field_name, "AddReferenceWarpper Only Vaild in List or Array Filed"));
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000420C File Offset: 0x0000240C
		public static void ObjectAddReferenceWarpper<TValueType>(object obj, List<string> data, string field_name, Dictionary<string, TValueType> dict)
		{
			try
			{
				ValueTuple<FieldInfo, Func<object, object>, Action<object, object>> valueTuple = obj.GetType().FieldFromCache(field_name, true, true);
				FieldInfo item = valueTuple.Item1;
				Func<object, object> item2 = valueTuple.Item2;
				Action<object, object> item3 = valueTuple.Item3;
				if (item.FieldType.IsGenericType && item.FieldType.GetGenericTypeDefinition() == typeof(List<>))
				{
					IList list = item2(obj) as IList;
					using (List<string>.Enumerator enumerator = data.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							string key = enumerator.Current;
							TValueType tvalueType;
							if (dict.TryGetValue(key, out tvalueType))
							{
								list.Add(tvalueType);
							}
						}
						goto IL_11D;
					}
				}
				if (item.FieldType.IsArray)
				{
					Array array = item2(obj) as Array;
					int length = array.Length;
					WarpperFunction.ArrayResize(ref array, data.Count + array.Length);
					for (int i = 0; i < data.Count; i++)
					{
						TValueType tvalueType2;
						if (dict.TryGetValue(data[i], out tvalueType2))
						{
							array.SetValue(tvalueType2, i + length);
						}
					}
					item3(obj, array);
				}
				IL_11D:;
			}
			catch (Exception ex)
			{
				ModLoader.LogErrorWithModInfo(string.Format("ObjectAddReferenceWarpper {0}.{1} {2}", obj.GetType().Name, field_name, ex.Message));
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00004394 File Offset: 0x00002594
		public static void ArrayResize(ref Array array, int newSize)
		{
			Array array2 = Array.CreateInstance(array.GetType().GetElementType(), newSize);
			Array.Copy(array, array2, Math.Min(array.Length, array2.Length));
			array = array2;
		}

		// Token: 0x0400003C RID: 60
		private static readonly string ExtraData = "额外数据ExtraData";

		// Token: 0x02000018 RID: 24
		public enum WarpType
		{
			// Token: 0x0400003E RID: 62
			NONE,
			// Token: 0x0400003F RID: 63
			COPY,
			// Token: 0x04000040 RID: 64
			CUSTOM,
			// Token: 0x04000041 RID: 65
			REFERENCE,
			// Token: 0x04000042 RID: 66
			ADD,
			// Token: 0x04000043 RID: 67
			MODIFY,
			// Token: 0x04000044 RID: 68
			ADD_REFERENCE
		}
	}
}
