using System;
using System.Runtime.InteropServices;
using System.Security;

namespace KeraLua
{
	// Token: 0x020000A4 RID: 164
	[SuppressUnmanagedCodeSecurity]
	internal static class NativeMethods
	{
		// Token: 0x060004AD RID: 1197
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_absindex(IntPtr luaState, int idx);

		// Token: 0x060004AE RID: 1198
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_arith(IntPtr luaState, int op);

		// Token: 0x060004AF RID: 1199
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_atpanic(IntPtr luaState, IntPtr panicf);

		// Token: 0x060004B0 RID: 1200
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_callk(IntPtr luaState, int nargs, int nresults, IntPtr ctx, IntPtr k);

		// Token: 0x060004B1 RID: 1201
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_checkstack(IntPtr luaState, int extra);

		// Token: 0x060004B2 RID: 1202
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_close(IntPtr luaState);

		// Token: 0x060004B3 RID: 1203
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_compare(IntPtr luaState, int index1, int index2, int op);

		// Token: 0x060004B4 RID: 1204
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_concat(IntPtr luaState, int n);

		// Token: 0x060004B5 RID: 1205
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_copy(IntPtr luaState, int fromIndex, int toIndex);

		// Token: 0x060004B6 RID: 1206
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_createtable(IntPtr luaState, int elements, int records);

		// Token: 0x060004B7 RID: 1207
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_dump(IntPtr luaState, IntPtr writer, IntPtr data, int strip);

		// Token: 0x060004B8 RID: 1208
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_error(IntPtr luaState);

		// Token: 0x060004B9 RID: 1209
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_gc(IntPtr luaState, int what, int data);

		// Token: 0x060004BA RID: 1210
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_gc(IntPtr luaState, int what, int data, int data2);

		// Token: 0x060004BB RID: 1211
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_getallocf(IntPtr luaState, ref IntPtr ud);

		// Token: 0x060004BC RID: 1212
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int lua_getfield(IntPtr luaState, int index, string k);

		// Token: 0x060004BD RID: 1213
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int lua_getglobal(IntPtr luaState, string name);

		// Token: 0x060004BE RID: 1214
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_gethook(IntPtr luaState);

		// Token: 0x060004BF RID: 1215
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_gethookcount(IntPtr luaState);

		// Token: 0x060004C0 RID: 1216
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_gethookmask(IntPtr luaState);

		// Token: 0x060004C1 RID: 1217
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_geti(IntPtr luaState, int index, long i);

		// Token: 0x060004C2 RID: 1218
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int lua_getinfo(IntPtr luaState, string what, IntPtr ar);

		// Token: 0x060004C3 RID: 1219
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_getiuservalue(IntPtr luaState, int idx, int n);

		// Token: 0x060004C4 RID: 1220
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern IntPtr lua_getlocal(IntPtr luaState, IntPtr ar, int n);

		// Token: 0x060004C5 RID: 1221
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_getmetatable(IntPtr luaState, int index);

		// Token: 0x060004C6 RID: 1222
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_getstack(IntPtr luaState, int level, IntPtr n);

		// Token: 0x060004C7 RID: 1223
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_gettable(IntPtr luaState, int index);

		// Token: 0x060004C8 RID: 1224
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_gettop(IntPtr luaState);

		// Token: 0x060004C9 RID: 1225
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern IntPtr lua_getupvalue(IntPtr luaState, int funcIndex, int n);

		// Token: 0x060004CA RID: 1226
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_iscfunction(IntPtr luaState, int index);

		// Token: 0x060004CB RID: 1227
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_isinteger(IntPtr luaState, int index);

		// Token: 0x060004CC RID: 1228
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_isnumber(IntPtr luaState, int index);

		// Token: 0x060004CD RID: 1229
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_isstring(IntPtr luaState, int index);

		// Token: 0x060004CE RID: 1230
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_isuserdata(IntPtr luaState, int index);

		// Token: 0x060004CF RID: 1231
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_isyieldable(IntPtr luaState);

		// Token: 0x060004D0 RID: 1232
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_len(IntPtr luaState, int index);

		// Token: 0x060004D1 RID: 1233
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int lua_load(IntPtr luaState, IntPtr reader, IntPtr data, string chunkName, string mode);

		// Token: 0x060004D2 RID: 1234
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_newstate(IntPtr allocFunction, IntPtr ud);

		// Token: 0x060004D3 RID: 1235
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_newthread(IntPtr luaState);

		// Token: 0x060004D4 RID: 1236
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_newuserdatauv(IntPtr luaState, UIntPtr size, int nuvalue);

		// Token: 0x060004D5 RID: 1237
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_next(IntPtr luaState, int index);

		// Token: 0x060004D6 RID: 1238
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_pcallk(IntPtr luaState, int nargs, int nresults, int errorfunc, IntPtr ctx, IntPtr k);

		// Token: 0x060004D7 RID: 1239
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_pushboolean(IntPtr luaState, int value);

		// Token: 0x060004D8 RID: 1240
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_pushcclosure(IntPtr luaState, IntPtr f, int n);

		// Token: 0x060004D9 RID: 1241
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_pushinteger(IntPtr luaState, long n);

		// Token: 0x060004DA RID: 1242
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_pushlightuserdata(IntPtr luaState, IntPtr udata);

		// Token: 0x060004DB RID: 1243
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_pushlstring(IntPtr luaState, byte[] s, UIntPtr len);

		// Token: 0x060004DC RID: 1244
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_pushnil(IntPtr luaState);

		// Token: 0x060004DD RID: 1245
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_pushnumber(IntPtr luaState, double number);

		// Token: 0x060004DE RID: 1246
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_pushthread(IntPtr luaState);

		// Token: 0x060004DF RID: 1247
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_pushvalue(IntPtr luaState, int index);

		// Token: 0x060004E0 RID: 1248
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_rawequal(IntPtr luaState, int index1, int index2);

		// Token: 0x060004E1 RID: 1249
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_rawget(IntPtr luaState, int index);

		// Token: 0x060004E2 RID: 1250
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_rawgeti(IntPtr luaState, int index, long n);

		// Token: 0x060004E3 RID: 1251
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_rawgetp(IntPtr luaState, int index, IntPtr p);

		// Token: 0x060004E4 RID: 1252
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern UIntPtr lua_rawlen(IntPtr luaState, int index);

		// Token: 0x060004E5 RID: 1253
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_rawset(IntPtr luaState, int index);

		// Token: 0x060004E6 RID: 1254
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_rawseti(IntPtr luaState, int index, long i);

		// Token: 0x060004E7 RID: 1255
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_rawsetp(IntPtr luaState, int index, IntPtr p);

		// Token: 0x060004E8 RID: 1256
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_resetthread(IntPtr luaState);

		// Token: 0x060004E9 RID: 1257
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_resume(IntPtr luaState, IntPtr from, int nargs, out int results);

		// Token: 0x060004EA RID: 1258
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_rotate(IntPtr luaState, int index, int n);

		// Token: 0x060004EB RID: 1259
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_setallocf(IntPtr luaState, IntPtr f, IntPtr ud);

		// Token: 0x060004EC RID: 1260
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern void lua_setfield(IntPtr luaState, int index, string key);

		// Token: 0x060004ED RID: 1261
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern void lua_setglobal(IntPtr luaState, string key);

		// Token: 0x060004EE RID: 1262
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_sethook(IntPtr luaState, IntPtr f, int mask, int count);

		// Token: 0x060004EF RID: 1263
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_seti(IntPtr luaState, int index, long n);

		// Token: 0x060004F0 RID: 1264
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_setiuservalue(IntPtr luaState, int index, int n);

		// Token: 0x060004F1 RID: 1265
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern IntPtr lua_setlocal(IntPtr luaState, IntPtr ar, int n);

		// Token: 0x060004F2 RID: 1266
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_setmetatable(IntPtr luaState, int objIndex);

		// Token: 0x060004F3 RID: 1267
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_settable(IntPtr luaState, int index);

		// Token: 0x060004F4 RID: 1268
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_settop(IntPtr luaState, int newTop);

		// Token: 0x060004F5 RID: 1269
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_setupvalue(IntPtr luaState, int funcIndex, int n);

		// Token: 0x060004F6 RID: 1270
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_setwarnf(IntPtr luaState, IntPtr warningFunctionPtr, IntPtr ud);

		// Token: 0x060004F7 RID: 1271
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_status(IntPtr luaState);

		// Token: 0x060004F8 RID: 1272
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern UIntPtr lua_stringtonumber(IntPtr luaState, string s);

		// Token: 0x060004F9 RID: 1273
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_toboolean(IntPtr luaState, int index);

		// Token: 0x060004FA RID: 1274
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_tocfunction(IntPtr luaState, int index);

		// Token: 0x060004FB RID: 1275
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_toclose(IntPtr luaState, int index);

		// Token: 0x060004FC RID: 1276
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern long lua_tointegerx(IntPtr luaState, int index, out int isNum);

		// Token: 0x060004FD RID: 1277
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_tolstring(IntPtr luaState, int index, out UIntPtr strLen);

		// Token: 0x060004FE RID: 1278
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern double lua_tonumberx(IntPtr luaState, int index, out int isNum);

		// Token: 0x060004FF RID: 1279
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_topointer(IntPtr luaState, int index);

		// Token: 0x06000500 RID: 1280
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_tothread(IntPtr luaState, int index);

		// Token: 0x06000501 RID: 1281
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_touserdata(IntPtr luaState, int index);

		// Token: 0x06000502 RID: 1282
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_type(IntPtr luaState, int index);

		// Token: 0x06000503 RID: 1283
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern IntPtr lua_typename(IntPtr luaState, int type);

		// Token: 0x06000504 RID: 1284
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr lua_upvalueid(IntPtr luaState, int funcIndex, int n);

		// Token: 0x06000505 RID: 1285
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_upvaluejoin(IntPtr luaState, int funcIndex1, int n1, int funcIndex2, int n2);

		// Token: 0x06000506 RID: 1286
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern double lua_version(IntPtr luaState);

		// Token: 0x06000507 RID: 1287
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern void lua_warning(IntPtr luaState, string msg, int tocont);

		// Token: 0x06000508 RID: 1288
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void lua_xmove(IntPtr from, IntPtr to, int n);

		// Token: 0x06000509 RID: 1289
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int lua_yieldk(IntPtr luaState, int nresults, IntPtr ctx, IntPtr k);

		// Token: 0x0600050A RID: 1290
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_argerror(IntPtr luaState, int arg, string message);

		// Token: 0x0600050B RID: 1291
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_callmeta(IntPtr luaState, int obj, string e);

		// Token: 0x0600050C RID: 1292
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void luaL_checkany(IntPtr luaState, int arg);

		// Token: 0x0600050D RID: 1293
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern long luaL_checkinteger(IntPtr luaState, int arg);

		// Token: 0x0600050E RID: 1294
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr luaL_checklstring(IntPtr luaState, int arg, out UIntPtr len);

		// Token: 0x0600050F RID: 1295
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern double luaL_checknumber(IntPtr luaState, int arg);

		// Token: 0x06000510 RID: 1296
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_checkoption(IntPtr luaState, int arg, string def, string[] list);

		// Token: 0x06000511 RID: 1297
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern void luaL_checkstack(IntPtr luaState, int sz, string message);

		// Token: 0x06000512 RID: 1298
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void luaL_checktype(IntPtr luaState, int arg, int type);

		// Token: 0x06000513 RID: 1299
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern IntPtr luaL_checkudata(IntPtr luaState, int arg, string tName);

		// Token: 0x06000514 RID: 1300
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern void luaL_checkversion_(IntPtr luaState, double ver, UIntPtr sz);

		// Token: 0x06000515 RID: 1301
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_error(IntPtr luaState, string message);

		// Token: 0x06000516 RID: 1302
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int luaL_execresult(IntPtr luaState, int stat);

		// Token: 0x06000517 RID: 1303
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_fileresult(IntPtr luaState, int stat, string fileName);

		// Token: 0x06000518 RID: 1304
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_getmetafield(IntPtr luaState, int obj, string e);

		// Token: 0x06000519 RID: 1305
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_getsubtable(IntPtr luaState, int index, string name);

		// Token: 0x0600051A RID: 1306
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern long luaL_len(IntPtr luaState, int index);

		// Token: 0x0600051B RID: 1307
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_loadbufferx(IntPtr luaState, byte[] buff, UIntPtr sz, string name, string mode);

		// Token: 0x0600051C RID: 1308
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_loadfilex(IntPtr luaState, string name, string mode);

		// Token: 0x0600051D RID: 1309
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_newmetatable(IntPtr luaState, string name);

		// Token: 0x0600051E RID: 1310
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr luaL_newstate();

		// Token: 0x0600051F RID: 1311
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void luaL_openlibs(IntPtr luaState);

		// Token: 0x06000520 RID: 1312
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern long luaL_optinteger(IntPtr luaState, int arg, long d);

		// Token: 0x06000521 RID: 1313
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern double luaL_optnumber(IntPtr luaState, int arg, double d);

		// Token: 0x06000522 RID: 1314
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int luaL_ref(IntPtr luaState, int registryIndex);

		// Token: 0x06000523 RID: 1315
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern void luaL_requiref(IntPtr luaState, string moduleName, IntPtr openFunction, int global);

		// Token: 0x06000524 RID: 1316
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void luaL_setfuncs(IntPtr luaState, [In] LuaRegister[] luaReg, int numUp);

		// Token: 0x06000525 RID: 1317
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern void luaL_setmetatable(IntPtr luaState, string tName);

		// Token: 0x06000526 RID: 1318
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern IntPtr luaL_testudata(IntPtr luaState, int arg, string tName);

		// Token: 0x06000527 RID: 1319
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern IntPtr luaL_tolstring(IntPtr luaState, int index, out UIntPtr len);

		// Token: 0x06000528 RID: 1320
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern IntPtr luaL_traceback(IntPtr luaState, IntPtr luaState2, string message, int level);

		// Token: 0x06000529 RID: 1321
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		internal static extern int luaL_typeerror(IntPtr luaState, int arg, string typeName);

		// Token: 0x0600052A RID: 1322
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void luaL_unref(IntPtr luaState, int registryIndex, int reference);

		// Token: 0x0600052B RID: 1323
		[DllImport("lua54", CallingConvention = CallingConvention.Cdecl)]
		internal static extern void luaL_where(IntPtr luaState, int level);

		// Token: 0x040001D0 RID: 464
		private const string LuaLibraryName = "lua54";
	}
}
