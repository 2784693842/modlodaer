using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace LitJson
{
	// Token: 0x0200000F RID: 15
	public class JsonMapper
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x000031A0 File Offset: 0x000013A0
		static JsonMapper()
		{
			JsonMapper.max_nesting_depth = 100;
			JsonMapper.array_metadata = new Dictionary<Type, ArrayMetadata>();
			JsonMapper.conv_ops = new Dictionary<Type, IDictionary<Type, MethodInfo>>();
			JsonMapper.object_metadata = new Dictionary<Type, ObjectMetadata>();
			JsonMapper.type_properties = new Dictionary<Type, IList<PropertyMetadata>>();
			JsonMapper.static_writer = new JsonWriter();
			JsonMapper.datetime_format = DateTimeFormatInfo.InvariantInfo;
			JsonMapper.base_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.custom_exporters_table = new Dictionary<Type, ExporterFunc>();
			JsonMapper.base_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.custom_importers_table = new Dictionary<Type, IDictionary<Type, ImporterFunc>>();
			JsonMapper.RegisterBaseExporters();
			JsonMapper.RegisterBaseImporters();
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003254 File Offset: 0x00001454
		private static void AddArrayMetadata(Type type)
		{
			if (JsonMapper.array_metadata.ContainsKey(type))
			{
				return;
			}
			ArrayMetadata data = default(ArrayMetadata);
			data.IsArray = type.IsArray;
			if (type.GetInterface("System.Collections.IList") != null)
			{
				data.IsList = true;
			}
			foreach (PropertyInfo p_info in type.GetProperties())
			{
				if (!(p_info.Name != "Item"))
				{
					ParameterInfo[] parameters = p_info.GetIndexParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == typeof(int))
					{
						data.ElementType = p_info.PropertyType;
					}
				}
			}
			object obj = JsonMapper.array_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.array_metadata.Add(type, data);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000334C File Offset: 0x0000154C
		private static void AddObjectMetadata(Type type)
		{
			if (JsonMapper.object_metadata.ContainsKey(type))
			{
				return;
			}
			ObjectMetadata data = default(ObjectMetadata);
			if (type.GetInterface("System.Collections.IDictionary") != null)
			{
				data.IsDictionary = true;
			}
			data.Properties = new Dictionary<string, PropertyMetadata>();
			foreach (PropertyInfo p_info in type.GetProperties())
			{
				if (p_info.Name == "Item")
				{
					ParameterInfo[] parameters = p_info.GetIndexParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
					{
						data.ElementType = p_info.PropertyType;
					}
				}
				else
				{
					PropertyMetadata p_data = default(PropertyMetadata);
					p_data.Info = p_info;
					p_data.Type = p_info.PropertyType;
					data.Properties.Add(p_info.Name, p_data);
				}
			}
			foreach (FieldInfo f_info in type.GetFields())
			{
				PropertyMetadata p_data2 = default(PropertyMetadata);
				p_data2.Info = f_info;
				p_data2.IsField = true;
				p_data2.Type = f_info.FieldType;
				data.Properties.Add(f_info.Name, p_data2);
			}
			object obj = JsonMapper.object_metadata_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.object_metadata.Add(type, data);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000034D8 File Offset: 0x000016D8
		private static void AddTypeProperties(Type type)
		{
			if (JsonMapper.type_properties.ContainsKey(type))
			{
				return;
			}
			IList<PropertyMetadata> props = new List<PropertyMetadata>();
			foreach (PropertyInfo p_info in type.GetProperties())
			{
				if (!(p_info.Name == "Item"))
				{
					props.Add(new PropertyMetadata
					{
						Info = p_info,
						IsField = false
					});
				}
			}
			foreach (FieldInfo f_info in type.GetFields())
			{
				props.Add(new PropertyMetadata
				{
					Info = f_info,
					IsField = true
				});
			}
			object obj = JsonMapper.type_properties_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.type_properties.Add(type, props);
				}
				catch (ArgumentException)
				{
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000035D0 File Offset: 0x000017D0
		private static MethodInfo GetConvOp(Type t1, Type t2)
		{
			object obj = JsonMapper.conv_ops_lock;
			lock (obj)
			{
				if (!JsonMapper.conv_ops.ContainsKey(t1))
				{
					JsonMapper.conv_ops.Add(t1, new Dictionary<Type, MethodInfo>());
				}
			}
			if (JsonMapper.conv_ops[t1].ContainsKey(t2))
			{
				return JsonMapper.conv_ops[t1][t2];
			}
			MethodInfo op = t1.GetMethod("op_Implicit", new Type[]
			{
				t2
			});
			obj = JsonMapper.conv_ops_lock;
			lock (obj)
			{
				try
				{
					JsonMapper.conv_ops[t1].Add(t2, op);
				}
				catch (ArgumentException)
				{
					return JsonMapper.conv_ops[t1][t2];
				}
			}
			return op;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000036C0 File Offset: 0x000018C0
		private static object ReadValue(Type inst_type, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd)
			{
				return null;
			}
			Type underlying_type = Nullable.GetUnderlyingType(inst_type);
			Type value_type = underlying_type ?? inst_type;
			if (reader.Token == JsonToken.Null)
			{
				if (inst_type.IsClass || underlying_type != null)
				{
					return null;
				}
				throw new JsonException(string.Format("Can't assign null to an instance of type {0}", inst_type));
			}
			else
			{
				if (reader.Token != JsonToken.Double && reader.Token != JsonToken.Int && reader.Token != JsonToken.Long && reader.Token != JsonToken.String && reader.Token != JsonToken.Boolean)
				{
					object instance = null;
					if (reader.Token == JsonToken.ArrayStart)
					{
						JsonMapper.AddArrayMetadata(inst_type);
						ArrayMetadata t_data = JsonMapper.array_metadata[inst_type];
						if (!t_data.IsArray && !t_data.IsList)
						{
							throw new JsonException(string.Format("Type {0} can't act as an array", inst_type));
						}
						IList list;
						Type elem_type;
						if (!t_data.IsArray)
						{
							list = (IList)Activator.CreateInstance(inst_type);
							elem_type = t_data.ElementType;
						}
						else
						{
							list = new ArrayList();
							elem_type = inst_type.GetElementType();
						}
						list.Clear();
						for (;;)
						{
							object item = JsonMapper.ReadValue(elem_type, reader);
							if (item == null && reader.Token == JsonToken.ArrayEnd)
							{
								break;
							}
							list.Add(item);
						}
						if (t_data.IsArray)
						{
							int i = list.Count;
							instance = Array.CreateInstance(elem_type, i);
							for (int j = 0; j < i; j++)
							{
								((Array)instance).SetValue(list[j], j);
							}
						}
						else
						{
							instance = list;
						}
					}
					else if (reader.Token == JsonToken.ObjectStart)
					{
						JsonMapper.AddObjectMetadata(value_type);
						ObjectMetadata t_data2 = JsonMapper.object_metadata[value_type];
						instance = Activator.CreateInstance(value_type);
						string property;
						for (;;)
						{
							reader.Read();
							if (reader.Token == JsonToken.ObjectEnd)
							{
								return instance;
							}
							property = (string)reader.Value;
							if (t_data2.Properties.ContainsKey(property))
							{
								PropertyMetadata prop_data = t_data2.Properties[property];
								if (prop_data.IsField)
								{
									((FieldInfo)prop_data.Info).SetValue(instance, JsonMapper.ReadValue(prop_data.Type, reader));
								}
								else
								{
									PropertyInfo p_info = (PropertyInfo)prop_data.Info;
									if (p_info.CanWrite)
									{
										p_info.SetValue(instance, JsonMapper.ReadValue(prop_data.Type, reader), null);
									}
									else
									{
										JsonMapper.ReadValue(prop_data.Type, reader);
									}
								}
							}
							else if (!t_data2.IsDictionary)
							{
								if (!reader.SkipNonMembers)
								{
									break;
								}
								JsonMapper.ReadSkip(reader);
							}
							else
							{
								((IDictionary)instance).Add(property, JsonMapper.ReadValue(t_data2.ElementType, reader));
							}
						}
						throw new JsonException(string.Format("The type {0} doesn't have the property '{1}'", inst_type, property));
					}
					return instance;
				}
				Type json_type = reader.Value.GetType();
				if (value_type.IsAssignableFrom(json_type))
				{
					return reader.Value;
				}
				if (JsonMapper.custom_importers_table.ContainsKey(json_type) && JsonMapper.custom_importers_table[json_type].ContainsKey(value_type))
				{
					return JsonMapper.custom_importers_table[json_type][value_type](reader.Value);
				}
				if (JsonMapper.base_importers_table.ContainsKey(json_type) && JsonMapper.base_importers_table[json_type].ContainsKey(value_type))
				{
					return JsonMapper.base_importers_table[json_type][value_type](reader.Value);
				}
				if (value_type.IsEnum)
				{
					return Enum.ToObject(value_type, reader.Value);
				}
				MethodInfo conv_op = JsonMapper.GetConvOp(value_type, json_type);
				if (conv_op != null)
				{
					return conv_op.Invoke(null, new object[]
					{
						reader.Value
					});
				}
				throw new JsonException(string.Format("Can't assign value '{0}' (type {1}) to type {2}", reader.Value, json_type, inst_type));
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00003A50 File Offset: 0x00001C50
		private static IJsonWrapper ReadValue(WrapperFactory factory, JsonReader reader)
		{
			reader.Read();
			if (reader.Token == JsonToken.ArrayEnd || reader.Token == JsonToken.Null)
			{
				return null;
			}
			IJsonWrapper instance = factory();
			if (reader.Token == JsonToken.String)
			{
				instance.SetString((string)reader.Value);
				return instance;
			}
			if (reader.Token == JsonToken.Double)
			{
				instance.SetDouble((double)reader.Value);
				return instance;
			}
			if (reader.Token == JsonToken.Int)
			{
				instance.SetInt((int)reader.Value);
				return instance;
			}
			if (reader.Token == JsonToken.Long)
			{
				instance.SetLong((long)reader.Value);
				return instance;
			}
			if (reader.Token == JsonToken.Boolean)
			{
				instance.SetBoolean((bool)reader.Value);
				return instance;
			}
			if (reader.Token == JsonToken.ArrayStart)
			{
				instance.SetJsonType(JsonType.Array);
				for (;;)
				{
					IJsonWrapper item = JsonMapper.ReadValue(factory, reader);
					if (item == null && reader.Token == JsonToken.ArrayEnd)
					{
						break;
					}
					instance.Add(item);
				}
			}
			else if (reader.Token == JsonToken.ObjectStart)
			{
				instance.SetJsonType(JsonType.Object);
				for (;;)
				{
					reader.Read();
					if (reader.Token == JsonToken.ObjectEnd)
					{
						break;
					}
					string property = (string)reader.Value;
					instance[property] = JsonMapper.ReadValue(factory, reader);
				}
			}
			return instance;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003B79 File Offset: 0x00001D79
		private static void ReadSkip(JsonReader reader)
		{
			JsonMapper.ToWrapper(() => new JsonMockWrapper(), reader);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003BA4 File Offset: 0x00001DA4
		private static void RegisterBaseExporters()
		{
			JsonMapper.base_exporters_table[typeof(byte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((byte)obj));
			};
			JsonMapper.base_exporters_table[typeof(char)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((char)obj));
			};
			JsonMapper.base_exporters_table[typeof(DateTime)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToString((DateTime)obj, JsonMapper.datetime_format));
			};
			JsonMapper.base_exporters_table[typeof(decimal)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((decimal)obj);
			};
			JsonMapper.base_exporters_table[typeof(sbyte)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((sbyte)obj));
			};
			JsonMapper.base_exporters_table[typeof(short)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((short)obj));
			};
			JsonMapper.base_exporters_table[typeof(ushort)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToInt32((ushort)obj));
			};
			JsonMapper.base_exporters_table[typeof(uint)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(Convert.ToUInt64((uint)obj));
			};
			JsonMapper.base_exporters_table[typeof(ulong)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write((ulong)obj);
			};
			JsonMapper.base_exporters_table[typeof(DateTimeOffset)] = delegate(object obj, JsonWriter writer)
			{
				writer.Write(((DateTimeOffset)obj).ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz", JsonMapper.datetime_format));
			};
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00003DB0 File Offset: 0x00001FB0
		private static void RegisterBaseImporters()
		{
			ImporterFunc importer = (object input) => Convert.ToByte((int)input);
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(byte), importer);
			importer = ((object input) => Convert.ToUInt64((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ulong), importer);
			importer = ((object input) => Convert.ToInt64((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(long), importer);
			importer = ((object input) => Convert.ToSByte((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(sbyte), importer);
			importer = ((object input) => Convert.ToInt16((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(short), importer);
			importer = ((object input) => Convert.ToUInt16((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(ushort), importer);
			importer = ((object input) => Convert.ToUInt32((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(uint), importer);
			importer = ((object input) => Convert.ToSingle((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(float), importer);
			importer = ((object input) => Convert.ToDouble((int)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(int), typeof(double), importer);
			importer = ((object input) => Convert.ToDecimal((double)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(double), typeof(decimal), importer);
			importer = ((object input) => Convert.ToSingle((double)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(double), typeof(float), importer);
			importer = ((object input) => Convert.ToUInt32((long)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(long), typeof(uint), importer);
			importer = ((object input) => Convert.ToChar((string)input));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(char), importer);
			importer = ((object input) => Convert.ToDateTime((string)input, JsonMapper.datetime_format));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(DateTime), importer);
			importer = ((object input) => DateTimeOffset.Parse((string)input, JsonMapper.datetime_format));
			JsonMapper.RegisterImporter(JsonMapper.base_importers_table, typeof(string), typeof(DateTimeOffset), importer);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000416E File Offset: 0x0000236E
		private static void RegisterImporter(IDictionary<Type, IDictionary<Type, ImporterFunc>> table, Type json_type, Type value_type, ImporterFunc importer)
		{
			if (!table.ContainsKey(json_type))
			{
				table.Add(json_type, new Dictionary<Type, ImporterFunc>());
			}
			table[json_type][value_type] = importer;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004194 File Offset: 0x00002394
		private static void WriteValue(object obj, JsonWriter writer, bool writer_is_private, int depth)
		{
			if (depth > JsonMapper.max_nesting_depth)
			{
				throw new JsonException(string.Format("Max allowed object depth reached while trying to export from type {0}", obj.GetType()));
			}
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj is IJsonWrapper)
			{
				if (writer_is_private)
				{
					writer.TextWriter.Write(((IJsonWrapper)obj).ToJson());
					return;
				}
				((IJsonWrapper)obj).ToJson(writer);
				return;
			}
			else
			{
				if (obj is string)
				{
					writer.Write((string)obj);
					return;
				}
				if (obj is double)
				{
					writer.Write((double)obj);
					return;
				}
				if (obj is float)
				{
					writer.Write((float)obj);
					return;
				}
				if (obj is int)
				{
					writer.Write((int)obj);
					return;
				}
				if (obj is bool)
				{
					writer.Write((bool)obj);
					return;
				}
				if (obj is long)
				{
					writer.Write((long)obj);
					return;
				}
				if (obj is Array)
				{
					writer.WriteArrayStart();
					foreach (object obj2 in ((Array)obj))
					{
						JsonMapper.WriteValue(obj2, writer, writer_is_private, depth + 1);
					}
					writer.WriteArrayEnd();
					return;
				}
				if (obj is IList)
				{
					writer.WriteArrayStart();
					foreach (object obj3 in ((IList)obj))
					{
						JsonMapper.WriteValue(obj3, writer, writer_is_private, depth + 1);
					}
					writer.WriteArrayEnd();
					return;
				}
				IDictionary dictionary = obj as IDictionary;
				if (dictionary != null)
				{
					writer.WriteObjectStart();
					foreach (object obj4 in dictionary)
					{
						DictionaryEntry entry = (DictionaryEntry)obj4;
						string key = entry.Key as string;
						string propertyName = (key != null) ? key : Convert.ToString(entry.Key, CultureInfo.InvariantCulture);
						writer.WritePropertyName(propertyName);
						JsonMapper.WriteValue(entry.Value, writer, writer_is_private, depth + 1);
					}
					writer.WriteObjectEnd();
					return;
				}
				Type obj_type = obj.GetType();
				if (JsonMapper.custom_exporters_table.ContainsKey(obj_type))
				{
					JsonMapper.custom_exporters_table[obj_type](obj, writer);
					return;
				}
				if (JsonMapper.base_exporters_table.ContainsKey(obj_type))
				{
					JsonMapper.base_exporters_table[obj_type](obj, writer);
					return;
				}
				if (!(obj is Enum))
				{
					JsonMapper.AddTypeProperties(obj_type);
					IEnumerable<PropertyMetadata> enumerable = JsonMapper.type_properties[obj_type];
					writer.WriteObjectStart();
					foreach (PropertyMetadata p_data in enumerable)
					{
						if (p_data.IsField)
						{
							writer.WritePropertyName(p_data.Info.Name);
							JsonMapper.WriteValue(((FieldInfo)p_data.Info).GetValue(obj), writer, writer_is_private, depth + 1);
						}
						else
						{
							PropertyInfo p_info = (PropertyInfo)p_data.Info;
							if (p_info.CanRead)
							{
								writer.WritePropertyName(p_data.Info.Name);
								JsonMapper.WriteValue(p_info.GetValue(obj, null), writer, writer_is_private, depth + 1);
							}
						}
					}
					writer.WriteObjectEnd();
					return;
				}
				Type e_type = Enum.GetUnderlyingType(obj_type);
				if (e_type == typeof(long))
				{
					writer.Write((long)obj);
					return;
				}
				if (e_type == typeof(uint))
				{
					writer.Write((long)((ulong)((uint)obj)));
					return;
				}
				if (e_type == typeof(ulong))
				{
					writer.Write((ulong)obj);
					return;
				}
				if (e_type == typeof(ushort))
				{
					writer.Write((int)((ushort)obj));
					return;
				}
				if (e_type == typeof(short))
				{
					writer.Write((int)((short)obj));
					return;
				}
				if (e_type == typeof(byte))
				{
					writer.Write((int)((byte)obj));
					return;
				}
				if (e_type == typeof(sbyte))
				{
					writer.Write((int)((sbyte)obj));
					return;
				}
				writer.Write((int)obj);
				return;
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000045E0 File Offset: 0x000027E0
		public static string ToJson(object obj)
		{
			object obj2 = JsonMapper.static_writer_lock;
			string result;
			lock (obj2)
			{
				JsonMapper.static_writer.Reset();
				JsonMapper.WriteValue(obj, JsonMapper.static_writer, true, 0);
				result = JsonMapper.static_writer.ToString();
			}
			return result;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000463C File Offset: 0x0000283C
		public static void ToJson(object obj, JsonWriter writer)
		{
			JsonMapper.WriteValue(obj, writer, false, 0);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004647 File Offset: 0x00002847
		public static JsonData ToObject(JsonReader reader)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), reader);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004674 File Offset: 0x00002874
		public static JsonData ToObject(TextReader reader)
		{
			JsonReader json_reader = new JsonReader(reader);
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), json_reader);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000046B2 File Offset: 0x000028B2
		public static JsonData ToObject(string json)
		{
			return (JsonData)JsonMapper.ToWrapper(() => new JsonData(), json);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000046DE File Offset: 0x000028DE
		public static T ToObject<T>(JsonReader reader)
		{
			return (T)((object)JsonMapper.ReadValue(typeof(T), reader));
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000046F8 File Offset: 0x000028F8
		public static T ToObject<T>(TextReader reader)
		{
			JsonReader json_reader = new JsonReader(reader);
			return (T)((object)JsonMapper.ReadValue(typeof(T), json_reader));
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00004724 File Offset: 0x00002924
		public static T ToObject<T>(string json)
		{
			JsonReader reader = new JsonReader(json);
			return (T)((object)JsonMapper.ReadValue(typeof(T), reader));
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00004750 File Offset: 0x00002950
		public static object ToObject(string json, Type ConvertType)
		{
			JsonReader reader = new JsonReader(json);
			return JsonMapper.ReadValue(ConvertType, reader);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000476B File Offset: 0x0000296B
		public static IJsonWrapper ToWrapper(WrapperFactory factory, JsonReader reader)
		{
			return JsonMapper.ReadValue(factory, reader);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004774 File Offset: 0x00002974
		public static IJsonWrapper ToWrapper(WrapperFactory factory, string json)
		{
			JsonReader reader = new JsonReader(json);
			return JsonMapper.ReadValue(factory, reader);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00004790 File Offset: 0x00002990
		public static void RegisterExporter<T>(ExporterFunc<T> exporter)
		{
			ExporterFunc exporter_wrapper = delegate(object obj, JsonWriter writer)
			{
				exporter((T)((object)obj), writer);
			};
			JsonMapper.custom_exporters_table[typeof(T)] = exporter_wrapper;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000047CC File Offset: 0x000029CC
		public static void RegisterImporter<TJson, TValue>(ImporterFunc<TJson, TValue> importer)
		{
			ImporterFunc importer_wrapper = (object input) => importer((TJson)((object)input));
			JsonMapper.RegisterImporter(JsonMapper.custom_importers_table, typeof(TJson), typeof(TValue), importer_wrapper);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00004810 File Offset: 0x00002A10
		public static void UnregisterExporters()
		{
			JsonMapper.custom_exporters_table.Clear();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000481C File Offset: 0x00002A1C
		public static void UnregisterImporters()
		{
			JsonMapper.custom_importers_table.Clear();
		}

		// Token: 0x0400001E RID: 30
		private static readonly int max_nesting_depth;

		// Token: 0x0400001F RID: 31
		private static readonly IFormatProvider datetime_format;

		// Token: 0x04000020 RID: 32
		private static readonly IDictionary<Type, ExporterFunc> base_exporters_table;

		// Token: 0x04000021 RID: 33
		private static readonly IDictionary<Type, ExporterFunc> custom_exporters_table;

		// Token: 0x04000022 RID: 34
		private static readonly IDictionary<Type, IDictionary<Type, ImporterFunc>> base_importers_table;

		// Token: 0x04000023 RID: 35
		private static readonly IDictionary<Type, IDictionary<Type, ImporterFunc>> custom_importers_table;

		// Token: 0x04000024 RID: 36
		private static readonly IDictionary<Type, ArrayMetadata> array_metadata;

		// Token: 0x04000025 RID: 37
		private static readonly object array_metadata_lock = new object();

		// Token: 0x04000026 RID: 38
		private static readonly IDictionary<Type, IDictionary<Type, MethodInfo>> conv_ops;

		// Token: 0x04000027 RID: 39
		private static readonly object conv_ops_lock = new object();

		// Token: 0x04000028 RID: 40
		private static readonly IDictionary<Type, ObjectMetadata> object_metadata;

		// Token: 0x04000029 RID: 41
		private static readonly object object_metadata_lock = new object();

		// Token: 0x0400002A RID: 42
		private static readonly IDictionary<Type, IList<PropertyMetadata>> type_properties;

		// Token: 0x0400002B RID: 43
		private static readonly object type_properties_lock = new object();

		// Token: 0x0400002C RID: 44
		private static readonly JsonWriter static_writer;

		// Token: 0x0400002D RID: 45
		private static readonly object static_writer_lock = new object();
	}
}
